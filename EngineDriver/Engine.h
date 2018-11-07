#include <inttypes.h>
#include <stdbool.h>
#include "SpeedCounter.h"
#include "PIDController.h"

#ifndef ENGINE_H_
#define ENGINE_H_

enum EngineState { UNKNOWN = 0, STOP = 1, FORWARD = 2, BACKWARD = 3 };

struct EnginePins {
  uint8_t enabled;
  uint8_t forward;
  uint8_t backward;
};

struct Engine 
{
  volatile enum EngineState state;
  volatile uint16_t distance;
  volatile uint16_t fullDistance;
  volatile uint16_t speed;
  uint8_t pwm;
  volatile bool signaled;
  struct EnginePins pins;
  struct SpeedCounter speedCounter;
  struct PIDController pidController;
};

void Engine_initialize(struct Engine*, uint8_t enabledPin, uint8_t forwardPin, uint8_t backwardPin);
void Engine_setState(struct Engine*, enum EngineState);
void Engine_stop(struct Engine*);
void Engine_forward(struct Engine*, uint16_t setSpeed);
void Engine_backward(struct Engine*, uint16_t setSpeed);
void Engine_speed(struct Engine*, uint8_t);
void Engine_pwm(struct Engine*, uint8_t);
void Engine_encoderPulse(struct Engine*);
void Engine_control(struct Engine*);
void Engine_setPID(struct Engine*, uint16_t, uint16_t, uint16_t);
void Engine_setSpeed(struct Engine* engine, int16_t setSpeed);
float Engine_calcDistance(struct Engine* engine);
void Engine_calcDistance2(struct Engine* engine, char *distanceStr);
//uint32_t Engine_calcDistance(struct Engine* engine);

#endif
