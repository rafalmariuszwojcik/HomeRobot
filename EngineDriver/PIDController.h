#include <inttypes.h>

#ifndef PIDCONTROLLER_H_
#define PIDCONTROLLER_H_

struct PIDController {
  float Kp;
  float Ki;
  float Kd;
  float dt;
  float outMin;
  float outMax;
  float integral;
  float pre_error;
};

void PIDController_initialize(struct PIDController* pidController, float Kp, float Ki, float Kd, float dt, float outMin, float outMax);
void PIDController_reset(struct PIDController* pidController);
float PIDController_compute(struct PIDController* pidController, float setpoint, float pv);
void PIDController_setPID(struct PIDController* pidController, uint16_t kp, uint16_t ki, uint16_t kd);

#endif
