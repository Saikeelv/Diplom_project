#include <HX711.h>
#include <math.h>

// --- Определение пинов ---
#define SENSOR_SPEED_PIN 2    // Пин для ИК-датчика скорости (энкодера)
#define CURRENT_SENSOR1  A0   // Пин для датчика тока двигателя
#define CURRENT_SENSOR2  A1   // Пин для датчика тока прижимного механизма
#define TEMP_SENSOR      A2   // Пин для датчика температуры (термистор)
#define HX711_DT         A3   // Пин данных для тензодатчика HX711
#define HX711_SCK        A4   // Пин синхронизации для HX711
#define MOTOR_PWM        3    // Пин ШИМ для двигателя
#define MOTOR_DIR        7    // Направление двигателя (H-мост DIR, фиксированное)
#define PRESS_PWM        9    // Пин ШИМ для прижимного механизма
#define PRESS_DIR        8    // Направление прижима (H-мост DIR, фиксированное)
#define RELAY_UP         5    // Пин реле для подъема прижимного механизма
#define RELAY_DOWN       6    // Пин реле для опускания прижимного механизма

// --- Опция инверсии логики реле ---
#ifdef RELAY_ACTIVE_LOW
  #define RELAY_ON  LOW
  #define RELAY_OFF HIGH
#else
  #define RELAY_ON  HIGH
  #define RELAY_OFF LOW
#endif

// --- Константы ---
const float VCC = 5.0;                 // Напряжение питания (5В)
const float CURRENT_FACTOR = 0.04;     // Коэффициент преобразования для датчиков тока
const float SPEED_THRESHOLD = 5000.0;  // Порог скорости (имп/мин)
const int MAX_TRIALS = 10;              // Максимальное количество испытаний
const float WEIGHT_THRESHOLD = 50.0;   // Порог веса для уменьшения мощности прижима (г)
const int LIFT_DURATION = 2000;        // Время подъема прижима (мс)
const float TEMP_MIN = 0.0;            // Минимальная температура (°C)
const float TEMP_MAX = 100.0;          // Максимальная температура
const float CURRENT_MAX = 20.0;        // Максимальный ток
const float WEIGHT_MIN = -2000.0;      // Минимальный вес
const float WEIGHT_MAX = 10000.0;      // Максимальный вес
const float SPEED_MAX = 20000.0;       // Максимальная скорость (имп/мин)
const unsigned long SPIN_UP_DELAY = 1000; // Задержка раскрутки (мс)
const unsigned long MOTOR_RAMP_UP_TIME = 1500; // Время разгона мотора (мс)
const unsigned long DATA_INTERVAL = 100;  // Интервал отправки данных (мс)
const int SPEED_AVG_COUNT = 5;         // Количество значений для сглаживания скорости
const unsigned long DEBOUNCE_INTERVAL = 1; // Минимальный интервал между импульсами (мс)
const float PULSES_PER_REV = 10.0;     // Импульсов за оборот энкодера

// --- Калибровка HX711 ---
HX711 scale;
float calibration_factor = -0.381;

// --- Коэффициенты для термистора ---
const float A = 1.009249522e-03;
const float B = 2.378405444e-04;
const float C = 2.019202697e-07;

// --- Переменные состояния ---
bool experimentRunning = false;
bool checking = false;
bool lifting = false;
bool rampingUp = false;                // Флаг разгона мотора
int trialCounter = 0;
int packetCounter = 0;
volatile unsigned long pulseCount = 0;
unsigned long lastSpeedTime = 0;
unsigned long lastDataTime = 0;
unsigned long liftStartTime = 0;
unsigned long experimentStartTime = 0;
unsigned long rampUpStartTime = 0;     // Время начала разгона
unsigned long lastSpeedUpdateTime = 0; // Время последнего обновления скорости
int currentError = 0;
unsigned long checkStartTime = 0;
bool checkPressed = false;
unsigned long pressStartTime = 0;
bool pressActive = false;
float speedBuffer[SPEED_AVG_COUNT];    // Буфер для сглаживания скорости (имп/мин)
int speedBufferIndex = 0;              // Индекс буфера
unsigned long lastPulseTime = 0;       // Время последнего импульса для фильтрации

// --- Функция расчёта температуры ---
float Thermistor(int Vo) {
  float logRt = log(10000.0 * ((1024.0 / Vo - 1)));
  float T = 1.0 / (A + B * logRt + C * logRt * logRt * logRt);
  float Tc = T - 273.15;
  return Tc - 2; // Коррекция на -2°C
}

