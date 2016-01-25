// TaskSink.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <zmq.h>

static char *s_recv(void *socket) {
	char buffer[256];
	int size = zmq_recv(socket, buffer, 255, 0);
	if (size == -1)
		return nullptr;
	if (size > 255)
		size = 255;
	buffer[size] = 0;
	return _strdup(buffer);
}

int _tmain(int argc, _TCHAR* argv[])
{
	void *context = zmq_ctx_new();

	void *receiver = zmq_socket(context, ZMQ_PULL);
	zmq_bind(receiver, "tcp://*:5558");

	char *string = s_recv(receiver);
	printf("Task send starts.");
	free(string);

	for (int task_nbr = 0; task_nbr < 10; ++task_nbr)
	{
		string = s_recv(receiver);
		printf("%s|", string);
		free(string);
	}
	
	printf("Task send finished.");

	zmq_close(receiver);
	zmq_ctx_destroy(context);

	return 0;
}

