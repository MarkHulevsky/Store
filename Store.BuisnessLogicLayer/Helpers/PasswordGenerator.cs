using Fare;
using System;

namespace Store.BuisnessLogic.Helpers
{
    public static class PasswordGenerator
    {
        private const string PATTERN = @"[a-z0-9A-Z]";
        private const int PASSWORD_LENGTH = 10;

        public static string GeneratePassword()
        {
            var rand = new Random();
            var xeger = new Xeger(PATTERN, rand);

            var password = string.Empty;

            for (int i = 0; i < PASSWORD_LENGTH; i++)
            {
                password += xeger.Generate();
            }
            return password;
        }
    }
}
