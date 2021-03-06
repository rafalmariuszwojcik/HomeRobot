#include <stdlib.h>
#include <string.h>
//#include <TimerOne.h>
#include <util/atomic.h>

extern "C"
{
  #include "Engine.h"
  #include "Robot.h"
  #include "Encoder.h"
  #include "Input.h"
}

const byte CMD_UNKNOWN = 0xFF;
const byte CMD_MAXPARAMSCOUNT = 10;

struct Command 
{
  char *command;
  void (*command_ptr)(int[], byte, void*);
  void *object;
};

Robot robot;
Encoder leftEncoder;
Encoder rightEncoder;
volatile bool timerSignal;

void cmd_EngineForward(int[], byte, void*);
void cmd_EngineBackward(int[], byte, void*);
void cmd_EngineStop(int[], byte, void*);
void cmd_EngineSpeed(int[], byte, void*);
void cmd_EnginePWM(int[], byte, void*);
void cmd_EngineSetPID(int[], byte, void*);

void cmd_robotForward(int[], byte, void*);
void cmd_robotBackward(int[], byte, void*);
void cmd_robotStop(int[], byte, void*);
void cmd_robotSpeed(int[], byte, void*);
void cmd_robotAddLeg(int[], byte, void*);
void cmd_echo(int[], byte, void*);
byte getCommandIndex(char command[]);

Command commands[] = 
{
  {(char*)"RF", cmd_EngineForward, &(robot.rightEngine)},
  {(char*)"RB", cmd_EngineBackward, &(robot.rightEngine)},
  {(char*)"RS", cmd_EngineStop, &(robot.rightEngine)},
  {(char*)"RSPD", cmd_EngineSpeed, &(robot.rightEngine)},
  {(char*)"RPWM", cmd_EnginePWM, &(robot.rightEngine)},
  {(char*)"RSETPID", cmd_EngineSetPID, &(robot.rightEngine)},
  {(char*)"LF", cmd_EngineForward, &(robot.leftEngine)},
  {(char*)"LB", cmd_EngineBackward, &(robot.leftEngine)},
  {(char*)"LS", cmd_EngineStop, &(robot.leftEngine)},
  {(char*)"LSPD", cmd_EngineSpeed, &(robot.leftEngine)},
  {(char*)"LPWM", cmd_EnginePWM, &(robot.leftEngine)},
  {(char*)"LSETPID", cmd_EngineSetPID, &(robot.leftEngine)},
  {(char*)"FWD", cmd_robotForward, NULL},
  {(char*)"BWD", cmd_robotBackward, NULL},
  {(char*)"STOP", cmd_robotStop, NULL},
  {(char*)"SPD", cmd_robotSpeed, NULL},
  {(char*)"ADDLEG", cmd_robotAddLeg, &robot},
  {(char*)"ECHO", cmd_echo, NULL}
};

uint32_t startTime;

uint8_t available_ptr()
{
  return Serial.available();
}

char read_ptr()
{
  int serial = Serial.read();
  if (serial != -1) 
  {
    char data = (char)serial;
    return data;
  }

  return 0;
}

void setup() {
  Serial.begin(115200);
  //Serial.begin(76800);
  //Serial.begin(74880);
  while (!Serial) {}
  
  Robot_initialize(&robot);
  
  // Attach an interrupt to the ISR vector
  attachInterrupt(0, pin_ISR, RISING);
  attachInterrupt(1, pin_ISR1, RISING);

  // attach the service routine here 100 Hz.
  //Timer1.initialize(10000);
  //Timer1.attachInterrupt( timerIsr ); 

  Input_Initialize(available_ptr, read_ptr);
}

