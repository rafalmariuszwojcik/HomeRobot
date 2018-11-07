#include <inttypes.h>

#ifndef SPEEDCOUNTER_H_
#define SPEEDCOUNTER_H_

struct SpeedCounter {
  volatile uint16_t curr_speed;
  volatile uint16_t avg_speed;
  volatile uint16_t max_speed;
  uint8_t filter_Level;
  volatile uint32_t avg_speed_internal;
  volatile uint32_t prev_micros;
  volatile uint32_t one_signal_time; // in micros.
};

uint8_t SpeedCounter_calc(struct SpeedCounter*);
uint8_t SpeedCounter_control(struct SpeedCounter*);
void SpeedCounter_initialize(struct SpeedCounter*, uint8_t);

#endif


