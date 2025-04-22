# Микросервис “Расписание”

### Структура проекта
```
MICROSERVICE-SCHEDULE/
├── pgdata/               # Данные из бд
├── Scripts/              # Скрипты инициализации и бекапа бд  
├── Secrets/              # Пароль от бд
├── Tests/                # Тесты
├── TimeTable/            # 
│   ├── Configuration/    # Сборка проекта
│   ├── Contracts/        # Схема response (ответа API)
│   ├── Controllers/      # Работа с API, подключение к беку
│   ├── Data/             # Подключение к базе данных
│   ├── Logging/          # Логгер приложения, для вывода необходимой инфы в любом месте приложения
│   ├── Migration/        # Миграция бд
│   ├── Model/            # 
│   │    ├── Entity/      # Сущности
│   │    └── Repository/  # Базовые запросы напрямую в бд
│   ├── Properties/       # Настройки подключения по http
│   ├── Servise/          # Сервис бизнес логики содержит специфические требования
│   ├── ConsumerBase.cs   # Взаимодействие с кафкой в роли коньюмера
│   ├── KafkaModule.cs    # Взаимодействие с кафкой в роли продьюсера 
│   ├── Program.cs        # Точка входа
│   ├── TimeTable.csproj  # Настройка кафки, бд
│   ├── TimeTable.csproj.user  
│   ├── TimeTable.http       
│   ├── appsettings.Development.json 
│   └── appsettings.json
├── .gitignore            
├── compose.yml          # Конфигурация Docker
├── Dockerfile           # Конфигурация контейнера
├── init.sql             # Структура бд
└── lessonapi.yaml       # API документация 
```
### База данных

- PostgreSQL 16
- Автоматическое создание таблиц при запуске

## Требования

- Docker
- Docker Compose
- C# net8.0

## Разработка

### Первичный запуск
0. Установить Docker Desktop: https://desktop.docker.com/win/main/amd64/Docker%20Desktop%20Installer.exe

   	#### Если не удастся запустить Docker Desktop из-за ошибки с WSL/WSL2, перейти к "Первичный запуск без kafka и docker" ниже

2. Склонировать репозиторий, зайти в папку с файлом compose.yaml

	`git clone https://github.com/alenkaLo/Microservice-schedule-.git`

3. В папке Secrets создать файл pass.env с таким содержимым

   	`POSTGRES_PASSWORD=[ТУТ ПАРОЛЬ К БД]`
	
 	`POSTGRES_USER=postgres`
	
 	`POSTGRES_DB=postgres`

4. Запустить контейнер с PostgreSQL и kafka (Docker Desktop должен быть включён)

	`docker-compose up -d`

5. Проверить, что БД работает

	`docker ps`

6. Инициализировать базу данных (только при первом запуске)

	`docker exec -i microservice-schedule--db-1 psql -U postgres -f init.sql`

7. Теперь можно запускать проект через Visual Studio, http://localhost:5233/swagger/

### Повторный запуск после перезапуска ПК

1. В папке с файлом compose.yaml запустить контейнер с PostgreSQL и kafka (Docker Desktop должен быть включён)

	`docker-compose up -d`

2. Теперь можно запускать проект через Visual Studio, http://localhost:5233/swagger/

В дальнейшем можно включать/выключать контейнер `docker-compose up -d`/`docker-compose down`. Данные будут храниться в папке `pgdata`, которая создается при запуске контейнера.

### Возможные проблемы

 - Если при запуске БД ошибка "Error response from daemon: Container is restarting, wait until the container is running", то убедиться, что Docker Desktop включен, затем перезапустить контейнер:

	`docker-compose down`
	
	`docker-compose up -d`
 - Если при запросах через Swagger ошибки с типами данных, возможно локальная база данных устарела. Нужно удалить папку pgdata и повторить шаги 3-6 из первичного запуска

### Первичный запуск без kafka и Docker

0. Установить PostgreSQL 16.8: https://get.enterprisedb.com/postgresql/postgresql-16.8-1-windows-x64.exe

	- При установке выбрать такой же пароль к БД, как на сервере. Имя пользователя, порт, название БД, пусть установки и т.д. оставить по умолчанию
 	- При выборе компонентов обязательно выбрать PostgreSQL Server и Command Line Tools

1. Инициализировать БД через скрипт Scripts/init_db.bat (только при первом запуске)

2. В дальнейшем можно запускать проект через Visual Studio

