// Глобальные переменные
unsigned long startTime = 0;    // Время начала эксперимента
int experimentNumber = 0;       // Номер текущего испытания
int packetNumber = 0;          // Номер пакета данных
bool isRunning = false;        // Флаг выполнения эксперимента
bool isChecking = false;       // Флаг режима проверки
const int MAX_EXPERIMENTS = 10; // Максимальное количество испытаний
const int BUTTON_PIN = 4;      // Пин для кнопки (можно изменить)

// Константы для расчётов
const float MAX_FORCE = 10.0;      // Максимальная сила прижима
const float MAX_SPEED = 3650.0;    // Максимальная скорость
const float MIN_SPEED = 3000.0;    // Минимальная скорость
const int EXPERIMENT_DURATION = 5;  // Длительность эксперимента в секундах
const int CHECK_DURATION = 5;      // Длительность проверки в секундах
const int UPDATE_INTERVAL = 100;   // Интервал обновления в мс (10 раз в секунду)

void setup() {
  Serial.begin(115200);  // Увеличиваем скорость порта
  randomSeed(analogRead(0));  // Инициализация генератора случайных чисел
  pinMode(BUTTON_PIN, INPUT_PULLUP);  // Настраиваем пин кнопки с подтяжкой вверх
  Serial.println("Arduino ready");  // Сообщение о готовности
}

void loop() {
  // Обработка входящих команд
  if (Serial.available() > 0) {
    char incomingChar = Serial.read();  // Читаем один символ
    
    
    if (incomingChar == '0') {  // Остановка
      isRunning = false;
      isChecking = false;
      experimentNumber = 0;
      packetNumber = 0;
      Serial.println("7777");
    }
    else if (incomingChar == '1') {  // Начало эксперимента
      if (!isRunning && !isChecking && experimentNumber < MAX_EXPERIMENTS) {
        isRunning = true;
        isChecking = false;
        startTime = millis();
        packetNumber = 0;
        Serial.println("1111");
      }
    }
    else if (incomingChar == '2') {  // Начало проверки
      if (!isRunning && !isChecking) {
        isChecking = true;
        isRunning = false;
        startTime = millis();
        packetNumber = 0;
        Serial.println("2222");  // Отметка начала проверки
      }
    }
  }

  // Проверка состояния кнопки
  static bool lastButtonState = HIGH;
  bool buttonState = digitalRead(BUTTON_PIN);
  if (buttonState == LOW && lastButtonState == HIGH && isRunning) {  // Нажатие кнопки
    // Отправляем пакет с ошибкой 505
    float currentTime = (millis() - startTime) / 1000.0;
    Serial.print(packetNumber);
    Serial.print(";");
    Serial.print(currentTime, 2);
    Serial.print(";");
    Serial.print(0.0, 2);  // Температура
    Serial.print(";");
    Serial.print(0.0, 2);  // Сила
    Serial.print(";");
    Serial.print(0.0, 2);  // Скорость
    Serial.print(";");
    Serial.print(experimentNumber + 1);
    Serial.print(";");
    Serial.println("43");  // Код ошибки
    
    // Завершаем эксперимент
    isRunning = false;
    Serial.println("9999");
  }
  lastButtonState = buttonState;

  // Режим эксперимента (1)
  if (isRunning) {
    unsigned long currentMillis = millis() - startTime;
    float currentTime = currentMillis / 1000.0;  // Текущее время в секундах
    
    static unsigned long lastUpdate = 0;
    
    if (currentTime <= EXPERIMENT_DURATION) {
      if (currentMillis - lastUpdate >= UPDATE_INTERVAL) {
        lastUpdate = currentMillis;
        
        float t = currentTime / EXPERIMENT_DURATION;  // Нормализованное время [0..1]
        
        // Температура - линейный рост (1 градус/минута)
        float temperature = (currentTime / 60.0);
        
        // Сила прижима - экспоненциальный рост от 0 до 10
        float force = MAX_FORCE * (1 - exp(-5 * t));
        float forceNoise = force * 0.05 * (random(-100, 101) / 100.0);  // ±5%
        force += forceNoise;
        
        // Скорость вращения - экспоненциальное падение от 3650 до 3000
        float speed = MIN_SPEED + (MAX_SPEED - MIN_SPEED) * exp(-5 * t);
        float speedNoise = speed * 0.05 * (random(-100, 101) / 100.0);  // ±5%
        speed += speedNoise;
        
        // Формируем пакет данных с номером пакета
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
        Serial.println("0");
        
        packetNumber++;
      }
    }
    else {
      experimentNumber++;
      if (experimentNumber >= MAX_EXPERIMENTS) {
        isRunning = false;
        Serial.println("9999");  // Конец всех испытаний
      }
      else {
        startTime = millis();
        packetNumber = 0;
        Serial.println("1111");  // Начало следующего испытания
      }
    }
  }

  // Режим проверки (2)
  if (isChecking) {
    unsigned long currentMillis = millis() - startTime;
    float currentTime = currentMillis / 1000.0;  // Текущее время в секундах
    
    static unsigned long lastUpdate = 0;
    
    if (currentTime <= CHECK_DURATION) {
      if (currentMillis - lastUpdate >= UPDATE_INTERVAL) {
        lastUpdate = currentMillis;
        
        float t = currentTime / CHECK_DURATION;  // Нормализованное время [0..1]
        
        // Температура - линейный рост (1 градус/минута)
        float temperature = (currentTime / 60.0);
        
        // Сила прижима и скорость берём как максимальные/минимальные значения
        float force = MAX_FORCE * (1 - exp(-5 * 1.0));
        float speed = MIN_SPEED;
        
        // Сила1 и Сила2 - линейный рост от 0.5 до 5
        float force1 = 0.5 + (5.0 - 0.5) * t;
        float force2 = 0.5 + (5.0 - 0.5) * t;
        
        // Формируем пакет данных для режима 2
        Serial.print(packetNumber);
        Serial.print(";");
        Serial.print(force, 2);
        Serial.print(";");
        Serial.print(speed, 2);
        Serial.print(";");
        Serial.print(temperature, 2);
        Serial.print(";");
        Serial.print(force1, 2);
        Serial.print(";");
        Serial.println(force2, 2);
        
        packetNumber++;
      }
    }
    else {
      isChecking = false;
      Serial.println("8888");  // Отметка конца проверки
    }
  }
}