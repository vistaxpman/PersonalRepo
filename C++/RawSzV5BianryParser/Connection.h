#pragma once

#include "Stdafx.h"
#include "AbstractSzV5Core.h"
#include "Buffer.h"
#include "Common.h"
#include "Messages.h"

using std::chrono::system_clock;
using std::chrono::duration_cast;
using std::chrono::milliseconds;
using std::string;
using std::stringstream;
using std::tm;
using msclr::auto_gcroot;
using System::TimeSpan;
using Csc::AlgoTrading::DataFeed::Cli::SzV5::AbstractSzV5Core;

namespace Csc
{
    namespace AlgoTrading
    {
        namespace DataFeed
        {
            namespace SzV5
            {
                class Connection
                {
                public:
                    Connection(const string &host, const string &port, const string &sendCompanyId, const string &targetCompanyId, const string &password, const int32_t heartbeatInterval, AbstractSzV5Core ^core)
                        : _host(host), _port(port), _senderCompanyId(sendCompanyId), _targetCompanyId(targetCompanyId), _password(password), _heartbeatInterval(heartbeatInterval), _core(core), _running(false)
                    {
                    }

                    ~Connection()
                    {
                        _running = false;
                        WaitForMultipleObjectsEx(2, GetThreadHandles(), TRUE, INFINITE, FALSE);

                        _core.release();
                    }

                    bool Initialize()
                    {
                        addrinfo hintAddrInfo = { 0 };
                        hintAddrInfo.ai_family = AF_UNSPEC;
                        hintAddrInfo.ai_socktype = SOCK_STREAM;
                        hintAddrInfo.ai_protocol = IPPROTO_TCP;

                        addrinfo *addrInfo;
                        auto r = getaddrinfo(_host.data(), _port.data(), &hintAddrInfo, &addrInfo);
                        if (r != NO_ERROR)
                        {
                            LogFatal(ClassName, "Get addr info failed.");
                            return false;
                        }

                        auto ptr = addrInfo;
                        for (; ptr; ptr = ptr->ai_next)
                        {
                            _socket = socket(ptr->ai_family, ptr->ai_socktype, ptr->ai_protocol);
                            if (_socket == INVALID_SOCKET) {
                                LogFatal(ClassName, "Create socket failed.");
                                return false;
                            }

                            r = connect(_socket, ptr->ai_addr, static_cast<int>(ptr->ai_addrlen));
                            if (r == SOCKET_ERROR)
                            {
                                closesocket(_socket);
                                _socket = INVALID_SOCKET;
                                continue;
                            }
                            break;
                        }
                        FreeAddrInfoA(addrInfo);
                        if (!ptr)
                        {
                            LogFatal(ClassName, "Connect to server failed.");
                            closesocket(_socket);
                            _socket = INVALID_SOCKET;
                            return false;
                        }

                        LogonMessage logonMessage(_senderCompanyId, _targetCompanyId, _heartbeatInterval, _password, DefaultApplicationVersion);
                        r = send(_socket, reinterpret_cast<char*>(&logonMessage), sizeof LogonMessage, 0);
                        if (r == SOCKET_ERROR) {
                            stringstream stream;
                            stream << "Send logon message failed with error: " << WSAGetLastError() << '.';
                            LogFatal(ClassName, stream.str());
                            closesocket(_socket);
                            _socket = INVALID_SOCKET;
                            return false;
                        }

                        stringstream stream;
                        stream << "Send logon message: " << r << "bytes.";
                        LogInfo(ClassName, stream.str());

                        char receivedBuffer[Buffer::InitializeSize];
                        r = recv(_socket, receivedBuffer, sizeof receivedBuffer, 0);
                        _now = system_clock::now();

                        if (r < 0)
                        {
                            stringstream stream;
                            stream << "Recv logon message failed with error: " << WSAGetLastError() << '.';
                            LogFatal(ClassName, stream.str());
                            closesocket(_socket);
                            _socket = INVALID_SOCKET;
                            return false;
                        }

                        if (r == 0)
                        {
                            LogFatal(ClassName, "Recv logon message failed: socket is closed by server.");
                            closesocket(_socket);
                            _socket = INVALID_SOCKET;
                            return false;
                        }

                        stream.str("");
                        stream << "Recv logon message: " << r << "bytes.";
                        LogInfo(ClassName, stream.str());
                        _buffer.Append(receivedBuffer, r);

                        _running = true;
                        auto receiveThreadHandle = (HANDLE)_beginthreadex(nullptr, 0, &Connection::Receive, this, 0, nullptr);
                        if (receiveThreadHandle == 0)
                        {
                            LogFatal(ClassName, "Create recv thread failed.");
                            closesocket(_socket);
                            _socket = INVALID_SOCKET;
                            return false;
                        }

                        auto sendThreadHandle = (HANDLE)_beginthreadex(nullptr, 0, &Connection::Send, this, 0, nullptr);
                        if (sendThreadHandle == 0)
                        {
                            LogFatal(ClassName, "Create send thread failed.");
                            _running = false;
                            closesocket(_socket);
                            _socket = INVALID_SOCKET;
                            return false;
                        }

                        _threadHandles[0] = receiveThreadHandle;
                        _threadHandles[1] = sendThreadHandle;

                        LogInfo(ClassName, "Connection is initialized.");
                        return true;
                    }

                    void Clear()
                    {
                        _socket = INVALID_SOCKET;
                        _running = false;
                        _buffer.RetrieveAll();
                    }

