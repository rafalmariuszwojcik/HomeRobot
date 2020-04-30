#include <inttypes.h>
#include <stdbool.h>

#ifndef INPUT_H_
#define INPUT_H_

const uint8_t InputBufferSize = 64;

struct InputBuffer {
  int8_t dataIndex;
  char[InputBufferSize] data;
};

char* Input_GetData();

#endif