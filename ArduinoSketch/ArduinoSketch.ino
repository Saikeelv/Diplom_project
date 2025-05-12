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
const float SPEED_THRESHOLD = 9000.0; // Порог скорости (11000 имп/мин)
const int MAX_TRIALS = 3;             // Максимальное количество испытаний
const float WEIGHT_THRESHOLD = 50.0;   // Порог веса для уменьшения мощности прижима (в граммах)
const int LIFT_DURATION = 2000;        // Время подъема прижимного механизма (2 секунды)
const float TEMP_MIN = 0.0;            // Минимальная допустимая температура (°C)
const float TEMP_MAX = 100.0;          // Максимальная допустимая температура
const float CURRENT_MAX = 20.0;        // Максимальный допустимый ток
const float WEIGHT_MIN = -2000.0;      // Минимальный допустимый вес
const float WEIGHT_MAX = 10000.0;      // Максимальный допустимый вес
const float SPEED_MAX = 20000.0;       // Максимальная допустимая скорость (имп/мин)
const unsigned long SPIN_UP_DELAY = 1000; // Задержка раскрутки мотора (1 секунда)

// --- Калибровка HX711 ---
HX711 scale;                           // Объект для работы с тензодатчиком
float calibration_factor = -0.381;     // Коэффициент калибровки тензодатчика

// --- Коэффициенты для термистора (Стейнхарт-Харт) ---
const float A = 1.009249522e-03;
const float B = 2.378405444e-04;
const float C = 2.019202697e-07;

// --- Переменные состояния ---
bool experimentRunning = false;        // Флаг выполнения эксперимента
bool checking = false;                 // Флаг выполнения проверочной процедуры
bool lifting = false;                  // Флаг подъема прижимного механизма
int trialCounter = 0;                  // Счетчик испытаний
int packetCounter = 0;                 // Счетчик пакетов данных
volatile unsigned long pulseCount = 0; // Счетчик импульсов от ИК-датчика
unsigned long lastSpeedTime = 0;       // Время последнего измерения скорости
unsigned long lastDataTime = 0;        // Время последней отправки данных
unsigned long liftStartTime = 0;       // Время начала подъема
unsigned long experimentStartTime = 0; // Время начала эксперимента
int currentError = 0;                  // Код текущей ошибки
unsigned long checkStartTime = 0;      // Время начала проверочной процедуры
bool checkPressed = false;             // Флаг нажатия в проверочной процедуре
unsigned long pressStartTime = 0;      // Время начала работы прижима
bool pressActive = false;              // Флаг активности прижимного механизма

// --- Функция расчёта температуры для термистора ---
float Thermistor(int Vo) {
  float logRt = log(10000.0 * ((1024.0 / Vo - 1))); // Логарифм сопротивления термистора
  float T = 1.0 / (A + B * logRt + C * logRt * logRt * logRt); // Температура в Кельвинах
  float Tc = T - 273.15; // Температура в градусах Цельсия
  return Tc - 2; // Коррекция на -2°C
}

// --- Обработчик прерывания для датчика скорости ---
void speedSensorISR() {
  pulseCount++;                        // Увеличение счетчика импульсов
}