// --- Обработчик прерывания для датчика скорости ---
void speedSensorISR() {
  unsigned long currentTime = micros();
  if (currentTime - lastPulseTime >= DEBOUNCE_INTERVAL * 1000) { // Фильтрация дребезга
    pulseCount++;
    lastPulseTime = currentTime;
  }
}

void setup() {
  Serial.begin(115200);
  
  // Инициализация пинов
  pinMode(SENSOR_SPEED_PIN, INPUT);
  pinMode(MOTOR_PWM, OUTPUT);
  pinMode(MOTOR_DIR, OUTPUT);
  pinMode(PRESS_PWM, OUTPUT);
  pinMode(PRESS_DIR, OUTPUT);
  pinMode(RELAY_UP, OUTPUT);
  pinMode(RELAY_DOWN, OUTPUT);
  digitalWrite(MOTOR_DIR, HIGH);
  digitalWrite(PRESS_DIR, HIGH);
  digitalWrite(RELAY_UP, RELAY_OFF);
  digitalWrite(RELAY_DOWN, RELAY_OFF);
  analogWrite(MOTOR_PWM, 0);
  analogWrite(PRESS_PWM, 0);

  // Настройка прерывания
  attachInterrupt(digitalPinToInterrupt(SENSOR_SPEED_PIN), speedSensorISR, RISING);

  // Инициализация HX711
  scale.begin(HX711_DT, HX711_SCK);
  scale.set_scale();
  scale.tare();
  scale.set_scale(calibration_factor);

  // Инициализация буфера скорости
  for (int i = 0; i < SPEED_AVG_COUNT; i++) {
    speedBuffer[i] = 0.0;
  }

  Serial.println("READY");
}

void loop() {
  handleSerial();

  if (lifting && millis() - liftStartTime >= LIFT_DURATION) {
    digitalWrite(RELAY_UP, RELAY_OFF);
    analogWrite(PRESS_PWM, 0);
    lifting = false;
    if (experimentRunning && trialCounter < MAX_TRIALS) {
      Serial.println("1111");
      startTrial();
    } else if (experimentRunning) {
      Serial.println("9999");
      Serial.flush();
      experimentRunning = false;
      stopAll(false);
    } else if (checking) {
      Serial.println("8888");
      Serial.flush();
      checking = false;
      stopAll(false);
    }
  }

  if (rampingUp && millis() - rampUpStartTime >= MOTOR_RAMP_UP_TIME) {
    // Завершение разгона, активация прижима
    rampingUp = false;
    digitalWrite(PRESS_DIR, HIGH);
    analogWrite(PRESS_PWM, 128);
    digitalWrite(RELAY_DOWN, RELAY_ON);
    digitalWrite(RELAY_UP, RELAY_OFF);
    pressStartTime = millis();
    pressActive = true;
  }

  // Обновление скорости во время разгона или испытания
  if (rampingUp || (experimentRunning && pressActive && !lifting)) {
    if (millis() - lastSpeedUpdateTime >= DATA_INTERVAL) {
      updateSpeedBuffer();
      lastSpeedUpdateTime = millis();
    }
  }

  if ((experimentRunning || checking) && pressActive && !lifting) {
    float units = scale.get_units(5); // Усреднение 5 измерений
    float weight = units * 0.035274 / 10;
    if (weight < 0 && weight > WEIGHT_MIN) weight = 0; // Отрицательный вес в 0
    adjustPressPower(weight);
    if (checking) {
      handleCheckRoutine(weight);
    }
    if (millis() - lastDataTime >= DATA_INTERVAL) {
      lastDataTime = millis();
      readAndSendData(weight);
    }
  }
}

void updateSpeedBuffer() {
  // Расчет скорости для заполнения буфера
  unsigned long currentTime = millis();
  float elapsedTime = (currentTime - lastSpeedTime) / 1000.0;
  float speed = (elapsedTime > 0) ? pulseCount * 60.0 / elapsedTime : 0; // имп/мин
  pulseCount = 0;
  lastSpeedTime = currentTime;

  // Сглаживание скорости
  speedBuffer[speedBufferIndex] = speed;
  speedBufferIndex = (speedBufferIndex + 1) % SPEED_AVG_COUNT;
}

