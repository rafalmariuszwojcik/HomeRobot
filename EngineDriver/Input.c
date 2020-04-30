#include <string.h>
#include <stdbool.h>
#include "Input.h"

struct InputBuffer _buffer;
bool _Input_IsDataValid(char data);

void Input_Initialize(uint8_t (*available_ptr)(), char (*read_ptr)())
{
  memset(&_buffer, 0, sizeof(struct InputBuffer));
  _buffer.available_ptr = available_ptr;
  _buffer.read_ptr = read_ptr;
  _buffer.dataIndex = -1;
}

char* Input_GetData() 
{
  while (_buffer.available_ptr != NULL && _buffer.available_ptr() > 0)
  {
    char data = _buffer.read_ptr != NULL ? _buffer.read_ptr() : 0;
    if (_Input_IsDataValid(data) && _buffer.dataIndex <= _INPUT_BUFFER_SIZE - 3)
    {
      _buffer.data[++_buffer.dataIndex] = data;
      _buffer.data[_buffer.dataIndex + 1] = 0;
    }
    else 
    {
      _buffer.dataIndex = -1;
      break;
    }
  }

  if (_buffer.dataIndex >= 0 && _buffer.data[_buffer.dataIndex] == ';')
  {
    _buffer.dataIndex = -1;
    return &(_buffer.data[0]);
  }
  else 
  {
    return NULL;
  }
}

bool _Input_IsDataValid(char data)
{
  return (data == ',') || (data == ';') || (data == 32) || (data >= 48 && data <= 57) || (data >= 65 && data <= 90) || (data >= 97 && data <= 122);
}
