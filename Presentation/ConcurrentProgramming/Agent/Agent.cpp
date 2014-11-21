#include <array>
#include <algorithm>
#include <iostream>

#include <tchar.h>
#include <agents.h>

using namespace Concurrency;
using namespace std;

// Demonstrates a basic agent that produces values.
class Producer : public agent
{
public:
    explicit Producer(ITarget<double>& target)
        : _target(target)
    {
    }

protected:
    void run()
    {
        // For illustration, create a predefined array of stock quotes.
        // A real-world application would read these from an external source,
        // such as a network connection or a database.
        array<double, 6> quotes = { 24.44, 24.65, 24.99, 23.76, 22.30, 25.89 };

        // Send each quote to the target buffer.
        for_each(begin(quotes), end(quotes),
            [&] (double quote)
        {
            send(_target, quote);
            // Pause before sending the next quote.
            Concurrency::wait(20);
        });

        // Send a negative value to indicate the end of processing.
        send(_target, -1.0);

        // Set the agent to the finished state.
        done();
    }

private:
    // The target buffer to write to.
    ITarget<double>& _target;
};

// Demonstrates a basic agent that consumes values.
class Consumer : public agent
{
public:
    explicit Consumer(ISource<double>& source)
        : _source(source)
    {
    }

protected:
    void run()
    {
        // Read quotes from the source buffer until we receive
        // a negative value.
        double quote;
        while ((quote = receive(_source)) >= 0.0)
        {
            // Print the quote.
            wcout.setf(ios::fixed);
            wcout.precision(2);
            wcout << L"Current quote is " << quote << L'.' << endl;

            // Pause before reading the next quote.
            Concurrency::wait(10);
        }

        // Set the agent to the finished state.
        done();
    }

private:
    // The source buffer to read from.
    ISource<double>& _source;
};

int _tmain(int argc, _TCHAR* argv[])
{
    // A message buffer that is shared by the agents.
    //overwrite_buffer<double> buffer;
    unbounded_buffer<double> buffer;

    // Create and start the producer and consumer agents.
    Producer producer(buffer);
    Consumer consumer(buffer);
    producer.start();
    consumer.start();

    // Wait for the agents to finish.
    agent::wait(&producer);
    agent::wait(&consumer);

    ::getchar();
    return 0;
}