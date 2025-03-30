0) Установка Visual Studio и проверка сборки проекта:
 - Скачать https://github.com/alenkaLo/Microservice-schedule-.git
 - установить visual studio, можно без компонентов
 - открыть проект в visual studio, ПКМ->установить необходимые компоненты
 - f5. Должен открыться браузер с нужной страницей.

1) Установить Docker Desktop: https://desktop.docker.com/win/main/amd64/Docker%20Desktop%20Installer.exe

2) Открыть powershell в корневой папке проекта:
 - Инициализировать user-secrets: `dotnet user-secrets init -p .\TimeTable\TimeTable.csproj`
 - Задать пароль для SSL-сертификата (можно заменить Pass1234 на что-то более безопасное): `dotnet user-secrets -p TimeTable\TimeTable.csproj set "Kestrel:Certificates:Default:Password" "Pass1234"`
 - Запустить Docker Desktop (окно можно закрыть, главное - чтобы он был на фоне): `start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"`
 - Выполнить команды из Dockerfile: `docker build .`
 - Запустить контейнер: `docker run --rm -it -p 8001:8001 -e ASPNETCORE_HTTPS_PORTS=8001 -e ASPNETCORE_ENVIRONMENT=Development -v ${env:APPDATA}/microsoft/UserSecrets/:/home/app/.microsoft/usersecrets -v ${env:USERPROFILE}/.aspnet/https/:/https/ -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx`

3) Теперь в браузере попробовать открыть: https://localhost:8001/swagger
