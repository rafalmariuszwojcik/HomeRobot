#include <Arduino.h>
#include "Engine.h"

void _Engine_initPins(struct Engine* engine);
  
void Engine_initialize(struct Engine* engine, uint8_t enabledPin, uint8_t forwardPin, uint8_t backwardPin)
{
  memset(engine, 0, sizeof(struct Engine));
  engine->state = UNKNOWN;
  engine->pins.enabled = enabledPin;
  engine->pins.forward = forwardPin;
  engine->pins.backward = backwardPin;
  _Engine_initPins(engine);
  Engine_stop(engine);
  SpeedCounter_initialize(&(engine->speedCounter), 6);
  PIDController_initialize(&(engine->pidController), 3, 6, 0, 10, 0, 255);
}

void Engine_setState(struct Engine* engine, enum EngineState state) {
  if (engine->state != state)
  {
    switch (state) 
    {
      case STOP:
        Engine_stop(engine);
        break;

      case FORWARD:
        Engine_forward(engine, 0);
        break;

      case BACKWARD:
        Engine_backward(engine, 0);
        break;
    }

    engine->state = state;
  }
}

void Engine_stop(struct Engine* engine) 
{
  digitalWrite(engine->pins.enabled, LOW);
  digitalWrite(engine->pins.forward, LOW);
  digitalWrite(engine->pins.backward, LOW);
  engine->pwm = 0;
  engine->speed = 0;
  engine->signaled = true;
}

void Engine_forward(struct Engine* engine, uint16_t setSpeed) 
{
  digitalWrite(engine->pins.backward, LOW);
  digitalWrite(engine->pins.forward, HIGH);
  digitalWrite(engine->pins.enabled, HIGH);
  engine->distance = 0;
  engine->speed = setSpeed != 0 ? setSpeed : 25; 
  engine->signaled = true; 
}

void Engine_backward(struct Engine* engine, uint16_t setSpeed) 
{
  digitalWrite(engine->pins.forward, LOW);
  digitalWrite(engine->pins.backward, HIGH);
  digitalWrite(engine->pins.enabled, HIGH);
  engine->distance = 0;
  engine->speed = setSpeed != 0 ? setSpeed : 25; 
  engine->signaled = true;
}

void Engine_speed(struct Engine* engine, uint8_t setSpeed) 
{
  if (engine->state == FORWARD || engine->state == BACKWARD)
  {
    engine->speed = setSpeed;
    PIDController_reset(&(engine->pidController));
  }
}

void Engine_pwm(struct Engine* engine, uint8_t pwm) 
{
  if (engine->state == FORWARD || engine->state == BACKWARD)
  {
    analogWrite(engine->pins.enabled, pwm);
    engine->pwm = pwm;
  }
}

void Engine_encoderPulse(struct Engine* engine) 
{
  engine->distance++;
  if (engine->state == FORWARD) engine->fullDistance++;
  else if (engine->state == BACKWARD) engine->fullDistance--;
  SpeedCounter_calc(&(engine->speedCounter));
  engine->signaled = true;
}

void Engine_control(struct Engine* engine) 
{
  SpeedCounter_control(&(engine->speedCounter));
  uint8_t pwm = (uint8_t)(PIDController_compute(&(engine->pidController), (float)(engine->speed), (float)(engine->speedCounter.avg_speed)));
  Engine_pwm(engine, pwm);
}

void Engine_setPID(struct Engine* engine, uint16_t kp, uint16_t ki, uint16_t kd)
{
  PIDController_setPID(&(engine->pidController), kp, ki, kd);
}

void Engine_setSpeed(struct Engine* engine, int16_t setSpeed)
{
  if (setSpeed == 0) 
  {
    Engine_stop(engine);
    engine->state = STOP;
  }
  else if (setSpeed > 0) 
  {
    if (engine->state != FORWARD)
    {
      Engine_forward(engine, abs(setSpeed));
      engine->state = FORWARD;
    }
    else
    {
      Engine_speed(engine, abs(setSpeed));
    }
  }
  else if (setSpeed < 0)
  {
    if (engine->state != BACKWARD)
    {
      Engine_backward(engine, abs(setSpeed));
      engine->state = BACKWARD;
    }
    else 
    {
      Engine_speed(engine, abs(setSpeed));
    }
  }
}

float Engine_calcDistance(struct Engine* engine)
{
  float distance = (float)(engine->fullDistance);
  uint32_t one_signal_time = engine->speedCounter.one_signal_time;
  uint32_t prev_micros = engine->speedCounter.prev_micros;
  if (one_signal_time != 0 && prev_micros != 0)
  {
    uint32_t curr_micros = micros();
    uint32_t timeDelta = curr_micros - prev_micros;
    distance = /*distance +*/ (float)((float)timeDelta / (float)one_signal_time);
  }

  return distance;
}

void Engine_calcDistance2(struct Engine* engine, char *distanceStr)
{
  uint32_t distance = engine->fullDistance;
  uint32_t one_signal_time = engine->speedCounter.one_signal_time;
  uint32_t prev_micros = engine->speedCounter.prev_micros;
  if (one_signal_time != 0 && prev_micros != 0)
  {
    uint32_t timeDelta = micros() - prev_micros;
    uint32_t d1 = (timeDelta * 10000) / one_signal_time;
    ltoa(d1, distanceStr, 10);  
  }
}

/*
uint32_t Engine_calcDistance(struct Engine* engine)
{
  uint32_t distance = 0;
  uint32_t one_signal_time = engine->speedCounter.one_signal_time;
  uint32_t prev_micros = engine->speedCounter.prev_micros;
  if (one_signal_time != 0 && prev_micros != 0)
  {
    uint32_t curr_micros = micros();
    uint32_t timeDelta = curr_micros - prev_micros;
    //distance = distance + (float)((float)timeDelta / (float)one_signal_time);
    distance = timeDelta;
  }
  
  return distance;
  //return (10 % 17);
}*/

void _Engine_initPins(struct Engine* engine) 
{
  pinMode(engine->pins.enabled, OUTPUT);
  pinMode(engine->pins.forward, OUTPUT);
  pinMode(engine->pins.backward, OUTPUT);
}
