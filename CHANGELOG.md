# Лог изменений

Все заметные изменения в этом проекте будут отражаться в этом документе.

Формат лога изменений базируется на [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [1.4.6] - 2022-11-23

### Изменено

* Из методов интеграции `AddDbTools` извлечён перегруженный метод  конфигурирования `ConfigureDbTools`. Старые методы помечены как устаревшие.

## [1.3.5] - 2021-08-05

### Добавлено

* Логирование уровня `linq2db`

## [1.3.3] - 2021-07-28

### Добавлено

* Возможность при интеграции указать источник провайдера данных для инъекции

## [1.2.2] - 2020-12-01

### Изменено

* Переход на `linq2db v3.1.6`

## [1.2.1] - 2020-06-01

### Добавлено

* Короткий метод для доступа к таблицам `Tab<>()`

### Исправлено

* Не публиковался менеджер `IDbManager` при интеграции.

## [1.2.0] - 2020-05-18

### Добавлено

* автотранзакции.

## [1.1.0] - 2020-05-18

### Добавлено

* поддержка классической конфигурации строк подключения;
* метод `IDbManager.DoOnce`, для выполнения одиночных запросов.

### Изменено

* имя метода `IDbManager.Connect` на `IDbManager.Use`

## [1.0.0] - 2020-05-15

### Добавлено

* поддержка классической конфигурации строк подключения;
* поддержка кастомной конфигурации строк подключения с шаблонированием;
* менеджер работы с БД `IDbManager`, предоставляющий настроенное подключение;

