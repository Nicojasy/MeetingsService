using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MeetingsService
{
    public class AuthOptions
    {
        public const string ADRESS = "email@gmail.com";
        public const string ISSUER = "MeetingsService";
        public const string AUDIENCE = "Attendee";
        const string KEY = "super_key";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
