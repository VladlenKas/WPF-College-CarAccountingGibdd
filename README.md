## 📦 Установка

### Шаг 1: Скачайте файлы

Скачайте два файла из этого релиза:

- 📥 `CarAccountingGibddSetup_v1.0.0.exe` — установщик приложения
- 📥 `gibdd.sql` — дамп базы данных MySQL

### Шаг 2: Установите приложение

1. Запустите `CarAccountingGibddSetup_v1.0.0.exe`
2. Следуйте инструкциям установщика
3. Приложение установится в `C:\Program Files\Car Accounting Gibdd\`

### Шаг 3: Настройте MySQL

#### Установите MySQL 8.0+ (если ещё не установлен)

Скачать: https://dev.mysql.com/downloads/installer/

#### Импортируйте базу данных через Data Import

1. Откройте MySQL Workbench
2. Подключитесь к localhost
3. В меню выберите: **Server → Data Import**
4. Выберите **Import from Self-Contained File**
5. Укажите путь к скачанному файлу `gibdd.sql`
6. В **Default Target Schema** выберите существующую базу или создайте новую (`gibdd`)
7. Нажмите **Start Import**
8. Дождитесь завершения импорта

#### Настройте подключение

1. Откройте файл: `C:\Program Files\Car Accounting Gibdd\config.json`
2. Измените пароль и версию MySQL:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;user=root;password=ВАШ_ПАРОЛЬ;database=gibdd"
  },
  "MySQLVersion": ВАША_ВЕРСИЯ
}
```
3. По умолчанию: password — root, MySQLVersion — 8.039


#### Запустите приложение

Найдите ярлык на рабочем столе или в меню "Пуск"