void setup() {
  Serial.begin(115200);                // Инициализация последовательного порта
  
  // Инициализация пинов
  pinMode(SENSOR_SPEED_PIN, INPUT);    // ИК-датчик как вход
  pinMode(MOTOR_PWM, OUTPUT);          // ШИМ двигателя как выход
  pinMode(MOTOR_DIR, OUTPUT);          // Направление двигателя
  pinMode(PRESS_PWM, OUTPUT);          // ШИМ прижима как выход
  pinMode(PRESS_DIR, OUTPUT);          // Направление прижима
  pinMode(RELAY_UP, OUTPUT);           // Реле подъема как выход
  pinMode(RELAY_DOWN, OUTPUT);         // Реле опускания как выход
  digitalWrite(MOTOR_DIR, HIGH);       // Мотор вперед
  digitalWrite(PRESS_DIR, HIGH);       // Прижим фиксированное направление
  digitalWrite(RELAY_UP, RELAY_OFF);   // Начальное состояние: реле выключены
  digitalWrite(RELAY_DOWN, RELAY_OFF);
  analogWrite(MOTOR_PWM, 0);           // Мотор выключен
  analogWrite(PRESS_PWM, 0);           // Прижим выключен

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

  // Управление подъемом (неблокирующая логика)
  if (lifting && millis() - liftStartTime >= LIFT_DURATION) {
    digitalWrite(RELAY_UP, RELAY_OFF); // Отключение реле подъема
    analogWrite(PRESS_PWM, 0);         // Отключение прижима
    lifting = false;                   // Сброс флага подъема
    // Serial.print("Lift ended, experimentRunning="); Serial.print(experimentRunning);
    // Serial.print(", trialCounter="); Serial.println(trialCounter);
    if (experimentRunning && trialCounter < MAX_TRIALS) {
      Serial.println("1111");          // Вывод 1111 после каждого испытания (кроме последнего)
      startTrial();                    // Запустить следующее испытание
      // Serial.print("Starting trial: "); Serial.println(trialCounter);
    } else if (experimentRunning) {
      Serial.println("9999");          // Вывод 9999 после 10-го испытания
      Serial.flush();
      experimentRunning = false;        // Завершение эксперимента
      stopAll(false);                  // Остановка без подъема
    } else if (checking) {
      Serial.println("8888");          // Вывод 8888 для режима 2
      Serial.flush();
      checking = false;                // Завершение проверки
      stopAll(false);                  // Остановка без подъема
    }
  }

  if ((experimentRunning || checking) && !lifting) { // Если идет эксперимент или проверка
    // Чтение веса один раз за цикл
    float units = scale.get_units(5);  // Усреднение 5 измерений
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
        float units = scale.get_units(5);
        float weight = units * 0.035274 / 10;
        readAndSendData(weight);       // Последний пакет
        Serial.println("9999");        // Код завершения
        Serial.flush();
        experimentRunning = false;      // Завершение эксперимента
        stopAll(true);                 // Остановка с подъемом
      } else if (checking) {
        float units = scale.get_units(5);
        float weight = units * 0.035274 / 10;
        readAndSendData(weight);       // Последний пакет
        Serial.println("8888");        // Код завершения проверки
        Serial.flush();
        checking = false;              // Завершение проверки
        stopAll(true);                 // Остановка с подъемом
      }
    } else if (cmd == '2') {           // Команда запуска проверки
      Serial.println("2222");          // Подтверждение
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
  experimentStartTime = millis();      // Время начала эксперимента
  currentError = 0;                    // Сброс ошибок
  lifting = false;                     // Сброс флага подъема
  checking = false;                    // Сброс проверки
  Serial.println("1111");              // Подтверждение запуска
  startTrial();                        // Запуск первого испытания
  // Serial.print("Experiment started, trial: "); Serial.println(trialCounter);
}

void startTrial() {
  // Запуск одного испытания
  if (trialCounter >= MAX_TRIALS) {    // Если достигнуто максимум испытаний
    return;                            // Завершение обрабатывается в loop
  }
  trialCounter++;                      // Увеличение счетчика испытаний
  packetCounter = 0;                   // Сброс счетчика пакетов для нового испытания
  digitalWrite(MOTOR_DIR, HIGH);       // Мотор вперед
  digitalWrite(PRESS_DIR, HIGH);       // Прижим фиксированное направление
  analogWrite(MOTOR_PWM, 255);         // Полная мощность двигателя
  analogWrite(PRESS_PWM, 128);         // Половина мощности прижима
  digitalWrite(RELAY_DOWN, RELAY_ON);  // Опускание прижимного механизма
  digitalWrite(RELAY_UP, RELAY_OFF);
  pressStartTime = millis();           // Запись времени начала
  pressActive = true;                  // Активация прижима
  // Serial.print("Trial started: "); Serial.println(trialCounter);
}