void handleSerial() {
  if (Serial.available()) {
    char cmd = Serial.read();
    if (cmd == '1') {
      startExperiment();
    } else if (cmd == '0') {
      if (experimentRunning) {
        float units = scale.get_units(5);
        float weight = units * 0.035274 / 10;
        if (weight < 0 && weight > WEIGHT_MIN) weight = 0;
        if (pressActive) {
          readAndSendData(weight);
        }
        Serial.println("7777"); // Остановка пользователем
        Serial.flush();
        experimentRunning = false;
        stopAll(true);
      } else if (checking) {
        float units = scale.get_units(5);
        float weight = units * 0.035274 / 10;
        if (weight < 0 && weight > WEIGHT_MIN) weight = 0;
        readAndSendData(weight);
        Serial.println("7777"); // Остановка пользователем
        Serial.flush();
        checking = false;
        stopAll(true);
      }
    } else if (cmd == '2') {
      Serial.println("2222");
      startCheck();
    }
  }
}

void startExperiment() {
  experimentRunning = true;
  trialCounter = 0;
  packetCounter = 0;
  pulseCount = 0;
  lastSpeedTime = millis();
  lastDataTime = millis();
  lastSpeedUpdateTime = millis();
  experimentStartTime = millis();
  currentError = 0;
  lifting = false;
  checking = false;
  rampingUp = false;
  for (int i = 0; i < SPEED_AVG_COUNT; i++) {
    speedBuffer[i] = 0.0; // Сброс буфера скорости
  }
  speedBufferIndex = 0;
  Serial.println("1111");
  startTrial();
}

void startTrial() {
  if (trialCounter >= MAX_TRIALS) {
    return;
  }
  trialCounter++;
  packetCounter = 0;
  pulseCount = 0;
  lastSpeedTime = millis();
  lastSpeedUpdateTime = millis();
  digitalWrite(MOTOR_DIR, HIGH);
  analogWrite(MOTOR_PWM, 255);
  // Начать разгон без прижима
  rampingUp = true;
  rampUpStartTime = millis();
  pressActive = false;
  digitalWrite(RELAY_DOWN, RELAY_OFF);
  digitalWrite(RELAY_UP, RELAY_OFF);
  analogWrite(PRESS_PWM, 0);
}

void stopAll(bool lift) {
  analogWrite(MOTOR_PWM, 0);
  analogWrite(PRESS_PWM, 0);
  digitalWrite(RELAY_DOWN, RELAY_OFF);
  digitalWrite(MOTOR_DIR, HIGH);
  digitalWrite(PRESS_DIR, HIGH);

  if (lift) {
    lifting = true;
    liftStartTime = millis();
    digitalWrite(RELAY_UP, RELAY_ON);
    digitalWrite(RELAY_DOWN, RELAY_OFF);
    analogWrite(PRESS_PWM, 255);
  } else {
    experimentRunning = false;
    pressActive = false;
    checking = false;
    trialCounter = 0;
    rampingUp = false;
  }
}

void readAndSendData(float weight) {
  packetCounter++;

  // Получение avgSpeed из буфера
  float avgSpeed = 0.0;
  for (int i = 0; i < SPEED_AVG_COUNT; i++) {
    avgSpeed += speedBuffer[i];
  }
  avgSpeed /= SPEED_AVG_COUNT;

  // Чтение датчиков
  float current1 = readCurrent(CURRENT_SENSOR1);
  float current2 = readCurrent(CURRENT_SENSOR2);
  float temp = Thermistor(analogRead(TEMP_SENSOR));

  // Проверка ошибок
  if (experimentRunning && checkErrors(temp, weight, avgSpeed, current1, current2)) {
    Serial.print(packetCounter); Serial.print(";");
    Serial.print(millis() - experimentStartTime); Serial.print(";");
    Serial.print(temp); Serial.print(";");
    Serial.print(weight); Serial.print(";");
    Serial.print(avgSpeed / PULSES_PER_REV); Serial.print(";"); // об/мин
    Serial.print(trialCounter); Serial.print(";");
    Serial.println(currentError);
    Serial.println("9999");
    Serial.flush();
    experimentRunning = false;
    stopAll(true);
    return;
  }

  // Проверка скорости для завершения испытания
  if (experimentRunning && avgSpeed <= SPEED_THRESHOLD && millis() - pressStartTime >= SPIN_UP_DELAY) {
    analogWrite(MOTOR_PWM, 0);
    stopAll(true);
    return;
  }

  // Коррекция отрицательного веса
  if (weight < 0 && weight > WEIGHT_MIN) weight = 0;

  // Отправка данных
  if (experimentRunning) {
    Serial.print(packetCounter); Serial.print(";");
    Serial.print(millis() - experimentStartTime); Serial.print(";");
    Serial.print(temp); Serial.print(";");
    Serial.print(weight); Serial.print(";");
    Serial.print(avgSpeed / PULSES_PER_REV); Serial.print(";"); // об/мин
    Serial.print(trialCounter); Serial.print(";");
    Serial.println(currentError);
  } else if (checking) {
    Serial.print(packetCounter); Serial.print(";");
    Serial.print(current1); Serial.print(";");
    Serial.print(current2); Serial.print(";");
    Serial.print(avgSpeed / PULSES_PER_REV); Serial.print(";"); // об/мин
    Serial.print(temp); Serial.print(";");
    Serial.print(weight); Serial.print(";");
    Serial.println(currentError);
  }
}

