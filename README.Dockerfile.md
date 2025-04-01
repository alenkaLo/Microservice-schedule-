0) Установка Visual Studio и проверка сборки проекта:
 - Скачать https://github.com/alenkaLo/Microservice-schedule-.git
 - установить visual studio, можно без компонентов
 - открыть проект в visual studio, ПКМ->установить необходимые компоненты
 - f5. Должен открыться браузер с нужной страницей.

1) Установить Docker Desktop: https://desktop.docker.com/win/main/amd64/Docker%20Desktop%20Installer.exe

2) Открыть powershell в корневой папке проекта:
 - Запустить Docker Desktop (окно можно закрыть, главное - чтобы он был на фоне): `start "C:\Program Files\Docker\Docker\Docker Desktop.exe"`
 - Инициализировать user-secrets: `dotnet user-secrets init -p .\TimeTable\TimeTable.csproj`
 - Задать пароль для SSL-сертификата: `dotnet user-secrets -p TimeTable\TimeTable.csproj set "Kestrel:Certificates:Default:Password" "Pass1234"`
 - Создать папку для сертификата (костыль) `mkdir ${env:USERPROFILE}/.aspnet` `mkdir ${env:USERPROFILE}/.aspnet/https`
 - Добавить сертификат: `dotnet dev-certs https -ep "${env:USERPROFILE}\.aspnet\https\aspnetapp.pfx" --trust -p "Pass1234"`
 - Запустить контейнер: `docker compose up`

3) Теперь в браузере попробовать открыть: https://localhost:8001/swagger
