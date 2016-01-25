// TaskWorker.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <zmq.h>

static char * s_recv(void *socket) {
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
	zmq_connect(receiver, "tcp://localhost:5557");

	void *sender = zmq_socket(context, ZMQ_PUSH);
	zmq_connect(sender, "tcp://localhost:5558");

	while (true)
	{
		char *string = s_recv(receiver);

		fflush(stdout);
		printf("%s|", string);

		Sleep(atoi(string) * 1000);

		zmq_send(sender, string, strlen(string), 0);
	}

	zmq_close(receiver);
	zmq_close(sender);
	zmq_ctx_destroy(context);

	return 0;
}