float readCurrent(int pin) {
  float voltage = analogRead(pin) * VCC / 1023.0;
  voltage -= (VCC / 2.0 - 0.007);
  return voltage / CURRENT_FACTOR;
}

bool checkErrors(float temp, float weight, float speed, float current1, float current2) {
  if (temp < TEMP_MIN || temp > TEMP_MAX) {
    currentError = 12;
    return true;
  }
  if (temp > 80.0) {
    currentError = 13;
    return true;
  }

  // if (!scale.is_ready()) {
  //   currentError = 21;
  //   Serial.println("Error 21: Scale not ready");
  //   return true;
  // }
  // if (weight < WEIGHT_MIN || weight > WEIGHT_MAX) {
  //   currentError = 22;
  //   Serial.print("Error 22: Weight="); Serial.println(weight);
  //   return true;
  // }
  //float rawWeight = weight * 10 / 0.035274;
  //rawWeight = rawWeight * calibration_factor;
  //if (rawWeight < WEIGHT_MIN || rawWeight > WEIGHT_MAX) {
  //  currentError = 22;
  //  return true;
  //}
  if (pressActive && abs(weight) < 10.0 && millis() - pressStartTime > 32000) {
    currentError = 23;
    return true;
  }

  // if (speed > SPEED_MAX) {
  //   currentError = 32;
  //   Serial.print("Error 32: Speed="); Serial.println(speed);
  //   return true;
  // }
  if (speed == 0 && pressActive && millis() - pressStartTime > 2000) {
    currentError = 33;
    return true;
  }

  if (current1 < -CURRENT_MAX || current1 > CURRENT_MAX) {
    currentError = 42;
    return true;
  }
  if (current1 > 30.0) {
    currentError = 43;
    return true;
  }

  if (current2 < -CURRENT_MAX || current2 > CURRENT_MAX) {
    currentError = 52;
    return true;
  }
  if (current2 > 30.0) {
    currentError = 53;
    return true;
  }

  return false;
}

void adjustPressPower(float weight) {
  if (abs(weight) > WEIGHT_THRESHOLD) {
    analogWrite(PRESS_PWM, 100);
  } else {
    analogWrite(PRESS_PWM, 128);
  }
}

void startCheck() {
  checking = true;
  checkStartTime = millis();
  checkPressed = false;
  currentError = 0;
  packetCounter = 0;
  pulseCount = 0;
  lastSpeedTime = millis();
  lastDataTime = millis();
  lastSpeedUpdateTime = millis();
  experimentRunning = false;
  for (int i = 0; i < SPEED_AVG_COUNT; i++) {
    speedBuffer[i] = 0.0; // Сброс буфера скорости
  }
  speedBufferIndex = 0;

  digitalWrite(MOTOR_DIR, HIGH);
  digitalWrite(PRESS_DIR, HIGH);
  analogWrite(MOTOR_PWM, 255);
  analogWrite(PRESS_PWM, 128);
  digitalWrite(RELAY_DOWN, RELAY_ON);
  digitalWrite(RELAY_UP, RELAY_OFF);
  pressActive = true;
}

void handleCheckRoutine(float weight) {
  if (!checkPressed && abs(weight) > 50 && millis() - checkStartTime > 2000) {
    checkPressed = true;
    analogWrite(MOTOR_PWM, 0);
    digitalWrite(RELAY_DOWN, RELAY_OFF);
    digitalWrite(RELAY_UP, RELAY_ON);
    digitalWrite(PRESS_DIR, HIGH);
    analogWrite(PRESS_PWM, 255);
    lifting = true;
    liftStartTime = millis();
  }
}