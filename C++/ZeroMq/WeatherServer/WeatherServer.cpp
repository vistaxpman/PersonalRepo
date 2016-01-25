// WeatherServer.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <zmq.h>
#include <ctime>

int _tmain(int argc, _TCHAR* argv[])
{
	void *context = zmq_ctx_new();
	void *publisher = zmq_socket(context, ZMQ_PUB);

	zmq_bind(publisher, "tcp://*:5556");

	srand(static_cast<unsigned>(time(nullptr)));
	while (true)
	{
		int zipcode, temperature, humidity;
		zipcode = static_cast<int>(static_cast<double>(rand()) / RAND_MAX * 100 + 100000);
		temperature = static_cast<int>(static_cast<double>(rand()) / RAND_MAX * 215 - 80);
		humidity = static_cast<int>(static_cast<double>(rand()) / RAND_MAX * 50 + 10);

		char update[20];
		sprintf_s(update, 20, "%05d %d %d", zipcode, temperature, humidity);
		zmq_send(publisher, update, strlen(update), 0);
	}

	zmq_close(publisher);
	zmq_ctx_destroy(context);

	return 0;
}

