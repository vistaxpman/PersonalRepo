#include "Stdafx.h"
#include "AbstractCtpCore.h"

#include "Common.h"
#include "CtpMdSpi.h"

using std::stringstream;

namespace Csc
{
    namespace AlgoTrading
    {
        namespace DataFeed
        {
            namespace Cli
            {
                namespace Ctp
                {
                    AbstractCtpCore::AbstractCtpCore(String ^host, int port, String ^ broker, String ^user, String ^password)
                        : _host(host), _port(port), _broker(broker), _user(user), _password(password), _api(CThostFtdcMdApi::CreateFtdcMdApi())
                    {
                        _spi = new CtpMdSpi(_api, this);
                        _api->RegisterSpi(_spi);

                        stringstream stream;
                        stream << "tcp://" << GetStdStringFromClrString(_host) << ":" << _port;
                        _api->RegisterFront(const_cast<char*>(stream.str().data()));
                    }

                    AbstractCtpCore::~AbstractCtpCore()
                    {
                        _api->RegisterSpi(nullptr);
                        _api->Release();
                        delete _spi;
                    }
                }
            }
        }
    }
}