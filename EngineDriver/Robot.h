#include <inttypes.h>
#include "Engine.h"

#ifndef ROBOT_H_
#define ROBOT_H_

const byte ROBOT_RE_ENABLED = 5;
const byte ROBOT_RE_FORWARD = 4;
const byte ROBOT_RE_BACKWARD = 7;

const byte ROBOT_LE_ENABLED = 6;
const byte ROBOT_LE_FORWARD = 8;
const byte ROBOT_LE_BACKWARD = 9;

enum CommandState { NEW, INPROGRESS, FINISHED };
enum RobotControl { CONTROL, LEFT_ENGINE_PULSE, RIGHT_ENGINE_PULSE };

struct RobotCommand
{
  volatile enum CommandState state;
  volatile float leftDistanceInMilimeters;
  volatile float rightDistanceInMilimeters;
  volatile int16_t leftDistance;
  volatile int16_t rightDistance;
  volatile uint8_t speed;
};

struct Robot 
{
  struct Engine leftEngine;
  struct Engine rightEngine; 
  int8_t currentCommandIndex;
  int8_t lastCommandIndex;
  
  struct RobotCommand command;
  //struct RobotCommand commands[32];

  float leftDistance;
  float rightDistance;
  /*
  volatile uint16_t curr_speed;
  volatile uint16_t avg_speed;
  volatile uint16_t max_speed;
  uint8_t filter_Level;
  volatile uint32_t avg_speed_internal;
  volatile uint32_t prev_micros;
  */
};

void Robot_initialize(struct Robot*);
void Robot_control(struct Robot*, enum RobotControl);
bool Robot_getState(struct Robot*, char *str);
void Robot_addCommand(struct Robot*, int16_t distance, int16_t angle, uint8_t setSpeed);

#endif
