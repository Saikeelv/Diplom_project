#include <HX711.h>

// --- Определение пинов ---
#define SENSOR_SPEED_PIN 2    // Пин для ИК-датчика скорости (энкодера)
#define CURRENT_SENSOR1  A0   // Пин для датчика тока двигателя
#define CURRENT_SENSOR2  A1   // Пин для датчика тока прижимного механизма
#define TEMP_SENSOR      A2   // Пин для датчика температуры
#define HX711_DT         A3   // Пин данных для тензодатчика HX711
#define HX711_SCK        A4   // Пин синхронизации для HX711
#define MOTOR_PWM        3    // Пин ШИМ для двигателя
#define PRESS_PWM        9    // Пин ШИМ для прижимного механизма
#define RELAY_UP         5    // Пин реле для подъема прижимного механизма
#define RELAY_DOWN       6    // Пин реле для опускания прижимного механизма

// --- Опция инверсии логики реле (раскомментировать, если реле активны на LOW) ---
// #define RELAY_ACTIVE_LOW

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
const int SPEED_THRESHOLD = 3000;      // Порог скорости (импульсы/с)
const int MAX_TRIALS = 10;             // Максимальное количество испытаний
const float WEIGHT_THRESHOLD = 50.0;   // Порог веса для уменьшения мощности прижима (в граммах)
const int LIFT_DURATION = 1000;        // Время подъема прижимного механизма (1 секунда)
const float TEMP_MIN = 0.0;            // Минимальная допустимая температура (°C)
const float TEMP_MAX = 100.0;          // Максимальная допустимая температура
const float CURRENT_MAX = 50.0;        // Максимальный допустимый ток
const float WEIGHT_MIN = -1000.0;      // Минимальный допустимый вес
const float WEIGHT_MAX = 1000.0;       // Максимальный допустимый вес
const float SPEED_MAX = 10000.0;       // Максимальная допустимая скорость

// --- Калибровка HX711 ---
HX711 scale;                           // Объект для работы с тензодатчиком
float calibration_factor = -0.381;     // Коэффициент калибровки тензодатчика

// --- Переменные состояния ---
bool experimentRunning = false;        // Флаг выполнения эксперимента
bool testMode = false;                 // Флаг тестового режима (для проверки)
bool checking = false;                 // Флаг выполнения проверочной процедуры
int trialCounter = 0;                  // Счетчик испытаний
int packetCounter = 0;                 // Счетчик пакетов данных
volatile unsigned long pulseCount = 0; // Счетчик импульсов от ИК-датчика (volatile для прерываний)
unsigned long lastSpeedTime = 0;       // Время последнего измерения скорости
unsigned long lastDataTime = 0;        // Время последней отправки данных
int currentError = 0;                  // Код текущей ошибки
unsigned long checkStartTime = 0;      // Время начала проверочной процедуры
bool checkPressed = false;             // Флаг нажатия в проверочной процедуре
unsigned long pressStartTime = 0;      // Время начала работы прижима
bool pressActive = false;              // Флаг активности прижимного механизма
bool lifting = false;                  // Флаг подъема прижимного механизма

// --- Обработчик прерывания для датчика скорости ---
void speedSensorISR() {
  pulseCount++;                        // Увеличение счетчика импульсов
}

void setup() {
  Serial.begin(115200);                // Инициализация последовательного порта
  
  // Инициализация пинов
  pinMode(SENSOR_SPEED_PIN, INPUT);    // ИК-датчик как вход
  pinMode(MOTOR_PWM, OUTPUT);          // ШИМ двигателя как выход
  pinMode(PRESS_PWM, OUTPUT);          // ШИМ прижима как выход
  pinMode(RELAY_UP, OUTPUT);           // Реле подъема как выход
  pinMode(RELAY_DOWN, OUTPUT);         // Реле опускания как выход
  digitalWrite(RELAY_UP, RELAY_OFF);   // Начальное состояние: реле выключены
  digitalWrite(RELAY_DOWN, RELAY_OFF);

  // Настройка прерывания для датчика скорости
  attachInterrupt(digitalPinToInterrupt(SENSOR_SPEED_PIN), speedSensorISR, RISING); // Прерывание на переход LOW->HIGH

  // Инициализация HX711
  scale.begin(HX711_DT, HX711_SCK);    // Запуск модуля HX711
  scale.set_scale();                   // Установка масштаба
  scale.tare();                        // Сбрасываем значения веса в 0
  scale.set_scale(calibration_factor); // Установка коэффициента калибровки

  Serial.println("READY");             // Сообщение о готовности
}

