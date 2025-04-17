# Микросервис “Расписание”
```Configuration```

Сборка проекта

```Data```

Подключение к базе данных

```Controller```

В контроллере происходит подключение к беку

```Model``` -> ```Repository```

В репозитории будут написаны базовые запросы напрямую в бд

```Services```

В сервисе бизнес логики будут написаны какие-то специфические требования
(напиример можно добавить логику заполнения расписания наперëд с учётом праздников)

```lessonapi.yaml```
файл с API

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

	- При установке выбрать такой же пароль к БД, как на сервере, имя пользователя, порт, название БД оставить по умолчанию, путь установки по умолчанию
 	- При выборе компонентов обязательно выбрать PostgreSQL Server и Command Line Tools

1. Инициализировать БД через скрипт Scripts/init_db.bat

2. Теперь можно запускать проект через Visual Studio