                    HANDLE *GetThreadHandles() { return _threadHandles; }

                private:
                    static unsigned int __stdcall Receive(void *p_this)
                    {
                        auto connection = static_cast<Connection*>(p_this);
                        auto &buffer = connection->_buffer;
                        const auto &socket = connection->_socket;
                        auto &running = connection->_running;
                        auto &core = connection->_core;
                        auto &now = connection->_now;

                        do
                        {
                            while (buffer.ReadableBytes() >= sizeof Header)
                            {
                                // Header
                                auto messageBuffer = const_cast<char*>(buffer.Read(sizeof Header));
                                auto header = reinterpret_cast<const Header*>(messageBuffer);
                                auto bodyLength = ntohl(header->BodyLength);
                                auto messageSize = bodyLength + sizeof Header + sizeof checksum_t;

                                if (buffer.ReadableBytes() < messageSize)
                                {
                                    // Partail message
                                    break;
                                }

                                // Whole message
                                auto type = ntohl(header->Type);
                                stringstream stream;
                                stream << "Recv message: " << type << ", size: " << messageSize << '.';
                                LogInfo(ClassName, stream.str());
                                switch (type)
                                {
                                case 1:
                                {
                                    LogInfo(ClassName, "Logon message is received.");
                                }
                                break;

                                case 2:
                                {
                                    auto m = LogoutMessage::GetMessage(messageBuffer, messageSize);
                                    stringstream stream;
                                    stream << "Logout message is received: " << m->SessionStatus << ": " << m->Text << '.';
                                    LogInfo(ClassName, stream.str());
                                }
                                break;

                                case 3:
                                {
                                    LogInfo(ClassName, "Heartbeat message is received.");
                                }
                                break;

                                case 300111:
                                {
                                    auto quotation = SpotMarketSnapshotMessage::GetQuotation(messageBuffer, messageSize);
                                    if (quotation)
                                    {
                                        quotation->ReceivedTime = GetDateTimeFromTimePoint(now);
                                        core->OnQuotationReceived(quotation);
                                    }
                                }
                                break;

                                case 309011:
                                {
                                    auto quotation = IndexSnapshotMessage::GetQuotation(messageBuffer, messageSize);
                                    if (quotation)
                                    {
                                        quotation->ReceivedTime = GetDateTimeFromTimePoint(now);
                                        core->OnQuotationReceived(quotation);
                                    }
                                }
                                break;

                                default:
                                {
                                    LogInfo(ClassName, "Message is ignored.");
                                }
                                break;
                                }
                                buffer.Retrieve(messageSize);
                            }

                            char receivedBuffer[Buffer::InitializeSize];
                            auto r = recv(socket, receivedBuffer, sizeof receivedBuffer, 0);
                            now = system_clock::now();

                            if (r < 0)
                            {
                                stringstream stream;
                                stream << "Recv failed with error: " << WSAGetLastError() << '.';
                                LogFatal(ClassName, stream.str());
                                running = false;
                                break;
                            }

                            if (r == 0)
                            {
                                LogFatal(ClassName, "Recv failed: socket is closed by server.");
                                running = false;
                                break;
                            }

                            stringstream stream;
                            stream << "Recv: " << r << "bytes.";
                            LogInfo(ClassName, stream.str());
                            buffer.Append(receivedBuffer, r);
                        } while (running);

                        return 0;
                    }

                    static unsigned int __stdcall Send(void *p_this)
                    {
                        auto connection = static_cast<Connection*>(p_this);
                        const auto &socket = connection->_socket;
                        auto &running = connection->_running;
                        const auto &heartbeatInterval = connection->_heartbeatInterval * 1000;

                        HeartbeatMessage heartbeatMessage;

                        while (running)
                        {
                            SleepEx(heartbeatInterval, FALSE);
                            auto r = send(socket, reinterpret_cast<char*>(&heartbeatMessage), sizeof HeartbeatMessage, 0);
                            if (r == SOCKET_ERROR)
                            {
                                stringstream stream;
                                stream << "Send failed with error: " << WSAGetLastError() << '.';
                                LogFatal(ClassName, stream.str());

                                running = false;
                                break;
                            }

                            stringstream stream;
                            stream << "Send heartbeat message: " << r << "bytes.";
                            LogInfo(ClassName, stream.str());
                        }

                        return 0;
                    }

                    static DateTime GetDateTimeFromTimePoint(system_clock::time_point timePoint)
                    {
                        auto t = system_clock::to_time_t(timePoint);
                        std::tm tm = { 0 };
                        localtime_s(&tm, &t);
                        auto ms = duration_cast<milliseconds>(timePoint.time_since_epoch()).count() % 1000;

                        return DateTime(1900 + tm.tm_year, 1 + tm.tm_mon, tm.tm_mday, tm.tm_hour, tm.tm_min, tm.tm_sec) +
                            TimeSpan::FromMilliseconds((double)ms);
                    }

                private:
                    static const string DefaultApplicationVersion;
                    static const char *ClassName;

                    string _host, _port, _password, _senderCompanyId, _targetCompanyId;
                    int32_t _heartbeatInterval;
                    SOCKET _socket = INVALID_SOCKET;

                    Buffer _buffer;

                    volatile bool _running;
                    HANDLE _threadHandles[2];
                    
                    system_clock::time_point _now;

                    auto_gcroot<AbstractSzV5Core^> _core;
                };
            }
        }
    }
}