#include <TimerOne.h>
#include <util/atomic.h>

extern "C"
{
  #include <encoder.h>
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

/*
Timer pulse gate.
*/
volatile uint8_t timer_pulse = 0;

/*
Encoder instance.
*/
Encoder encoder;

void setup() {
  /*
  Initialize serial communication.
  */
  Serial.begin(9600);
  while (!Serial) {}

  /*
  Initialize encoder instance.
  */
  Encoder_Initialize(&encoder, ardu_get_micros, ardu_digitalRead, ardu_atomic, ENCODER_PIN); 

  /*
  Initialize encoder input pin.
  */
  pinMode(ENCODER_PIN, INPUT);
  attachInterrupt(digitalPinToInterrupt(ENCODER_PIN), encoder_pin_isr, /*RISING*/CHANGE);
  
  

  
  
  //Encoder_signal(&encoder);
  ardu_init_timer();
}

void loop() {
  // put your main code here, to run repeatedly:
  char message[64];
  uint8_t signaled = Encoder_isSignaled(&encoder);
  if (timer_pulse == 1 /*|| signaled*/)
  {
    timer_pulse = 0;
    
    sprintf(message, "frequency: %lu.%02lu | duty: %lu.%02lu", encoder.frequency / 100, encoder.frequency % 100, encoder.duty / 100, encoder.duty % 100);
    Serial.println(message);
    //sprintf(message, "sizeof(bool): %lu", sizeof(message));
    //Serial.println(message);
    

    //Serial.println(encoder.frequency);
    
  }
}

/*
  Initialize timer interrupt to every 1 second.
  Timer will keep reporting encoder data to client application.
*/
void ardu_init_timer()
{
  Timer1.initialize(1000000); // every 1 second.
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
