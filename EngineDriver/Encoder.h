#include <inttypes.h>
#include <stdbool.h>

#ifndef ENCODER_H_
#define ENCODER_H_

struct Encoder {
  volatile bool signaled;	
  volatile uint32_t micros;
  volatile uint8_t count;
};

/*
This function should be called from encoder interruption.
It stores information about interruption in given Encoder structure. 
*/
void Encoder_signal(struct Encoder*);

/*
Get current state of referenced Encoder.
Should be called from main programm.
*/
struct Encoder Encoder_getStateAndReset(struct Encoder*);

#endif
