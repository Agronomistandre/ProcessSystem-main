# ProcessSystem
## Тестовый проект. Реализован API регистрации и отмены регистрации в каком-то сервисе

### На выбор предлагаются задания:

* По примеру ProcessSystem.UnitTests\RegisterTest.cs написать юнит-тесты для генерации токена и для методов FindByNameAndUrlAsync и FindByTokenAsync, а так же для проверки RegisterRequest.
* По примеру ProcessSystem.IntegrationsTest\RegisterTest.cs написать тесты, которые проверяют ошибочное поведение при вызове  Register и Unregister
* Добавить контроллер с методом StartProcess. Сделать имитацию запуска процесса из списка, полученного при регистрации в конце выполнить Post запрос на url из запроса
* Добавить параметр в файл appsettings. Использовать его в методе Register с помощью интерфейса IOptions
* Добавить микросервис, в который будет послан Post запрос после успешного выполнения метода Register
* Создать 2 версию контроллера RegisterController. Сделать доступными и 1 и 2 версию
* _Бонус_ Сделать в БД таблицу для хранения истории обращения к таблице register


Архитектура микросервисов
https://docs.microsoft.com/ru-ru/dotnet/architecture/microservices/

https://github.com/dotnet-architecture/eShopOnContainers
