int analogin = 3;
int val = 0;

void setup() {
  Serial.begin(9600);
}

void loop() {
  val = analogRead(analogin);
  Serial.println(val);
  delay(1000);
}
