#pragma once

#include "Stdafx.h"

using System::String;
using Csc::AlgoTrading::Common::Quotation;

namespace Csc
{
    namespace AlgoTrading
    {
        namespace DataFeed
        {
            namespace Ctp
            {
                class CtpMdSpi;
            }

            namespace Cli
            {
                namespace Ctp
                {
                    using Csc::AlgoTrading::DataFeed::Ctp::CtpMdSpi;

                    public ref class AbstractCtpCore abstract
                    {
                    public:
                        AbstractCtpCore(String ^host, int port, String ^ broker, String ^user, String ^password);
                        ~AbstractCtpCore();

                        String ^GetBroker() { return _broker; }
                        String ^GetUser() { return _user; }
                        String ^GetPassword() { return _password; }
                        virtual array<String^>^ GetSymblos() abstract;

                        bool Initialize()
                        {
                            _api->Init();
                            return true;
                        };

                        virtual void OnConnected() abstract;

                        virtual void OnDisconnected(int errorCode, String ^reason) abstract;

                        virtual void OnUserLogin(int errorCode, String ^reason) abstract;

                        virtual void OnSubMarketData(int errorCode, String ^reason, String ^symbol) abstract;

                        virtual void OnError(int errorCode, String ^reason) abstract;

                        virtual void OnQuotationReceived(Quotation ^quotation) abstract;

                    private:
                        String ^_host;
                        int _port;
                        String ^_broker;
                        String ^_user;
                        String ^_password;
                        array<String^> ^_symbols;

                        CThostFtdcMdApi *_api;
                        CtpMdSpi *_spi;
                    };
                }
            }
        }
    }
}
