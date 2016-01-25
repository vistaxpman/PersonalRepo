// WeatherClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include  <zmq.h>

static char *
s_recv(void *socket) {
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
	void *subscriber = zmq_socket(context, ZMQ_SUB);

	zmq_connect(subscriber, "tcp://localhost:5556");
	char *topic = "100001";
	zmq_setsockopt(subscriber, ZMQ_SUBSCRIBE, topic, strlen(topic));

	for (int update_nbr = 0; update_nbr < 100; ++update_nbr)
	{
		char* msg = s_recv(subscriber);
		int zipcode, temperature, humidity;
		sscanf_s(msg, "%d %d %d", &zipcode, &temperature, &humidity);

		printf("Weather on update[%2d]: %d, %d, %d.\n", update_nbr, zipcode, temperature, humidity);
	}
	return 0;
}

