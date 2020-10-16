#include <string.h>
#include "encoder.h"

/*
Initialize encoder instance. 
*/
void Encoder_Initialize(
  struct Encoder* encoder, 
  uint32_t (*get_micros_ptr)(), 
  uint16_t (*digitalRead_ptr)(uint16_t),
  void (*atomic_ptr)(void (*func)(void* object), void* object),
  uint8_t pin)
{
  memset(encoder, 0, sizeof(struct Encoder));
  encoder->get_micros_ptr = get_micros_ptr;
  encoder->digitalRead_ptr = digitalRead_ptr;
  encoder->atomic_ptr = atomic_ptr;
  encoder->pin = pin;
}

void Encoder_setTimes();

/*
This function should be called from encoder interruption.
It stores information about interruption in given Encoder structure. 
*/
void Encoder_signal(struct Encoder* encoder) 
{
  if (!encoder->signaled) 
  {
    if (encoder->get_micros_ptr != NULL)
    {
      if (encoder->times.currentTime.hasValue)
      {
        encoder->times.lastTime.micros = encoder->times.currentTime.micros;
        encoder->times.lastTime.hasValue = encoder->times.currentTime.hasValue;
      }

      encoder->times.currentTime.micros = encoder->get_micros_ptr();
      encoder->times.currentTime.hasValue = true;
    }
  }

  encoder->level = encoder->digitalRead_ptr != NULL ? encoder->digitalRead_ptr(encoder->pin) : 0;
  encoder->signaled = true;	
  encoder->count++;
};

struct EncoderStateResult 
{
  struct EncoderState result;
  struct Encoder* encoder;
};

void Encoder_getState(void* object)
{
  struct EncoderStateResult* state = object;
  state->result.signaled = state->encoder->signaled; 
  state->result.micros = state->encoder->times.currentTime.micros; 
  state->result.count = state->encoder->count;
  state->encoder->signaled = false;
  state->encoder->count = 0; 

  ////printf("value = %i\n", *(int*)object);
}

/*
Get current state of referenced Encoder and reset it
Should be called from main programm.
*/
struct EncoderState Encoder_getStateAndReset(struct Encoder* encoder) 
{
  struct EncoderStateResult state;
  state.result.signaled = false;
  
  if (encoder->signaled) {
    state.encoder = encoder;
    encoder->atomic_ptr(Encoder_getState, &state);
    ////ATOMIC_BLOCK(ATOMIC_RESTORESTATE) {
 //     result.signaled = encoder->signaled; 
 //     result.micros = encoder->micros; 
 //     result.count = encoder->count;
 //     encoder->signaled = false;
 //     encoder->count = 0; 
    ////}
  }

  return state.result;
};