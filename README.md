# ResetApiServerKMEPT - Локальный мессенджер с WebAPI

![Android](https://img.shields.io/badge/Android-3DDC84?style=for-the-badge&logo=android&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)

Проект представляет собой локальный мессенджер для Android с REST API сервером на .NET. Все данные хранятся и обрабатываются локально без использования облачных сервисов.

## 🔥 Основные возможности

- 📱 Локальный обмен сообщениями между пользователями
- 🔐 JWT-аутентификация
- 📡 REST API для управления сообщениями
- 💾 Локальное хранение данных (SQLite)
- 📊 Swagger-документация API

## 🛠 Технологический стек

**Серверная часть:**
- ASP.NET Core 7
- Entity Framework Core
- JWT Authentication
- SQLite Database
- Swagger UI

**Клиентская часть (Android):**
- Kotlin
- Retrofit (для работы с API)
- Room (локальное кэширование)

## 🚀 Быстрый старт

### Требования
- .NET 7 SDK
- Android Studio (для клиентской части)
- Любая SQLite-совместимая БД

### Запуск сервера
```bash
git clone https://github.com/CoderCppH/ResetApiServerKMEPT.git
cd ResetApiServerKMEPT
dotnet run
