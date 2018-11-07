#include <inttypes.h>

#ifndef PIDCONTROLLER_H_
#define PIDCONTROLLER_H_

#define P_ON_M 0
#define P_ON_E 1

struct PIDController {
  double ki;
  double kp;
  double kd;
  double outputSum;
  double lastInput;
  double outMin;
  double outMax;
  uint32_t SampleTime;
  uint8_t pOn;
  uint8_t pOnE;
};

void PIDController_initialize(struct PIDController*, double, double, double);
double PIDController_compute(struct PIDController*, double, double);

#endif
