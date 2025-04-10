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

### Первичный запуск базы данных
1. Склонировать репозиторий, зайти в папку с файлом compose.yml

	`git clone https://github.com/alenkaLo/Microservice-schedule-.git`

2. Запустить контейнер с PostgreSQL

	`docker-compose up -d`
3. Проверить, что БД работает

	`docker ps`

4. Инициализировать базу данных (только при первом запуске)

	`docker exec -i microservice-schedule--db-1 psql -U postgres -f init.sql`
    
В дальнейшем можно включать/выключать контейнер `docker-compose up -d`/`docker-compose down`. Данные будут храниться в папке `pgdata`, которая создается при запуске контейнера.
