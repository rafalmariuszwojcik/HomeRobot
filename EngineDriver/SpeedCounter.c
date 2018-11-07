#include <Arduino.h>
#include <stdlib.h>
#include "SpeedCounter.h"

#define ONE_SECOND 1000000UL 
#define MAX_SPEED 100;

uint8_t static _SpeedCounter_calc(struct SpeedCounter*, uint8_t);
void static _SpeedCounter_clear(struct SpeedCounter*);

void SpeedCounter_initialize(struct SpeedCounter* speedCounter, uint8_t filter_Level)
{
   memset(speedCounter, 0, sizeof(struct SpeedCounter));
   speedCounter->filter_Level = filter_Level;
   speedCounter->max_speed = MAX_SPEED;
}

uint8_t SpeedCounter_control(struct SpeedCounter* speedCounter)
{
  return _SpeedCounter_calc(speedCounter, 0);
}

uint8_t SpeedCounter_calc(struct SpeedCounter* speedCounter)
{
  return _SpeedCounter_calc(speedCounter, 1);
}

uint8_t static _SpeedCounter_calc(struct SpeedCounter* speedCounter, uint8_t pulse)
{
  uint8_t signaled = 0;
  uint32_t curr_micros = micros();
  uint32_t timeDelta = curr_micros - speedCounter->prev_micros;
  if (speedCounter->prev_micros != 0 && timeDelta != 0)
  {
    uint32_t spd = (ONE_SECOND * 100) / (timeDelta);
    uint32_t curr_speed = (spd % 100) >= 50 ? (spd / 100) + 1 : (spd / 100);
    curr_speed = curr_speed > speedCounter->max_speed ? speedCounter->max_speed : curr_speed;
    if (pulse || curr_speed <= (uint32_t)speedCounter->curr_speed)
    {
      if (pulse)
      {
        speedCounter->one_signal_time = timeDelta;
      }
      
      speedCounter->curr_speed = (uint16_t)curr_speed;
      uint8_t fLevel = speedCounter->filter_Level < 2 ? 2 : speedCounter->filter_Level;
      uint32_t avg_spd = ((speedCounter->avg_speed_internal * (fLevel - 1)) + spd);
      avg_spd = (avg_spd % fLevel) >= (fLevel / 2) ? (avg_spd / fLevel) + 1 : (avg_spd / fLevel);
      speedCounter->avg_speed_internal = avg_spd;
      speedCounter->avg_speed = (uint16_t)((avg_spd % 100) >= 50 ? (avg_spd / 100) + 1 : (avg_spd / 100));
      signaled = 1;
    }
  }
  else 
  {
    _SpeedCounter_clear(speedCounter);
    signaled = 1;
  }

  if (pulse)
  {
    speedCounter->prev_micros = curr_micros != 0 ? curr_micros : curr_micros + 1;
  }

  return signaled;
}

void static _SpeedCounter_clear(struct SpeedCounter* speedCounter)
{
  speedCounter->curr_speed = 0;
  speedCounter->avg_speed = 0;
  speedCounter->avg_speed_internal = 0;
  speedCounter->one_signal_time = 0;
}
