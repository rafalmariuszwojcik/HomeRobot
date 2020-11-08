#include <TimerOne.h>
#include <util/atomic.h>

extern "C"
{
  #include <encoder.h>
}

const byte ENCODER_PIN = 3;

/*
Declare Arduino specific functions.
*/
void ardu_atomic(void (*func)(void* object), void* object);
uint32_t ardu_get_micros();
uint16_t ardu_digitalRead(uint16_t pin);
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
  Encoder_Initialize(&encoder, ardu_get_micros, ardu_digitalRead, ardu_atomic, 1); 

  /*
  Initialize encoder input pin.
  */
  pinMode(ENCODER_PIN, INPUT);
  attachInterrupt(digitalPinToInterrupt(ENCODER_PIN), encoder_pin_isr, RISING);

  
  
  Encoder_signal(&encoder);
  encoder.pin = 8;
  ardu_init_timer();
}

void loop() {
  // put your main code here, to run repeatedly:
  if (timer_pulse == 1)
  {
    Serial.println("speedInfo");
    Serial.println(encoder.pin);
    timer_pulse = 0;
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
  ATOMIC_BLOCK(ATOMIC_RESTORESTATE) {
    func(object);
  }
}

uint32_t ardu_get_micros()
{
  return micros();
}

uint16_t ardu_digitalRead(uint16_t pin)
{
  return 0;
}
