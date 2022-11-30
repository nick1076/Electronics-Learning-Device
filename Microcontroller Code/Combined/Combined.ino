
//Voltage Vars
int voltage_offset =20;// set the correction offset value

//Data Vars
double cCurrent;
double cVoltage;
double cResistance;

void setup() {
  Serial.begin(9600);
}

void loop() {
  delay(1000);
  
  Current();
  
  Serial.println("");
}

void Current(){
  float overall = 0;

  for (int i = 0; i < 100; i++){
    float adc = analogRead(A0);
    float voltage = (adc*5/1023.0) - 2.5;
    float current = (voltage)/0.185;
    current = abs(current);
    overall += current;
  }

  overall = overall / 100;
  overall -= 0.02;
  overall = overall * 1000;
  overall = abs(overall);

  if (overall < 5){
    overall = 0;
  }
  cCurrent = overall;
  
  Serial.print("Current: ");
  Serial.print(cCurrent);
  Serial.println(" mA");
}

void Voltage(){
  int volt = analogRead(A0);// read the input
  double voltage = map(volt,0,1023, 0, 2500) + voltage_offset;// map 0-1023 to 0-2500 and add correction offset
  voltage /=100;// divide by 100 to get the decimal values
  Serial.print("Voltage: ");
  voltage -= .2; //Offset
  Serial.print(voltage);//print the voltge
  Serial.println("V");
  
  cVoltage = voltage;
}

void Resistance(){

  if (cVoltage == 0 && cCurrent == 0){
    cResistance = 0;
    Serial.print("Resistance: ");
    Serial.print(cResistance);
    Serial.println(" Ohms");
    return;
  }
  cResistance = (cVoltage/cCurrent) / 10;
  
  Serial.print("Resistance: ");
  Serial.print(cResistance);
  Serial.println(" Ohms");
}
