#language: ru-RU
Функция: Как пользователь, я могу попытаться отправить запрос на умную ссылку с методом, который не поддерживается приложением, чтобы увидеть ответ о неподдерживаемом методе.

Сценарий: Приложение отвечает 405 Metod Not Allowed ошибкой на POST запрос, если для запрашиваемой умной ссылки определены правила редиректа
    Дано Для умной ссылки /post-exists определены правила редиректа
    Когда Клиент отправляет POST-запрос на url /post-exists 
    Тогда Приложение отвечает 405 Method Not Allowed ошибкой

Сценарий: Приложение не отвечает 405 Not Found ошибкой на GET запрос
    Дано Для умной ссылки /post-non-exists не определены правила редиректа
    Когда Клиент отправляет GET-запрос на url /post-non-exists 
    Тогда Приложение не отвечает 405 Method Not Allowed ошибкой

Сценарий: Приложение отвечает 405 Method Not Allowed ошибкой на POST запрос, если для запрашиваемой умной ссылки не определены правила редиректа, 
    Дано Для умной ссылки /post-non-exists не определены правила редиректа
    Когда Клиент отправляет POST-запрос на url /post-non-exists 
    Тогда Приложение отвечает 405 Method Not Allowed ошибкой