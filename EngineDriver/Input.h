#include <inttypes.h>

#ifndef INPUT_H_
#define INPUT_H_

enum { INPUT_BUFFER_SIZE = 64, INPUT_MAX_BUFFER_INDEX = INPUT_BUFFER_SIZE - 2 };

struct InputBuffer {
  uint8_t (*available_ptr)();
  char (*read_ptr)();
  int8_t dataIndex;
  char data[INPUT_BUFFER_SIZE];
};

void Input_Initialize(uint8_t (*available_ptr)(), char (*read_ptr)());
char* Input_GetData();

#endif
