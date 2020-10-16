#include <inttypes.h>
#include <stdbool.h>

#ifndef ENCODER_H_
#define ENCODER_H_

struct EncoderTime
{
  bool hasValue;
  uint32_t micros;
};

struct EncoderStateTime
{
  struct EncoderTime lastTime;
  struct EncoderTime currentTime;
};

struct Encoder 
{
  uint32_t (*get_micros_ptr)();
  uint16_t (*digitalRead_ptr)(uint16_t);
  void (*atomic_ptr)(void (*func)(void* object), void* object);
  uint8_t pin;
  volatile bool signaled;	
  volatile uint8_t level;
  //volatile struct EncoderStateTime lowTime;
  //volatile struct EncoderStateTime highTime;
  volatile struct EncoderStateTime times;
  //volatile uint32_t micros;
  //volatile uint32_t millis;
  volatile uint8_t count;
};

struct EncoderState 
{
  bool signaled;
  uint32_t micros;
  uint8_t count;
};

/*
Initialize encoder instance. 
*/
void Encoder_Initialize(
  struct Encoder* encoder, 
  uint32_t (*get_micros_ptr)(), 
  uint16_t (*digitalRead_ptr)(uint16_t),
  void (*atomic_ptr)(void (*func)(void* object), void* object),
  uint8_t pin);

/*
This function should be called from encoder interruption.
It stores information about interruption in given Encoder structure. 
*/
void Encoder_signal(struct Encoder*);

/*
Get current state of referenced Encoder.
Should be called from main programm.
*/
struct EncoderState Encoder_getStateAndReset(struct Encoder*);

#endif