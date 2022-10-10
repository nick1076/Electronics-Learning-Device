int analogPin = 1;
int raw = 0;
int Vin = 5;
float Vout = 0;
float R1 = 10;
float R2 = 0;
float buffer = 0;

void setup(){
Serial.begin(9600);
}

void loop(){
    float overall = 0;
    float overallV = 0;
    int iterator = 0;
    for (int i = 0; i < 500; i++){
      raw = analogRead(analogPin);
      if(raw){
        buffer = raw * Vin;
        Vout = (buffer)/1024.0;
        buffer = (Vin/Vout) - 1;
        R2= R1 * buffer;
        overall += R2;
        overallV += Vout;
        iterator += 1;
        delay(10);
    }
    Serial.println("");
    Serial.print("R2: ");
    Serial.println(overall / iterator);
    Serial.print("Vout: ");
    Serial.println(overallV / iterator);
    Serial.print("Cout: ");
    Serial.println((overallV / iterator) / (overall/iterator));
    delay(100);
  }
}
