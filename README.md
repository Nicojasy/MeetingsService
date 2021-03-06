# Cервис для постановки встреч

## Детали реализации
*   Приложение на ASP.NET Core 3.1 Web API;
*   Для создания базы данных использовал EF Core + Sql server; 
*   При первом запуске в Program.cs создаётся бд и вносятся начальные данные. С этого момента данные будут сохраняться;
*	Инструкция по подключению ниже.

Реализованные методы:
*   Поставить встречу (AddMeeting);
*   Отменить встречу (DeleteMeeting);
*   Добавить участников (AddAttendee);
*   Удалить участника (DeleteAttendee);
*   Вывести список встреч с участниками (ViewAllMeetings).

Методы обрабатывают HTTP POST запросы с телом, содержащим все необходимые параметры в JSON.
<br>
xUnit тест на метод добавления встречи имеется.

Реализовано также:
* Проверка занято/свободно время участников;
* Проверка правильности введённой эл. почты.

В дальнейшем также будет, но пока ещё не реализовано:
* Отправка ссылки для подтверждения добавленного сотрудника на эл. почту (с помощью mailkit и token-а);
* Отправка приглашение на встречу всем участникам в виде емэйл;
* За 15 минут до встречи отправить напоминание.

P.S.: отправка сообщения на почту реализована при добавлении сотрудника, но, к сожалению, правильно настроить пока не получилось.

## Работа с проектом

### Вводная

Есть 5 методов для работы с сервисом встреч:

*   Вывести список встреч с участниками (ViewAllMeetings)
    
    URL-адрес: http://localhost:50590/api/meetings/viewallmeetings
    
    Body: null
    
    Output: \[{title, datetimestart, datetimeend, attendeesDto: \[{name, email}..{..}]},..{..}]
    
*   Удалить участника (DeleteAttendee)
    
    URL-адрес: http://localhost:50590/api/meetings/deleteattendee/2
    
    Body: null
    
    Output: result_string
    
*   Отменить встречу (DeleteMeeting)
    
    URL-адрес: http://localhost:50590/api/meetings/deletemeeting/2
    
    Body: null
    
    Output: result_string
        
*   Поставить встречу (AddMeeting)

    URL-адрес: http://localhost:50590/api/meetings/AddMeeting
    
    Body: title, datetimestart, datetimeend
    
    Output: result_string
    
*   Добавить участников (AddAttendee)
    
    URL-адрес: http://localhost:50590/api/meetings/AddAttendee
    
    Body: meetingid, attendee: \[{name, email}..{..}]

    Output: result_string
    
### Для работы приложения включены пакеты

Работа EF Core:

> Microsoft.EntityFrameworkCore.SqlServer<br>
> Microsoft.EntityFrameworkCore.Tools

Для отправки email и создания Token:

> MailKit<br>
> Microsoft.IdentityModel.Tokens

Для xUnit-тестирования:

> xunit<br>
> xunit.runner.visualstudio<br>
> Microsoft.NET.Test.Sdk<br>
> Moq

### Перед запуском

В проекте необходимо открыть и перед запуском настроить:

1. Необходимо изменить значение "DefaultConnection" на актуальное для настройки бд по пути "\MeetingService\appsettings.json"

```json
...
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=meetingsservice;Trusted_Connection=True;MultipleActiveResultSets=true",
  },
...
```

2. Дополнительно для работы отправки сообщений при реализации необходимо изменить данные в "\MeetingService\Services\EmailService.cs" и "\MeetingService\Config\AuthOptions.cs" на актуальные.

3. Возможно, потребуется создать бд "meetingsservice" на сервере без создания таблиц.

### Запуск

#### powershell

Если уже имеется VS, то запускаем (ctr+f5). Теперь можно работать в консоли диспетчера проекта или в powershell (на разных ос могут быть различия) через утилиту Invoke-RestMethod:

Запуск метода ViewAllMeetings():

> Invoke-RestMethod http://localhost:50590/api/meetings/viewallmeetings -Method POST

Запуск метода DeleteAttendee(Id):

> Invoke-RestMethod http://localhost:50590/api/meetings/deleteattendee/2 -Method POST

Запуск метода DeleteMeeting(Id):

> Invoke-RestMethod http://localhost:50590/api/meetings/deletemeeting/2 -Method POST

Запуск метода AddMeeting(MeetingDto):

> Invoke-RestMethod http://localhost:50590/api/meetings/AddMeeting -Method POST -Body (@{title = "Meeting6"; datetimestart = "05/10/2020 13:00"; datetimeend = "05/10/2020 17:00"} | ConvertTo-Json) -ContentType "application/json; charset=utf-8"

Запуск метода AddAttendee(AttendeeDto):

> $body = //здесь должны быть введены данные участников <br>
> данные в JSON, которые должны будут записаны в body: <br>
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
><br>
> А затем вводим <br>
> Invoke-RestMethod http://localhost:50590/api/meetings/AddAttendee -Method POST -Body ($body | ConvertTo-Json) -ContentType "application/json; charset=utf-8"

Имеются ещё методы ConfirmEmail, на который должен проходить по ссылке участник из своей почты, и часть кода в теле метода AddAttendee, но они пока не настроены до конца.

#### postman

Метод AddAttende можно протестировать с помощью Postman.

1. Установить метод POST.

2. Указать путь:
> http://localhost:50590/api/meetings/AddAttendee

3. В Body->raw->json внести:

```json
{
    "meetingid": "1",
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

4. Отправить запрос.

В ответе вернутся значения, где будет указано: кто был добавлен, а кто был отклонён (в том числе по причине занятости участника).

Таким же образом тестируются остальные методы, где необходимо либо ввести ссылку, либо ещё указать json данные:

Запуск метода ViewAllMeetings():

> http://localhost:50590/api/meetings/viewallmeetings

Запуск метода DeleteAttendee(Id):

> http://localhost:50590/api/meetings/deleteattendee/2

Запуск метода DeleteMeeting(Id):

> http://localhost:50590/api/meetings/deletemeeting/2

Запуск метода AddMeeting(MeetingDto):

> http://localhost:50590/api/meetings/AddMeeting

> json данные для отправки<br>
```json
{
    "title": "MeetingN",
    "datetimestart": "07/08/2020 10:40",
    "datetimeend": "07/08/2020 12:10"
}
```

Запуск метода AddAttendee(AttendeeDto):

> http://localhost:50590/api/meetings/AddAttendee

> json данные для отправки<br>
```json
{
    "meetingid": "2",
    "attendee": [
        {
            "name": "Matt",
            "email": "matt@gmail.com"
        },
        {
            "name": "Julia",
            "email": "julia@gmail.com"
        },
        {
            "name": "Henry",
            "email": "henry@gmail.com"
        },
        {
            "name": "Jake",
            "email": "jake@gmail.com"
        }
    ]
}
```