void loop() 
{
  static char token[] = {',', ';', '\0'};
  int parameters[CMD_MAXPARAMSCOUNT];
  char* value;
  byte index = 0;
  byte cmd = CMD_UNKNOWN;
  
  char* input = Input_GetData();
  if (input != NULL)
  {
    Serial.println(input);
    value = strtok(input, token);
    while(value != NULL && index <= CMD_MAXPARAMSCOUNT)
    {
      if (index == 0)
      {
        cmd = getCommandIndex(value);
      }
      else
      {
        parameters[index - 1] = atoi(value);
      }

      value = strtok(NULL, token);
      index++;
    }

    if (cmd != CMD_UNKNOWN)
    {
      commands[cmd].command_ptr(parameters, index - 1, commands[cmd].object);
      cmd = CMD_UNKNOWN;
    }
  }
  
  struct Encoder le = Encoder_getStateAndReset(&leftEncoder);
  struct Encoder re = Encoder_getStateAndReset(&rightEncoder);
  bool ts;
  /*
  ATOMIC_BLOCK(ATOMIC_RESTORESTATE) {
    ts = timerSignal;
    if (ts) {
      timerSignal = false;
    };
  }
  */

  if (le.signaled) {
    Robot_control(&robot, LEFT_ENGINE_PULSE);
    Serial.println("speedInfo");
  };

  if (re.signaled) {
    //Robot_control(&robot, RIGHT_ENGINE_PULSE);
  };

  if (ts) {
    //Robot_control(&robot, CONTROL);
  };

  
 
  char speedInfo[64];

  /*
  if (Robot_getState(&robot, speedInfo))
  {
    Serial.println(speedInfo);
  }

  if (robot.rightEngine.signaled || robot.leftEngine.signaled)
  {
    sprintf(
      speedInfo, 
      "DIST,%u,%u,%u,%u;", 
      robot.leftEngine.state, 
      robot.leftEngine.fullDistance, 
      robot.rightEngine.state, 
      robot.rightEngine.fullDistance);
    Serial.print(speedInfo);
  }

  if (robot.leftEngine.signaled)
  {
    sprintf(
      speedInfo, 
      "ENC,0,%u,0,%u;", 
      robot.leftEngine.fullDistance, 
      robot.leftEngine.state);
    Serial.print(speedInfo);
  }

  if (robot.rightEngine.signaled)
  {
    sprintf(
      speedInfo, 
      "ENC,0,%u,0,%u;", 
      robot.rightEngine.fullDistance, 
      robot.rightEngine.state);
    Serial.print(speedInfo);
  }
  */  
  /*
  if (robot.rightEngine.signaled)
  {
    sprintf(
      speedInfo, 
      "RD,%u;RSPD,%u;RAVGSPD,%u;RPWM,%u;EOL;", 
      robot.rightEngine.fullDistance, 
      robot.rightEngine.speedCounter._curr_speed, 
      robot.rightEngine.speedCounter.avg_speed, 
      robot.rightEngine.pwm);
    robot.rightEngine.signaled = 0;
    Serial.println(speedInfo);

    //sprintf(speedInfo, "E00,1,%u,%u,%u,%u;", robot.rightEngine.speedCounter.curr_speed, robot.rightEngine.speedCounter.avg_speed, robot.rightEngine.pwm, robot.rightEngine.fullDistance);
    //Serial.println(speedInfo);
  }
  */

  
/*
  if (robot.leftEngine.signaled)
  {
    sprintf(
      speedInfo, 
      "LD,%u;LSPD,%u;LAVGSPD,%u;LPWM,%u;EOL;", 
      robot.leftEngine.fullDistance, 
      robot.leftEngine.speedCounter._curr_speed, 
      robot.leftEngine.speedCounter.avg_speed, 
      robot.leftEngine.pwm);
    robot.leftEngine.signaled = 0;
    Serial.println(speedInfo);

    //sprintf(speedInfo, "E00,0,%u,%u,%u,%u;", robot.leftEngine.speedCounter.curr_speed, robot.leftEngine.speedCounter.avg_speed, robot.leftEngine.pwm, robot.leftEngine.fullDistance);
    //Serial.println(speedInfo);
  }*/
}

