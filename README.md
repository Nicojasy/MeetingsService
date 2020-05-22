# Cервис для постановки встреч


## Детали реализации
* Приложение на ASP.NET Core 3.1 Web API;
* Для создания базы данных использовал EF Core + Sql server; 
*	При первом запуске в Program.cs создаётся бд и вносятся начальные данные. С этого момента данные будут сохраняться;
*	Инструкция по подключению ниже.

Реализованные методы:
* Поставить встречу (AddMeeting);
* Отменить встречу (DeleteMeeting);
* Добавить участников (AddAttendee);
* Удалить участника (DeleteAttendee);
* Вывести список встреч с участниками (ViewAllMeetings).

Методы обрабатывают HTTP POST запросы с телом, содержащим все необходимые параметры в JSON.
xUnit тест на метод добавления встречи. 

Реализовано также:
* Проверка занято/свободно время участников;
* Проверка правильности введённой эл. почты.

В дальнейшем также будет, но пока ещё не реализовано:
* Отправка ссылки для подтверждения добавленного сотрудника на эл. почту (с помощью mailkit и token-а);
* Отправка приглашение на встречу всем участникам в виде емэйл;
* За 15 минут до встречи отправить напоминание.

P.S.: отправка сообщения на почту реализована при добавлении сотрудника, но, к сожалению, правильно настроить пока не получилось.

## Работа с проектом

### Для работы приложения включены пакеты:
Работа EF Core:
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Для отправки email и создания Token:
MailKit
Microsoft.IdentityModel.Tokens
Для xUnit-тестирования:
xunit
xunit.runner.visualstudio
Microsoft.NET.Test.Sdk
Moq

### Запуск:
В проекте необходимо открыть и перед запуском настроить:
* Необходимо изменить значение "DefaultConnection" на актуальное для настройки бд по пути "\MeetingService\appsettings.json"
```json
...
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=meetingsservice;Trusted_Connection=True;MultipleActiveResultSets=true",
  },
...
```
* Дополнительно для работы отправки сообщений при реализации необходимо изменить данные в "\MeetingService\Services\EmailService.cs" и "\MeetingService\Config\AuthOptions.cs" на актуальные.

Если уже имеется VS, то после запуска ctr+f5 в консоли диспетчера или в powershell (на разных ос могут быть различия) можно работать через утилиту Invoke-RestMethod:

Запуск метода ViewAllMeetings:

Invoke-RestMethod http://localhost:50590/api/meetings/viewallmeetings -Method POST

Запуск метода DeleteAttendee:

Invoke-RestMethod http://localhost:50590/api/meetings/deleteattendee/2 -Method POST

Запуск метода DeleteMeeting

Invoke-RestMethod http://localhost:50590/api/meetings/deletemeeting/2 -Method POST

Запуск метода AddMeeting:

Invoke-RestMethod http://localhost:50590/api/meetings/AddMeeting -Method POST -Body (@{title = "Meeting6"; datetimestart = "05/10/2020 13:00"; datetimeend = "05/10/2020 17:00"} | ConvertTo-Json) -ContentType "application/json; charset=utf-8"

Запуск метода AddAttendee:

$body = !здесь должны быть данные участников, но пока не смог правильно их внести
!данные в json:

```json
{
    "meetingid": "10",
    "attendee": [
        {
            "name": "Jake",
            "email": "jake@gmail.com"
        },
        {
            "name": "Kate",
            "email": "Kate@gmail.com"
        }
    ]
}
```

Invoke-RestMethod http://localhost:50590/api/meetings/AddAttendee -Method POST -Body ($body | ConvertTo-Json) -ContentType "application/json; charset=utf-8"

Имеются ещё методы ConfirmEmail, на который должен проходить по ссылке участник и часть кода в теле метода AddAttendee, но они пока не настроены до конца.

Метод AddAttende можно протестировать с помощью Postman:
путь указать:

Invoke-RestMethod http://localhost:50590/api/meetings/AddAttendee -Method POST -Body ($body | ConvertTo-Json) -ContentType "application/json; charset=utf-8"

В Body->raw->json внести:

```json
{
    "meetingid": "10",
    "attendee": [
        {
            "name": "Jake",
            "email": "jake@gmail.com"
        },
        {
            "name": "Kate",
            "email": "Kate@gmail.com"
        }
    ]
}
```

Затем сделать запрос и вернуться значения, кто был добавлен, а кто был отклонён.
