#include <string.h>
#include <stdlib.h>
#include "PIDController.h"

void PIDController_initialize(struct PIDController* pidController, float Kp, float Ki, float Kd, float dt, float outMin, float outMax)
{
  memset(pidController, 0, sizeof(struct PIDController));
  pidController->Kp = Kp;
  pidController->Ki = Ki;
  pidController->Kd = Kd;
  pidController->dt = (dt / 1000.0);
  pidController->outMin = outMin;
  pidController->outMax = outMax;
}

void PIDController_reset(struct PIDController* pidController)
{
  //pidController->integral = 0.0;
  //pidController->pre_error = 0.0;
}

void PIDController_setPID(struct PIDController* pidController, uint16_t kp, uint16_t ki, uint16_t kd)
{
  pidController->Kp = (float)kp / 100.0;
  pidController->Ki = (float)ki / 100.0;
  pidController->Kd = (float)kd / 100.0;
}

float PIDController_compute(struct PIDController* pidController, float setpoint, float pv)
{
  // calculate error.
  float error = setpoint - pv;
  
  // Proportional term
  float Pout = pidController->Kp * error;
  
  // Integral term
  pidController->integral += error * pidController->dt;
  if (pidController->integral > pidController->outMax) pidController->integral = pidController->outMax;
  else if (pidController->integral < pidController->outMin) pidController->integral = pidController->outMin;
  float Iout = pidController->Ki * pidController->integral;
  
  // Derivative term
  float derivative = (error - pidController->pre_error) / pidController->dt;
  float Dout = pidController->Kd * derivative;
  
  // Calculate total output
  float output = Pout + Iout + Dout;
  
  // Restrict to max/min
  if(output > pidController->outMax) output = pidController->outMax;
  else if(output < pidController->outMin) output = pidController->outMin;
  
  // Save error to previous error
  pidController->pre_error = error;
  
  return output;
}
