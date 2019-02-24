#include <Arduino.h>
#include <stdlib.h>
#include <stdio.h>
#include "robot.h"

const float Pi2 = 3.14159 * 2.0;
const float RobotRadius = 124.0 / 2.0;
const float WheelR = 66.4 / 2.0;
const uint8_t EncoderHoles = 20;

void Robot_initialize(struct Robot* robot)
{
  memset(robot, 0, sizeof(struct Robot));
  Engine_initialize(&(robot->leftEngine), ROBOT_LE_ENABLED, ROBOT_LE_FORWARD, ROBOT_LE_BACKWARD);
  Engine_initialize(&(robot->rightEngine), ROBOT_RE_ENABLED, ROBOT_RE_FORWARD, ROBOT_RE_BACKWARD);
  robot->currentCommandIndex = -1;
  robot->lastCommandIndex = -1;
}

void Robot_control(struct Robot* robot, enum RobotControl control)
{
  struct RobotCommand* command = &(robot->command);
  
  if (control == LEFT_ENGINE_PULSE) 
  {
    Engine_encoderPulse(&(robot->leftEngine));
    if (command->leftDistance > 0) command->leftDistance--;
    else command->leftDistance++;
  }
  else if (control == RIGHT_ENGINE_PULSE) 
  {
    Engine_encoderPulse(&(robot->rightEngine));
    if (command->rightDistance > 0) command->rightDistance--;
    else command->rightDistance++;
  }
  else 
  {
    Engine_control(&(robot->leftEngine));
    Engine_control(&(robot->rightEngine));
  }

  //float k = (float)(command->rightDistance)
  
  /*
  if (robot->currentCommandIndex >= 0)
  {
  }
  */
}

bool Robot_getState(struct Robot* robot, char *str)
{
  if (robot->rightEngine.signaled || robot->leftEngine.signaled)
  {
    char rightDistance[12];
    dtostrf(Engine_calcDistance(&(robot->rightEngine)), 1, 4, rightDistance);
    
    char leftDistance[12];
    dtostrf(Engine_calcDistance(&(robot->leftEngine)), 1, 4, leftDistance);
    
    sprintf(str, "ROBOT_MOVE,%u,%s,%u,%s;", robot->leftEngine.state, leftDistance, robot->rightEngine.state, rightDistance);
    return true;
  }

  return false;
}

/*
 * robot - object reference.
 * distance - distance in milimeters.
 * angle - angle in degrees.
 * setSpeed - speed in pt/sec.
 */
void Robot_addCommand(struct Robot* robot, int16_t distance, int16_t angle, uint8_t setSpeed)
{
  // left and right distance in milimeters.
  float dl;
  float dr;
  
  if (angle != 0)
  {
    float r = (360.0 * distance) / (Pi2 * angle);
    dl = (angle * Pi2 * (r - RobotRadius)) / 360.0;
    dr = (angle * Pi2 * (r + RobotRadius)) / 360.0;
  }
  else 
  {
    dl = distance;
    dr = distance;
  }
  
  // one hole distance in milimeters.
  float oneHoleDistance = (WheelR * Pi2) / (float)EncoderHoles; 
  
  robot->command.state = NEW;
  robot->command.leftDistance = round(dl / oneHoleDistance);
  robot->command.rightDistance = round(dr / oneHoleDistance);
  robot->command.leftDistanceInMilimeters = dl;
  robot->command.rightDistanceInMilimeters = dr;
  robot->command.speed = 30;
}
