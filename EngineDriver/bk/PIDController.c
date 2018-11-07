#include <Arduino.h>
#include <stdlib.h>
#include "PIDController.h"

void _PIDController_setTunings(struct PIDController*, double, double, double, uint8_t);

void PIDController_initialize(struct PIDController* pidController, double Kp, double Ki, double Kd)
{
  memset(pidController, 0, sizeof(struct PIDController));
  pidController->outMin = 0;
  pidController->outMax = 255;
  pidController->SampleTime = 5;//100; // 0.1 sec.
  _PIDController_setTunings(pidController, Kp, Ki, Kd, P_ON_E);
}

double PIDController_compute(struct PIDController* pidController, double input, double setPoint)
{
  /* Compute all the working error variables */
  double error = setPoint - input;
  double dInput = (input - pidController->lastInput);
  pidController->outputSum += (pidController->ki * error);
  
  /* Add Proportional on Measurement, if P_ON_M is specified */
  if(!pidController->pOnE) pidController->outputSum -= pidController->kp * dInput;
  if(pidController->outputSum > pidController->outMax) pidController->outputSum = pidController->outMax;
  else if(pidController->outputSum < pidController->outMin) pidController->outputSum = pidController->outMin;

  /* Add Proportional on Error, if P_ON_E is specified */
  double output;
  if(pidController->pOnE) output = pidController->kp * error;
  else output = 0;

  /* Compute Rest of PID Output */
  output += pidController->outputSum - pidController->kd * dInput;
  if(output > pidController->outMax) output = pidController->outMax;
  else if(output < pidController->outMin) output = pidController->outMin;
      /*myOutput = output;*/

  /*Remember some variables for next time*/
  pidController->lastInput = input;
  /*lastTime = now;*/

  return output;
}

void _PIDController_setTunings(struct PIDController* pidController, double Kp, double Ki, double Kd, uint8_t POn)
{
  pidController->pOn = POn;
  pidController->pOnE = POn == P_ON_E;
  
  //dispKp = Kp; dispKi = Ki; dispKd = Kd;

   double SampleTimeInSec = ((double)(pidController->SampleTime)) / 1000;
   pidController->kp = Kp;
   pidController->ki = Ki * SampleTimeInSec;
   pidController->kd = Kd / SampleTimeInSec;
}
