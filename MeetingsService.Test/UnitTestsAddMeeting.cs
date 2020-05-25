using MeetingsService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using static MeetingsService.Controllers.Dto;

namespace MeetingsService.Test
{
    public class UnitTestsAddMeeting
    {
        private MeetingsController controller;

        public UnitTestsAddMeeting()
        {
            // Arrange
            controller = new MeetingsController(null);
        }

        [Theory]
        [InlineData("MeetingTestttttttttttttttttttttttttttttttttttttttttttttttttt", "05/09/2020 20:42","05/09/2020 20:52")]
        [InlineData(null, "07/09/2020 20:42", "07/09/2020 20:52")]
        public async Task AddMeetingReturnsBadRequestResultWhenIsValidNameBad(string _title, string _datetimeStart, string _datetimeEnd)
        {
            var result = await controller.AddMeeting(new MeetingDto { title = _title, datetimestart = _datetimeStart, datetimeend = _datetimeEnd }) as BadRequestObjectResult;

            Assert.Equal($"Error: {_title}", result.Value);
        }

        [Theory]
        [InlineData("MeetingTest1", "05.09.2020 20:42", "05/09/2020 20:52")]
        [InlineData("MeetingTest2", "05/09/2020 20:42 PM", "05/09/2020 20:52")]
        public async Task AddMeetingReturnsBadRequestResultWhenTryParseExactDatetimeStartIsBad(string _title, string _datetimeStart, string _datetimeEnd)
        {
            var result = await controller.AddMeeting(new MeetingDto { title = _title, datetimestart = _datetimeStart, datetimeend = _datetimeEnd }) as BadRequestObjectResult;

            Assert.Equal($"{_datetimeStart} is not in an acceptable format.", result.Value);
        }

        [Theory]
        [InlineData("MeetingTest1", "05/09/2020 20:42", "05.09.2020 20:52")]
        [InlineData("MeetingTest2", "05/09/2020 20:42", "05/09/2020 20:52 PM")]
        public async Task AddMeetingReturnsBadRequestResultWhenTryParseExactDatetimeEndIsBad(string _title, string _datetimeStart, string _datetimeEnd)
        {
            var result = await controller.AddMeeting(new MeetingDto { title = _title, datetimestart = _datetimeStart, datetimeend = _datetimeEnd }) as BadRequestObjectResult;

            Assert.Equal($"{_datetimeEnd} is not in an acceptable format.", result.Value);
        }

        [Theory]
        [InlineData("MeetingTest1", "05/09/2020 20:52", "05/09/2020 20:42")]
        [InlineData("MeetingTest2", "05/09/2020 20:42", "04/09/2020 20:52")]
        public async Task AddMeetingReturnsBadRequestResultWhenDatetimeStartGreaterThanDatetimeEnd(string _title, string _datetimeStart, string _datetimeEnd)
        {
            var result = await controller.AddMeeting(new MeetingDto { title = _title, datetimestart = _datetimeStart, datetimeend = _datetimeEnd }) as BadRequestObjectResult;

            Assert.Equal($"Error: {_datetimeStart}>{_datetimeEnd}", result.Value);
        }
    }
}
