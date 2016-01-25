// HelloWorldServer.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <zmq.h>

int _tmain(int argc, _TCHAR* argv[])
{
	void *context = zmq_ctx_new();
	void *responder = zmq_socket(context, ZMQ_REP);
	zmq_bind(responder, "tcp://*:5555");

	while (true)
	{
		zmq_msg_t request;
		zmq_msg_init(&request);
		zmq_msg_recv(&request, responder, 0);
		zmq_msg_close(&request);

		Sleep(1000);

		zmq_msg_t reply;
		zmq_msg_init_size(&reply, 5);
		memcpy(zmq_msg_data(&reply), "World", 5);
		zmq_msg_send(&reply, responder, 0);
		zmq_msg_close(&reply);
	}

	zmq_close(responder);
	zmq_ctx_destroy(context);

	return 0;
}
