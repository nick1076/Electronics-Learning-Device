int offset =20;// set the correction offset value
void setup() {
  Serial.begin(9600);
  pinMode(4, INPUT);
  pinMode(0, OUTPUT);
  digitalWrite(0, HIGH);
}

void loop() {
  int volt = analogRead(4);// read the input
  double voltage = map(volt,0,1023, 0, 2500) + offset;// map 0-1023 to 0-2500 and add correction offset
  
  voltage /=100;// divide by 100 to get the decimal values
  voltage -= .2;
  Serial.println(voltage);
  delay(100);
  
  
}
