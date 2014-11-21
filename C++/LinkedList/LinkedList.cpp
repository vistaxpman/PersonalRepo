// Compile: CL.EXE .\LinkedList.cpp

#include <sstream>
#include <iostream>
#include <tchar.h>
#include <crtdbg.h>

using namespace std;

template <typename T>
struct Node
{
	T Value;
	Node *Next;
};

template <typename T>
Node<T> * BuildList(T initValue[], size_t size)
{
	if ((size == 0) || (initValue == NULL))
	{
		return NULL;
	}

	Node<T> *head, *temp;
	head = temp = NULL;
	for (unsigned int i = 0; i < size; i++)
	{
		head = new Node<T>;
		head->Value = initValue[i];

		head->Next = temp;
		temp = head;
	}

	return head;
};

template <typename T>
void FreeList(Node<T> *list)
{
	Node<T> *temp = list;
	while (temp != NULL)
	{
		Node<T> *node = temp->Next;
		delete temp;
		temp = node;
	}
}

template <typename T>
string ConvertToString(Node<T> *list)
{
	stringstream result;
	Node<T> *temp = list;
	while (temp != NULL)
	{
		result << temp->Value;
		temp = temp->Next;
	}

	return result.str();
}

template <typename T>
Node<T> * ReverseList(Node<T> *list)
{
	if (list == NULL)
	{
		return NULL;
	}

	if (list->Next == NULL)
	{
		return list;
	}

	Node<T> *prev, *curr, *next;
	prev = NULL;
	curr = list;
	next = list->Next;
	while (next != NULL)
	{
		next = curr->Next;
		curr->Next = prev;
		prev = curr;
		curr = next;
	}

	return prev;
}

int _tmain(int argc, _TCHAR* argv[])
{
	cout << "TEST FOR NULL LIST" << endl;
	_ASSERT(ReverseList<int>(NULL) == NULL);
	cout << "PASSED!" << endl;

	cout << "TEST FOR 2-elements LIST" << endl;
	int anotherInitValue[] = {0};
	Node<int> *list = BuildList(anotherInitValue, 1);
	_ASSERT(ReverseList(list) == list);
	FreeList(list);
	cout << "PASSED!" << endl;

	cout << "TEST FOR normal LIST" << endl;
	int initValue[] = {0, 1, 2, 3};
	list = BuildList(initValue, 4);
	string result = ConvertToString(list);
	_ASSERT(result == "3210");
	list = ReverseList(list);
	string result1 = ConvertToString(list);
	_ASSERT(result1 == "0123");
	FreeList(list);
	cout << "PASSED!" << endl;

	return 0;
}
