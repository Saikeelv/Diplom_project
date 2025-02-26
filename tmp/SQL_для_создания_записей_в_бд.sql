-- 1. Создание записи в Guide
INSERT INTO Guide (unit_of_measure) 
VALUES ('километры');

-- 2. Создание записи в Type_engine
INSERT INTO Type_engine (Type_eng) 
VALUES ('SomeType');

-- 3. Создание записи в Engine
INSERT INTO Engine (Marka, Type_eng_FK) 
VALUES ('SomeMarka', (SELECT last_insert_rowid() FROM Type_engine));

-- 4. Создание записи в Number_engine
INSERT INTO Number_engine (Number_eng, Engine_FK) 
VALUES ('12345', (SELECT last_insert_rowid() FROM Engine));

-- 5. Создание записи в Engine_mileage
INSERT INTO Engine_mileage (Engine_mil, Guide_FK) 
VALUES (10000.0, (SELECT Guide_PK FROM Guide WHERE unit_of_measure = 'километры'));

-- 6. Создание записи в Oil_mileage
INSERT INTO Oil_mileage (Oil_mil, Guide_FK) 
VALUES (5000.0, (SELECT Guide_PK FROM Guide WHERE unit_of_measure = 'километры'));

-- 7. Создание записи в Datetime для Sample
INSERT INTO Datetime (Time, Date) 
VALUES ('12:00:00', '2023-01-01');

-- 8. Создание записи в Clients
INSERT INTO Client (FIO, Phone_num) 
VALUES ('Иван Иванов', '1234567890');

-- 9. Создание записи в Sample
INSERT INTO Sample (Note, Datetime_FK, Number_eng_FK, Engine_mileage_FK, Oil_mileage_FK, Client_FK) 
VALUES ('Заметка о пробе', 
        (SELECT Datetime_PK FROM Datetime WHERE Time = '12:00:00' AND Date = '2023-01-01'), 
        (SELECT Number_eng_PK FROM Number_engine WHERE Number_eng = '12345'), 
        (SELECT Engine_mileage_PK FROM Engine_mileage WHERE Engine_mil = 10000.0), 
        (SELECT Oil_mileage_PK FROM Oil_mileage WHERE Oil_mil = 5000.0), 
        (SELECT Client_PK FROM Client WHERE FIO = 'Иван Иванов'));

-- 10. Создание второй записи в Datetime для Experiment
INSERT INTO Datetime (Time, Date) 
VALUES ('13:00:00', '2023-01-02');

-- 11. Исправленный запрос для Experiment
INSERT INTO Experiment (Number, Error, Sample_FK, Datetime_FK) 
VALUES (1, 0, 
        (SELECT Sample_PK FROM Sample WHERE Note = 'Заметка о пробе'), 
        (SELECT Datetime_PK FROM Datetime WHERE Time = '13:00:00' AND Date = '2023-01-02'));

-- 12. Исправленный запрос для Data_of_exp
INSERT INTO Data_of_exp (run_of_test, num_of_error, Time, Temp, Power, Speed, Experiment_FK) 
VALUES (1, 0, '14:00:00', '25C', '100W', '50rpm', 
        (SELECT Experiment_PK FROM Experiment WHERE Number = 1 AND Error = 0));