void stopAll(bool lift) {
  // Остановка всех механизмов
  analogWrite(MOTOR_PWM, 0);           // Отключение двигателя
  analogWrite(PRESS_PWM, 0);           // Отключение прижима
  digitalWrite(RELAY_DOWN, RELAY_OFF); // Отключение реле опускания
  digitalWrite(MOTOR_DIR, HIGH);       // Сохраняем направление
  digitalWrite(PRESS_DIR, HIGH);       // Сохраняем направление

  if (lift) {                          // Если требуется подъем
    lifting = true;                    // Устанавливаем флаг подъема
    liftStartTime = millis();          // Запись времени начала подъема
    digitalWrite(RELAY_UP, RELAY_ON);  // Включение реле подъема
    digitalWrite(RELAY_DOWN, RELAY_OFF);
    analogWrite(PRESS_PWM, 255);       // Максимальная мощность прижима
  } else {
    // Сброс флагов только при полной остановке (например, после 9999 или 8888)
    experimentRunning = false;
    pressActive = false;
    checking = false;
    trialCounter = 0;
  }
}

void readAndSendData(float weight) {
  // Чтение и отправка данных
  packetCounter++;                     // Увеличение счетчика пакетов

  // Расчет скорости (импульсы/мин)
  unsigned long currentTime = millis();
  float elapsedTime = (currentTime - lastSpeedTime) / 1000.0; // Время в секундах
  float speed = (elapsedTime > 0) ? pulseCount * 60.0 / elapsedTime : 0; // Импульсы/мин
  // Serial.print("PulseCount="); Serial.println(pulseCount); // Отладка до сброса
  pulseCount = 0;                      // Сброс счетчика импульсов
  lastSpeedTime = currentTime;         // Обновление времени
  // Serial.print("Speed="); Serial.println(speed);
  // Serial.print("TrialTime="); Serial.println(millis() - pressStartTime); // Время с начала испытания

  // Чтение датчиков
  float current1 = readCurrent(CURRENT_SENSOR1); // Ток двигателя
  float current2 = readCurrent(CURRENT_SENSOR2); // Ток прижима
  float temp = Thermistor(analogRead(TEMP_SENSOR)); // Температура с термистора

  // Проверка ошибок (только для эксперимента, не для проверки)
  if (experimentRunning && checkErrors(temp, weight, speed, current1, current2)) {
    Serial.print(packetCounter); Serial.print(";");
    Serial.print(millis() - experimentStartTime); Serial.print(";");
    Serial.print(temp); Serial.print(";");
    Serial.print(weight); Serial.print(";");
    Serial.print(speed); Serial.print(";");
    Serial.print(trialCounter); Serial.print(";");
    Serial.println(currentError);       // Пакет с ошибкой
    Serial.println("9999");            // Код завершения при ошибке
    Serial.flush();
    experimentRunning = false;          // Завершение эксперимента
    stopAll(true);                     // Остановка с подъемом
    return;
  }

  // Проверка скорости для завершения испытания (после 1 с раскрутки)
  if (experimentRunning && speed <= SPEED_THRESHOLD && pressActive && millis() - pressStartTime >= SPIN_UP_DELAY) {
    analogWrite(MOTOR_PWM, 0);         // Остановка мотора
    stopAll(true);                     // Поднять прижим
    // Serial.print("Trial ended, speed="); Serial.println(speed);
    return;                            // Ожидать подъема
  }
  if(weight < 0 && weight > WEIGHT_MIN) 
  {
    weight = 0;
  }

  // Отправка данных
  if (experimentRunning) {
    Serial.print(packetCounter); Serial.print(";");
    Serial.print(millis() - experimentStartTime); Serial.print(";");
    Serial.print(temp); Serial.print(";");
    Serial.print(weight); Serial.print(";");
    Serial.print(speed); Serial.print(";");
    Serial.print(trialCounter); Serial.print(";");
    Serial.println(currentError);
  } else if (checking) {
    Serial.print(packetCounter); Serial.print(";");
    Serial.print(current1); Serial.print(";");
    Serial.print(current2); Serial.print(";");
    Serial.print(speed); Serial.print(";");
    Serial.print(temp); Serial.print(";");
    Serial.print(weight); Serial.print(";");
    Serial.println(currentError);
  }
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
    // Serial.print("Error 12: Temp="); Serial.println(temp);
    return true;
  }
  if (temp > 80.0) {
    currentError = 13; // Перегрев масла
    // Serial.print("Error 13: Temp="); Serial.println(temp);
    return true;
  }

  // Вес
  // if (!scale.is_ready()) {
  //   currentError = 21; // Датчик веса не отвечает
  //   Serial.println("Error 21: Scale not ready");
  //   return true;
  // }
  if (weight < WEIGHT_MIN || weight > WEIGHT_MAX) {
    currentError = 22; // Некорректные показания веса
    // Serial.print("Error 22: Weight="); Serial.println(weight);
    return true;
  }
  if (pressActive && abs(weight) < 10.0 && millis() - pressStartTime > 32000) {
    currentError = 23; // Поломка прижимного механизма
    // Serial.println("Error 23: Press failure");
    return true;
  }

  // Скорость
  // if (speed > SPEED_MAX) {
  //   currentError = 32; // Некорректные показания скорости
  //   Serial.print("Error 32: Speed="); Serial.println(speed);
  //   return true;
  // }
  if (speed == 0 && pressActive && millis() - pressStartTime > 2000) {
    currentError = 33; // Зазор в датчике скорости
    // Serial.println("Error 33: Speed gap");
    return true;
  }

  // Ток двигателя
  if (current1 < -CURRENT_MAX || current1 > CURRENT_MAX) {
    currentError = 42; // Некорректные показания тока двигателя
    // Serial.print("Error 42: Current1="); Serial.println(current1);
    return true;
  }
  if (current1 > 30.0) {
    currentError = 43; // Перегрузка тока двигателя
    // Serial.print("Error 43: Current1="); Serial.println(current1);
    return true;
  }

  // Ток прижимного механизма
  if (current2 < -CURRENT_MAX || current2 > CURRENT_MAX) {
    currentError = 52; // Некорректные показания тока прижима
    // Serial.print("Error 52: Current2="); Serial.println(current2);
    return true;
  }
  if (current2 > 30.0) {
    currentError = 53; // Перегрузка тока прижима
    // Serial.print("Error 53: Current2="); Serial.println(current2);
    return true;
  }

  return false; // Ошибок нет
}

