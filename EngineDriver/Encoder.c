#include <Arduino.h>
#include <util/atomic.h>
#include "Encoder.h"

/*
This function should be called from encoder interruption.
It stores information about interruption in given Encoder structure. 
*/
void Encoder_signal(struct Encoder* encoder) {
  if (!encoder->signaled) encoder->micros = micros();
  encoder->signaled = true;	
  encoder->count++;
};

/*
Get current state of referenced Encoder and reset it
Should be called from main programm.
*/
struct Encoder Encoder_getStateAndReset(struct Encoder* encoder) {
  struct Encoder result;
  result.signaled = false;
  if (encoder->signaled) {
    ATOMIC_BLOCK(ATOMIC_RESTORESTATE) {
      result.signaled = encoder->signaled; 
      result.micros = encoder->micros; 
      result.count = encoder->count;
      encoder->signaled = false;
      encoder->count = 0; 
    }
  }

  return result;
};
