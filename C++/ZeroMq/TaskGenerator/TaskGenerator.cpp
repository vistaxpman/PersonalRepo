// TaskGenerator.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <zmq.h>
#include <ctime>

int _tmain(int argc, _TCHAR* argv[])
{
	void *context = zmq_ctx_new();
	
	void *sender = zmq_socket(context, ZMQ_PUSH);
	zmq_bind(sender, "tcp://*:5557");

	void *sink = zmq_socket(context, ZMQ_PUSH);
	zmq_connect(sink, "tcp://localhost:5558");

	printf("Press EMTER when workers are ready...\n");
	getchar();
	printf("Sending tasks to workers...\n");

	zmq_send(sink, "0", 2, 0);

	srand(static_cast<unsigned>(time(nullptr)));

	for (int task_nbr = 0; task_nbr < 10; ++task_nbr)
	{
		int workload = static_cast<int>(static_cast<double>(rand()) / RAND_MAX * 10);
		char msg[4];
		sprintf_s(msg, 4, "%d", workload);

		zmq_send(sender, msg, 4, 0);

		printf("%s|", msg);
	}

	Sleep(1000);

	printf("Task send finished.");

	zmq_close(sink);
	zmq_close(sender);
	zmq_ctx_destroy(context);

	return 0;
}

