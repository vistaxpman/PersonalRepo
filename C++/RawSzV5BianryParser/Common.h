#pragma once

#include "Stdafx.h"
#using "..\packages\NLog.4.3.3\lib\net45\NLog.dll"

using std::string;
using System::IntPtr;
using System::Runtime::InteropServices::Marshal;
using System::String;
using NLog::ILogger;
using NLog::LogManager;

inline string GetStdStringFromClrString(String ^clrString)
{
    IntPtr ptr = Marshal::StringToHGlobalAnsi(clrString);
    string std(static_cast<char*>(ptr.ToPointer()));
    Marshal::FreeHGlobal(ptr);

    return std;
}

// Trace
inline void LogTrace(String ^loggerName, String ^log)
{
    auto logger = LogManager::GetLogger(loggerName);
    logger->Trace(log);
}

inline void LogTrace(string &&loggerName, string &&log)
{
    LogTrace(gcnew String(loggerName.data()), gcnew String(log.data()));
}

inline void LogTrace(const string &loggerName, const string &log)
{
    LogTrace(gcnew String(loggerName.data()), gcnew String(log.data()));
}

// Debug
inline void LogDebug(String ^loggerName, String ^log)
{
    auto logger = LogManager::GetLogger(loggerName);
    logger->Debug(log);
}

inline void LogDebug(string &&loggerName, string &&log)
{
    LogDebug(gcnew String(loggerName.data()), gcnew String(log.data()));
}

inline void LogDebug(const string &loggerName, const string &log)
{
    LogDebug(gcnew String(loggerName.data()), gcnew String(log.data()));
}


// Info
inline void LogInfo(String ^loggerName, String ^log)
{
    auto logger = LogManager::GetLogger(loggerName);
    logger->Info(log);
}

inline void LogInfo(string &&loggerName, string &&log)
{
    LogInfo(gcnew String(loggerName.data()), gcnew String(log.data()));
}

inline void LogInfo(const string &loggerName, const string &log)
{
    LogInfo(gcnew String(loggerName.data()), gcnew String(log.data()));
}

// Warn
inline void LogWarn(String ^loggerName, String ^log)
{
    auto logger = LogManager::GetLogger(loggerName);
    logger->Warn(log);
}

inline void LogWarn(string &&loggerName, string &&log)
{
    LogWarn(gcnew String(loggerName.data()), gcnew String(log.data()));
}

inline void LogWarn(const string &loggerName, const string &log)
{
    LogWarn(gcnew String(loggerName.data()), gcnew String(log.data()));
}

// Error
inline void LogError(String ^loggerName, String ^log)
{
    auto logger = LogManager::GetLogger(loggerName);
    logger->Error(log);
}

inline void LogError(string &&loggerName, string &&log)
{
    LogError(gcnew String(loggerName.data()), gcnew String(log.data()));
}

inline void LogError(const string &loggerName, const string &log)
{
    LogError(gcnew String(loggerName.data()), gcnew String(log.data()));
}

// Fatal
inline void LogFatal(String ^loggerName, String ^log)
{
    auto logger = LogManager::GetLogger(loggerName);
    logger->Fatal(log);
}

inline void LogFatal(string &&loggerName, string &&log)
{
    LogFatal(gcnew String(loggerName.data()), gcnew String(log.data()));
}

inline void LogFatal(const string &loggerName, const string &log)
{
    LogFatal(gcnew String(loggerName.data()), gcnew String(log.data()));
}