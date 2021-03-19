#include <TimerOne.h>
#include <util/atomic.h>

extern "C"
{
  #include <encoder.h>
  #include <input.h>
  #include <command.h>
  #include <fp.h>
  #include <test_all.h>
  
}

const uint8_t ENCODER_PIN = 3;

/*
Declare Arduino specific functions.
*/
void ardu_atomic(void (*func)(void* object), void* object);
uint32_t ardu_get_micros();
uint32_t ardu_get_millis();
uint8_t ardu_digitalRead(uint8_t pin);
void ardu_init_timer(); 
void ardu_init_timer_isr();
uint8_t available_ptr();
char read_ptr();
void ardu_output(char* text);
bool test_func(void (*output_ptr)(char* text));

/*
Timer pulse gate.
*/
volatile uint8_t timer_pulse = 0;

/*
Encoder instance.
*/
Encoder encoder;

/*
Input buffer instance.
*/
InputBuffer input_buffer;

/*
Commands executor. 
*/
Commands commands;

void cmd_echo(int16_t[], uint8_t, void*);
void cmd_test(int16_t[], uint8_t, void*);
void cmd_engine(int16_t[], uint8_t, void*);

struct Command command_list[] = 
{
  { (char*)"ECHO", cmd_echo, NULL },
  { (char*)"TEST", cmd_test, NULL },
  { (char*)"ENG", cmd_engine, NULL },
  
};

/*
struct Test test_list[] = 
{
  {(char*)"T1", test_func},
  {(char*)"T2", test_func},
  {(char*)"T3", test_func},
  {(char*)"T4", test_func},
};
*/

void setup() {
  /*
  Initialize serial communication.
  */
  Serial.begin(115200);
  while (!Serial) {}

  test_all_run(ardu_output);

  /*
  Initialize encoder instance.
  */
  Encoder_Initialize(&encoder, ardu_get_micros, ardu_digitalRead, ardu_atomic, ENCODER_CHANGE, ENCODER_PIN); 
  //Encoder_Initialize(&encoder, ardu_get_micros, ardu_digitalRead, ardu_atomic, ENCODER_RISING, ENCODER_PIN); 
  //Encoder_Initialize(&encoder, ardu_get_micros, ardu_digitalRead, ardu_atomic, ENCODER_FALLING, ENCODER_PIN);

  /*
  Initialize input buffer.
  */
  Input_Initialize(&input_buffer, available_ptr, read_ptr);

  Commands_Initialize(&commands, command_list, sizeof(command_list) / sizeof(struct Command));


  /*
  Initialize encoder input pin.
  */
  pinMode(ENCODER_PIN, INPUT);
  attachInterrupt(digitalPinToInterrupt(ENCODER_PIN), encoder_pin_isr, CHANGE);
  //attachInterrupt(digitalPinToInterrupt(ENCODER_PIN), encoder_pin_isr, FALLING);
  //attachInterrupt(digitalPinToInterrupt(ENCODER_PIN), encoder_pin_isr, RISING);
  
  

  
  
  //Encoder_signal(&encoder);
  ardu_init_timer();
}

uint32_t t = 0;
uint32_t last_period = 0;
uint32_t last_duty = 0;


void loop() {
  // put your main code here, to run repeatedly:
  char message[128];
  uint32_t st = micros();
  uint8_t signaled = Encoder_isSignaled(&encoder);

  if (signaled)
  {
    t = micros() - st;
  }

  Commands_execute(&commands, Input_getData(&input_buffer));
  
  if (timer_pulse == 1 /*|| signaled*/)
  {
    timer_pulse = 0;

    
    //if (last_period != encoder.period || last_duty != encoder.duty)
    {
      sprintf(message, "frequency: %lu.%02lu | duty: %lu.%02lu | count: %lu | period: %lu | exec time: %lu", 
              fp_ToInt_24_8(encoder.frequency), fp_FracAsDecimal_24_8(encoder.frequency), 
              fp_ToInt_24_8(encoder.duty), fp_FracAsDecimal_24_8(encoder.duty), 
              encoder.count, 
              encoder.period, 
              t);
      Serial.println(message);
  
      //test_run(test_list, sizeof(test_list) / sizeof(struct Test), ardu_output);
    }
    

    last_period = encoder.period;
    last_duty = encoder.duty;
    
    //char myStr[] = "this is a test";
    //uint64_t qqq;
    
    //sprintf(message, "sizeof(uint64_t): %lu", (uint16_t)sizeof(uint64_t));
    //Serial.println(sizeof(bool));
    

    //Serial.println(sizeof(commands));
    
  }

  char* input = Input_getData(&input_buffer);
  if (input != NULL)
  {
    Serial.println(input);
  }

}

/*
  Initialize timer interrupt to every 1 second.
  Timer will keep reporting encoder data to client application.
*/
void ardu_init_timer()
{
  Timer1.initialize(100000); // every 0.5 second.
  Timer1.attachInterrupt(ardu_init_timer_isr); 
}

/*
  Timer interrupt. Set global timer_pulse variable to 1 (variable type is byte).
*/
void ardu_init_timer_isr()
{
  timer_pulse = 1;
}

/*
  Encoder pin interrupt procedure.
*/
void encoder_pin_isr() 
{
  Encoder_signal(&encoder);
}

void ardu_atomic(void (*func)(void* object), void* object)
{
  ATOMIC_BLOCK(ATOMIC_RESTORESTATE) 
  {
    func(object);
  }
}

uint32_t ardu_get_micros()
{
  return micros();
}

uint32_t ardu_get_millis()
{
  return millis();
}

uint8_t ardu_digitalRead(uint8_t pin)
{
  return digitalRead(pin) == HIGH ? 1 : 0;
}

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

void cmd_echo(int16_t[], uint8_t, void*)
{
  Serial.println("Echo...");
}

void ardu_output(char* text)
{
  Serial.println(text);
}

bool test_func(void (*output_ptr)(char* text))
{
  output_ptr((char*)"AAAAA");
  return false;
}

void cmd_test(int16_t[], uint8_t, void*)
{
  test_all_run(ardu_output);
}

void cmd_engine(int16_t params[], uint8_t count, void* object)
{
  ardu_output((char*)"ENGINE");
}
