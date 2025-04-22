#include <HX711.h>

// === Пины ===
#define HX711_DT 3
#define HX711_SCK 2
#define ENCODER_PIN 4
#define TEMP_PIN A2
#define CURRENT_MOTOR_PIN A0
#define CURRENT_PRESS_PIN A1
#define DIR_MOTOR 7
#define PWM_MOTOR 9
#define DIR_PRESS 8
#define PWM_PRESS 10

// === Датчики и переменные ===
HX711 scale;
unsigned long lastMeasureTime = 0;
unsigned long startTime;
unsigned long packetTimer = 0;
float currentTime = 0.0;
float speed = 0.0;
float temperature = 0.0;
float force = 0.0;
float currentMotor = 0.0;
float currentPress = 0.0;
int packetNumber = 0;
int experimentNumber = 0;
int ticks = 0;
bool encoderState = false;
bool experimentRunning = false;
bool loadDetected = false;

// === Константы ===
const float WEIGHT_THRESHOLD = 100.0;
const float SPEED_THRESHOLD = 3.0;
const int NUM_EXPERIMENTS = 10;
const int MEASURE_INTERVAL = 100; // мс
const int MOTOR_PWM = 200;
const int PRESS_PWM_HIGH = 200;
const int PRESS_PWM_LOW = 80;
const float TEMP_OVERHEAT = 60.0; // °C
const float CURRENT_OVERLOAD = 10.0; // ампер
const float VCC = 5.0;
const float ACS_FACTOR = 0.04; // 20 мВ/А

void setup() {
  Serial.begin(9600);
  pinMode(ENCODER_PIN, INPUT);
  pinMode(DIR_MOTOR, OUTPUT);
  pinMode(PWM_MOTOR, OUTPUT);
  pinMode(DIR_PRESS, OUTPUT);
  pinMode(PWM_PRESS, OUTPUT);

  scale.begin(HX711_DT, HX711_SCK);
  scale.set_scale(110.0);
  scale.tare();

  Serial.println("WAITING FOR START");
}

void loop() {
  if (Serial.available()) {
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();
    if (cmd == "start") {
      experimentRunning = true;
      experimentNumber = 0;
      packetNumber = 0;
      startTime = millis();
    }
  }

  if (experimentRunning && experimentNumber < NUM_EXPERIMENTS) {
    runExperiment();
  }
}

void runExperiment() {
  startMotors();

  unsigned long experimentStart = millis();
  packetTimer = experimentStart;
  loadDetected = false;
  ticks = 0;
  bool testComplete = false;

  while (!testComplete) {
    readEncoder();

    if (millis() - packetTimer >= MEASURE_INTERVAL) {
      currentTime = (millis() - startTime) / 1000.0;
      force = scale.is_ready() ? scale.get_units() : -999;

      // Температура — аналоговая шкала 0–1023, масштаб на 100 °C
      temperature = analogRead(TEMP_PIN) * (VCC / 1023.0) * 100.0;

      // Ток — по формуле из твоего кода
      currentMotor = calculateCurrent(CURRENT_MOTOR_PIN);
      currentPress = calculateCurrent(CURRENT_PRESS_PIN);

      speed = ticks * (1000.0 / MEASURE_INTERVAL); // об/сек
      ticks = 0;
      packetTimer = millis();

      int errorCode = checkErrors();

      sendPacket(errorCode);

      if (force < WEIGHT_THRESHOLD) {
        setPressDirection(true);
        analogWrite(PWM_PRESS, PRESS_PWM_HIGH);
      } else {
        loadDetected = true;
        analogWrite(PWM_PRESS, PRESS_PWM_LOW);
      }

      if (speed < SPEED_THRESHOLD) {
        testComplete = true;
      }
    }
  }

  retractPress();
  stopMotors();
  experimentNumber++;
}

void readEncoder() {
  if (digitalRead(ENCODER_PIN) == LOW && !encoderState) {
    encoderState = true;
    ticks++;
  }
  if (digitalRead(ENCODER_PIN) == HIGH) {
    encoderState = false;
  }
}

void startMotors() {
  digitalWrite(DIR_MOTOR, HIGH);
  analogWrite(PWM_MOTOR, MOTOR_PWM);

  setPressDirection(true);
  analogWrite(PWM_PRESS, PRESS_PWM_HIGH);
}

void stopMotors() {
  analogWrite(PWM_MOTOR, 0);
  analogWrite(PWM_PRESS, 0);
}

void setPressDirection(bool down) {
  digitalWrite(DIR_PRESS, down ? HIGH : LOW);
}

void retractPress() {
  setPressDirection(false);
  analogWrite(PWM_PRESS, PRESS_PWM_HIGH);
  unsigned long tStart = millis();
  while (scale.get_units() >= WEIGHT_THRESHOLD && millis() - tStart < 5000) {
    delay(50);
  }
  delay(200);
  analogWrite(PWM_PRESS, 0);
}

float calculateCurrent(int pin) {
  float voltage = (VCC / 1023.0) * analogRead(pin);
  voltage = voltage - (VCC * 0.5) + 0.007;
  return voltage / ACS_FACTOR;
}

void sendPacket(int errorCode) {
  packetNumber++;
  Serial.print(packetNumber);
  Serial.print(";");
  Serial.print(currentTime, 2);
  Serial.print(";");
  Serial.print(temperature, 2);
  Serial.print(";");
  Serial.print(force, 2);
  Serial.print(";");
  Serial.print(speed, 2);
  Serial.print(";");
  Serial.print(experimentNumber + 1);
  Serial.print(";");
  Serial.println(errorCode);
}

int checkErrors() {
  if (!scale.is_ready()) return 21;
  if (force < -50 || force > 20000) return 22;

  if (temperature > TEMP_OVERHEAT) return 13;

  if (speed < 0 || speed > 1000) return 32;

  if (currentMotor > CURRENT_OVERLOAD) return 43;
  if (currentPress > CURRENT_OVERLOAD) return 53;

  return 0;
}