void loop() {
  handleSerial();                      // Обработка команд с последовательного порта

  if ((experimentRunning || checking) && !lifting) { // Если идет эксперимент или проверка
    // Чтение веса один раз за цикл для оптимизации
    float units = scale.get_units(5);  // Усреднение 5 измерений для ускорения
    float weight = units * 0.035274 / 10; // Перевод в граммы
    adjustPressPower(weight);          // Регулировка мощности прижима
    if (checking) {
      handleCheckRoutine(weight);      // Обработка проверочной процедуры
    }
    if (millis() - lastDataTime >= 100) { // Отправка данных каждые 100 мс
      lastDataTime = millis();
      readAndSendData(weight);
    }
  }
}

void handleSerial() {
  // Обработка входящих команд
  if (Serial.available()) {
    char cmd = Serial.read();
    if (cmd == '1') {                  // Команда запуска эксперимента
      startExperiment();
    } else if (cmd == '0') {           // Команда остановки
      if (experimentRunning) {
        float units = scale.get_units(5); // Чтение веса для последнего пакета
        float weight = units * 0.035274 / 10;
        readAndSendData(weight);       // Отправка последнего пакета
        Serial.println("9999");        // Код завершения
        stopAll(true);
      } else if (checking) {
        float units = scale.get_units(5); // Чтение веса для последнего пакета
        float weight = units * 0.035274 / 10;
        readAndSendData(weight);       // Отправка последнего пакета
        Serial.println("8888");        // Код завершения проверки
        stopAll(true);
      }
    } else if (cmd == '2') {           // Команда запуска проверки
      Serial.println("2222");          // Подтверждение запуска проверки
      startCheck();
    }
  }
}

void startExperiment() {
  // Запуск эксперимента
  experimentRunning = true;
  trialCounter = 0;                    // Сброс счетчика испытаний
  packetCounter = 0;                   // Сброс счетчика пакетов
  pulseCount = 0;                      // Сброс счетчика импульсов
  lastSpeedTime = millis();            // Запись времени
  lastDataTime = millis();             // Сброс времени отправки данных
  currentError = 0;                    // Сброс ошибок
  lifting = false;                     // Сброс флага подъема
  Serial.println("1111");              // Подтверждение запуска
  startTrial();                        // Запуск первого испытания
}

void startTrial() {
  // Запуск одного испытания
  if (trialCounter >= MAX_TRIALS) {    // Если достигнуто максимум испытаний
    currentError = 1;                  // Успешное завершение
    float units = scale.get_units(5);  // Чтение веса для последнего пакета
    float weight = units * 0.035274 / 10;
    readAndSendData(weight);           // Отправка последнего пакета с кодом ошибки
    Serial.println("9999");            // Код завершения
    stopAll(true);
    return;
  }
  trialCounter++;                      // Увеличение счетчика испытаний
  analogWrite(MOTOR_PWM, 255);         // Полная мощность двигателя
  analogWrite(PRESS_PWM, 128);         // Половина мощности прижима
  digitalWrite(RELAY_DOWN, RELAY_ON);  // Опускание прижимного механизма
  pressStartTime = millis();           // Запись времени начала
  pressActive = true;                  // Активация прижима
}

void stopAll(bool lift) {
  // Остановка всех механизмов
  analogWrite(MOTOR_PWM, 0);           // Отключение двигателя
  analogWrite(PRESS_PWM, 0);           // Отключение прижима
  digitalWrite(RELAY_DOWN, RELAY_OFF); // Отключение реле опускания

  if (lift) {                          // Если требуется подъем
    digitalWrite(RELAY_UP, RELAY_ON);  // Включение реле подъема
    delay(LIFT_DURATION);              // Ожидание 1 секунды
    digitalWrite(RELAY_UP, RELAY_OFF); // Отключение реле подъема
  }

  // Сброс флагов состояния
  experimentRunning = false;
  testMode = false;
  checking = false;
  lifting = false;
  trialCounter = 0;
}

void readAndSendData(float weight) {
  // Чтение и отправка данных
  packetCounter++;                     // Увеличение счетчика пакетов

  // Расчет скорости (импульсы/с)
  unsigned long currentTime = millis();
  float elapsedTime = (currentTime - lastSpeedTime) / 1000.0; // Время в секундах
  float speed = (elapsedTime > 0) ? pulseCount / elapsedTime : 0; // Импульсы/с
  pulseCount = 0;                      // Сброс счетчика импульсов
  lastSpeedTime = currentTime;         // Обновление времени

  // Чтение датчиков
  float current1 = readCurrent(CURRENT_SENSOR1); // Ток двигателя
  float current2 = readCurrent(CURRENT_SENSOR2); // Ток прижима
  float temp = analogRead(TEMP_SENSOR) * VCC / 1023.0 * 100.0; // Температура

  // Проверка ошибок (только для эксперимента, не для проверки)
  if (experimentRunning && checkErrors(temp, weight, speed, current1, current2)) {
    // Отправка пакета с кодом ошибки
    Serial.print(packetCounter); Serial.print(";");
    Serial.print(current1); Serial.print(";");
    Serial.print(current2); Serial.print(";");
    Serial.print(speed); Serial.print(";");
    Serial.print(temp); Serial.print(";");
    Serial.print(weight); Serial.print(";");
    Serial.println(currentError);
    Serial.println("9999");            // Код завершения при ошибке
    stopAll(true);                     // Остановка эксперимента
    return;
  }

  // Отправка данных
  Serial.print(packetCounter); Serial.print(";");
  Serial.print(current1); Serial.print(";");
  Serial.print(current2); Serial.print(";");
  Serial.print(speed); Serial.print(";");
  Serial.print(temp); Serial.print(";");
  Serial.print(weight); Serial.print(";");
  Serial.println(currentError);
}

