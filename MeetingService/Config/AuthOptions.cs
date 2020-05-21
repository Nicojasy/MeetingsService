using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MeetingsService
{
    public class AuthOptions
    {
        public const string ADRESS = "dimataranapa12@gmail.com";
        public const string ISSUER = "MeetingsService";
        public const string AUDIENCE = "Attendee";
        const string KEY = "jgkkdG94G88g9super_Key649GJJVvY667bgyfTd8f";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
