# 🛡️ E2EE Messenger API (Lab 2)

Цей проєкт — це робочий прототип бекенду для месенджера з підтримкою наскрізного шифрування (End-to-End Encryption - Variant 8), побудований на ASP.NET Core та PostgreSQL.

## 🚀 Як запустити проєкт

1. **База даних:** Переконайтеся, що у вас запущений PostgreSQL. Створіть БД і виконайте SQL-скрипти для створення таблиць `users`, `conversations` та `messages` (є в коді/базі).
2. **Налаштування:** У файлі `MessengerApi/appsettings.json` вкажіть ваш `ConnectionString` до PostgreSQL.
3. **Запуск сервера:**
   ```bash
   cd MessengerApi
   dotnet run
