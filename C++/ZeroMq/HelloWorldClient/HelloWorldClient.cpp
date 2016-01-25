// HelloWorldClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include  <zmq.h>

int _tmain(int argc, _TCHAR* argv[])
{
	void *context = zmq_ctx_new();
	void *requester = zmq_socket(context, ZMQ_REQ);

	zmq_connect(requester, "tcp://localhost:5555/");
	for (int request_nbr = 0; request_nbr < 10; ++request_nbr)
	{
		zmq_msg_t request;
		zmq_msg_init_size(&request, 5);
		memcpy(zmq_msg_data(&request), "Hello", 5);
		zmq_msg_send(&request, requester, 0);
		printf("REQ %i\n", request_nbr);
		zmq_msg_close(&request);

		zmq_msg_t reply;
		zmq_msg_init(&reply);
		zmq_msg_recv(&reply, requester, 0);
		printf("REP %i\n", request_nbr);
		zmq_msg_close(&reply);
	}

	Sleep(2000);
	zmq_close(requester);
	zmq_ctx_destroy(context);

	return 0;
}

