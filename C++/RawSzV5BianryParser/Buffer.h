#pragma once

#include "Stdafx.h"

using std::vector;

namespace Csc
{
    namespace AlgoTrading
    {
        namespace DataFeed
        {
            namespace SzV5
            {
                class Buffer
                {
                public:
                    static const size_t InitializeSize = 1024;

                    explicit Buffer(size_t initialize_size = InitializeSize)
                        : _buffer(initialize_size)
                    {
                        assert(ReadableBytes() == 0);
                        assert(WritableBytes() == initialize_size);
                    }

                    size_t ReadableBytes() const
                    {
                        return _writerIndex - _readerIndex;
                    }

                    size_t WritableBytes() const
                    {
                        return _buffer.size() - _writerIndex;
                    }

                    void Retrieve(size_t length)
                    {
                        assert(length <= ReadableBytes());
                        if (length < ReadableBytes())
                        {
                            _readerIndex += length;
                            if (_readerIndex == _writerIndex)
                            {
                                RetrieveAll();
                            }
                        }
                        else
                        {
                            RetrieveAll();
                        }
                    }

                    void RetrieveAll()
                    {
                        _readerIndex = _writerIndex = 0;
                    }

                    void Append(const char *data, size_t length)
                    {
                        EnsureWritableBytes(length);
                        memcpy_s(WritableBegin(), length, data, length);
                        HasWritten(length);
                    }

                    void Append(const void *data, size_t length)
                    {
                        Append(static_cast<const char*>(data), length);
                    }

                    const char *Read(size_t length) const
                    {
                        assert(ReadableBytes() >= length);
                        return ReadableBegin();
                    }
                private:
                    void EnsureWritableBytes(size_t length)
                    {
                        if (WritableBytes() < length)
                        {
                            MakeSpace(length);
                        }
                        assert(WritableBytes() >= length);
                    }

                    char *WritableBegin()
                    {
                        return Begin() + _writerIndex;
                    }

                    const char *ReadableBegin() const
                    {
                        return Begin() + _readerIndex;
                    }

                    char *Begin()
                    {
                        return &*_buffer.begin();
                    }

                    const char *Begin() const
                    {
                        return &*_buffer.begin();
                    }

                    void MakeSpace(size_t length)
                    {
                        assert(WritableBytes() < length);

                        auto readable = ReadableBytes();
                        if (readable > 0)
                        {
                            memcpy_s(Begin(), readable, ReadableBegin(), readable);
                            _readerIndex = 0;
                            _writerIndex = readable;
                        }
                        assert(readable == ReadableBytes());

                        if (WritableBytes() < length)
                        {
                            _buffer.resize(_writerIndex + length);
                        }
                    }

                    void HasWritten(size_t length)
                    {
                        assert(length <= WritableBytes());
                        _writerIndex += length;
                    }

                private:
                    vector<char> _buffer;
                    size_t _readerIndex = 0;
                    size_t _writerIndex = 0;
                };
            }
        }
    }
}