float readCurrent(int pin) {
  // Чтение тока с датчика ACS758
  float voltage = analogRead(pin) * VCC / 1023.0; // Преобразование в напряжение
  voltage -= (VCC / 2.0 - 0.007);                 // Учет смещения
  return voltage / CURRENT_FACTOR;                // Преобразование в ток
}

bool checkErrors(float temp, float weight, float speed, float current1, float current2) {
  // Проверка ошибок датчиков
  // Температура
  if (temp < TEMP_MIN || temp > TEMP_MAX) {
    currentError = 12; // Некорректные показания температуры
    return true;
  }
  if (temp > 80.0) {
    currentError = 13; // Перегрев масла
    return true;
  }

  // Вес
  if (!scale.is_ready()) {
    currentError = 21; // Датчик веса не отвечает
    return true;
  }
  if (weight < WEIGHT_MIN || weight > WEIGHT_MAX) {
    currentError = 22; // Некорректные показания веса
    return true;
  }
  if (pressActive && abs(weight) < 10.0 && millis() - pressStartTime > 1000) {
    currentError = 23; // Поломка прижимного механизма
    return true;
  }

  // Скорость
  if (speed > SPEED_MAX) {
    currentError = 32; // Некорректные показания скорости
    return true;
  }
  if (speed == 0 && pressActive && millis() - pressStartTime > 2000) {
    currentError = 33; // Зазор в датчике скорости
    return true;
  }

  // Ток двигателя
  if (current1 < -CURRENT_MAX || current1 > CURRENT_MAX) {
    currentError = 42; // Некорректные показания тока двигателя
    return true;
  }
  if (current1 > 30.0) {
    currentError = 43; // Перегрузка тока двигателя
    return true;
  }

  // Ток прижимного механизма
  if (current2 < -CURRENT_MAX || current2 > CURRENT_MAX) {
    currentError = 52; // Некорректные показания тока прижима
    return true;
  }
  if (current2 > 30.0) {
    currentError = 53; // Перегрузка тока прижима
    return true;
  }

  return false; // Ошибок нет
}

void adjustPressPower(float weight) {
  // Регулировка мощности прижимного механизма
  if (abs(weight) > WEIGHT_THRESHOLD) {
    analogWrite(PRESS_PWM, 64);        // Уменьшение до 25% мощности
  } else {
    analogWrite(PRESS_PWM, 128);       // Половина мощности
  }
}

void startCheck() {
  // Запуск проверочной процедуры
  checking = true;                     // Установка флага проверки
  testMode = true;                     // Включение тестового режима
  checkStartTime = millis();           // Запись времени
  checkPressed = false;                // Сброс флага нажатия
  currentError = 0;                    // Сброс ошибки
  packetCounter = 0;                   // Сброс счетчика пакетов
  pulseCount = 0;                      // Сброс счетчика импульсов
  lastSpeedTime = millis();            // Запись времени
  lastDataTime = millis();             // Сброс времени отправки данных

  analogWrite(MOTOR_PWM, 255);         // Полная мощность двигателя
  analogWrite(PRESS_PWM, 128);         // Половина мощности прижима
  digitalWrite(RELAY_DOWN, RELAY_ON);  // Опускание прижимного механизма
  pressActive = true;                  // Активация прижима
}

void handleCheckRoutine(float weight) {
  // Обработка проверочной процедуры
  if (!checkPressed && abs(weight) > 50 && millis() - checkStartTime > 2000) { // Проверка веса после 2 секунд
    checkPressed = true;               // Установка флага нажатия
    analogWrite(MOTOR_PWM, 0);         // Остановка мотора
    digitalWrite(RELAY_DOWN, RELAY_OFF); // Отключение опускания
    digitalWrite(RELAY_UP, RELAY_ON);  // Включение подъема
    delay(LIFT_DURATION);              // Ожидание 1 секунды
    digitalWrite(RELAY_UP, RELAY_OFF); // Отключение подъема
    Serial.println("8888");            // Сообщение о завершении
    stopAll(false);                    // Остановка без дополнительного подъема
  }
}