void adjustPressPower(float weight) {
  // Регулировка мощности прижимного механизма
  if (abs(weight) > WEIGHT_THRESHOLD) {
    analogWrite(PRESS_PWM, 85);        // Уменьшение до 1/3 мощности (255/3)
  } else {
    analogWrite(PRESS_PWM, 128);       // Половина мощности
  }
}

void startCheck() {
  // Запуск проверочной процедуры
  checking = true;                     // Установка флага проверки
  checkStartTime = millis();           // Запись времени
  checkPressed = false;                // Сброс флага нажатия
  currentError = 0;                    // Сброс ошибки
  packetCounter = 0;                   // Сброс счетчика пакетов
  pulseCount = 0;                      // Сброс счетчика импульсов
  lastSpeedTime = millis();            // Запись времени
  lastDataTime = millis();             // Сброс времени отправки данных
  experimentRunning = false;            // Сброс эксперимента

  digitalWrite(MOTOR_DIR, HIGH);       // Мотор вперед
  digitalWrite(PRESS_DIR, HIGH);       // Прижим фиксированное направление
  analogWrite(MOTOR_PWM, 255);         // Полная мощность двигателя
  analogWrite(PRESS_PWM, 128);         // Половина мощности прижима
  digitalWrite(RELAY_DOWN, RELAY_ON);  // Опускание прижимного механизма
  digitalWrite(RELAY_UP, RELAY_OFF);
  pressActive = true;                  // Активация прижима
}

void handleCheckRoutine(float weight) {
  // Обработка проверочной процедуры
  if (!checkPressed && abs(weight) > 50 && millis() - checkStartTime > 2000) { // Проверка веса после 2 секунд
    checkPressed = true;               // Установка флага нажатия
    analogWrite(MOTOR_PWM, 0);         // Остановка мотора
    digitalWrite(RELAY_DOWN, RELAY_OFF); // Отключение опускания
    digitalWrite(RELAY_UP, RELAY_ON);  // Включение подъема
    digitalWrite(PRESS_DIR, HIGH);     // Сохраняем направление
    analogWrite(PRESS_PWM, 255);       // Максимальная мощность прижима
    lifting = true;                    // Устанавливаем флаг подъема
    liftStartTime = millis();          // Запись времени начала подъема
  }
}