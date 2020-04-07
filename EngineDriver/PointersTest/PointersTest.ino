struct BBB 
{
  volatile uint16_t b1;
};

struct AAA 
{
  uint16_t a1;
  BBB b_test;
};

AAA a_test;

void test1(struct AAA*);
void test2(struct BBB*);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  while (!Serial) {}

  test1(&a_test);
  //test2(&(a_test.b_test));
  
  /*
  a_test.a1 = 10;
  a_test.b_test.b1 = 20;
  */
  

  char speedInfo[64];
  sprintf(
      speedInfo, 
      "LD,%u,%u;", 
      a_test.a1, a_test.b_test.b1);
  Serial.println(speedInfo);
}

void loop() {
  // put your main code here, to run repeatedly:

}

void test1(struct AAA* p) {
  p->a1 = 11;
  p->b_test.b1 = 21;
  test2(&(p->b_test));
}

void test2(struct BBB* p) {
  p->b1 = 997;
}