bool isInputValid(int input) {
  return true;
}

void pin_ISR() 
{
  //Encoder_signal(&rightEncoder);
  //Robot_control(&robot, RIGHT_ENGINE_PULSE);
}

void pin_ISR1() 
{
  Encoder_signal(&leftEncoder);
  //Robot_control(&robot, LEFT_ENGINE_PULSE);
}

//volatile uint16_t sss = 0;

void timerIsr()
{
  //if (!lock)
  //Robot_control(&robot, CONTROL);
  timerSignal = true;
}

// command functions

void cmd_EngineForward(int[], byte, void* object) 
{
  startTime = millis();
  if (object != NULL) Engine_setState((struct Engine*)object, FORWARD);
}

void cmd_EngineBackward(int[], byte, void* object) 
{
  if (object != NULL) Engine_setState((struct Engine*)object, BACKWARD);
}

void cmd_EngineStop(int[], byte, void* object) 
{
  if (object != NULL) Engine_setState((struct Engine*)object, STOP);
}

void cmd_EngineSpeed(int params[], byte size, void* object) 
{
  if (size > 0 && object != NULL) Engine_speed((struct Engine*)object, params[0]);
}

void cmd_EnginePWM(int params[], byte size, void* object) 
{
  if (size > 0 && object != NULL) Engine_pwm((struct Engine*)object, params[0]);
}

void cmd_EngineSetPID(int params[], byte size, void* object)
{
  uint16_t kp = size > 0 ? params[0] : 0;
  uint16_t ki = size > 1 ? params[1] : 0;
  uint16_t kd = size > 2 ? params[2] : 0;
  if (object != NULL) Engine_setPID((struct Engine*)object, kp, ki, kd);
}

void cmd_robotForward(int params[], byte size, void*) 
{
  cmd_EngineForward(params, size, &(robot.leftEngine));
  cmd_EngineForward(params, size, &(robot.rightEngine));
}

void cmd_robotBackward(int params[], byte size, void*) 
{
  cmd_EngineBackward(params, size, &(robot.leftEngine));
  cmd_EngineBackward(params, size, &(robot.rightEngine));
}

void cmd_robotStop(int params[], byte size, void*) 
{
  cmd_EngineStop(params, size, &(robot.leftEngine));
  cmd_EngineStop(params, size, &(robot.rightEngine));
}

void cmd_robotSpeed(int params[], byte size, void*) 
{
  if (size > 1) 
  {
    Engine_setSpeed(&(robot.leftEngine), params[0]);
    Engine_setSpeed(&(robot.rightEngine), params[1]);
  }
}

void cmd_robotAddLeg(int params[], byte size, void* object)
{
  int16_t distance = size > 0 ? params[0] : 0;
  int16_t angle = size > 1 ? params[1] : 0;
  uint8_t setSpeed = size > 2 ? params[2] : 0;
  
  if (object != NULL) 
  {
    Robot_addCommand((struct Robot*)object, distance, angle, setSpeed);
    Serial.println(((struct Robot*)object)->leftDistance);
    Serial.println(((struct Robot*)object)->rightDistance);
  }
}

void cmd_echo(int params[], byte size, void*)
{
  Serial.println("ECHO");
  for (int i = 0; i < size; i++)
  {
    Serial.println(params[i]);
  }

  uint8_t a = 0;
  uint8_t b = 253;
  uint8_t c = a - b;
  Serial.println(c);
}

byte getCommandIndex(char *command){
  byte i = 0; 
  byte maxIndex = (sizeof(commands) / sizeof(Command)) - 1;
  for (i = 0; i <= maxIndex; i++)
  {
    if (strcmp(command, commands[i].command) == 0)
    {
      Serial.println(i);
      return i;
    }
  }

  Serial.println(maxIndex);
  return (maxIndex);
};
