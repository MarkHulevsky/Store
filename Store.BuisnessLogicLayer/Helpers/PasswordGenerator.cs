using Fare;
using System;

namespace Store.BuisnessLogicLayer.Helpers
{
    public static class PasswordGenerator
    {
        private const string pattern = @"[a-z0-9A-Z]";
        private const int _passwordLength = 10;

        public static string GeneratePassword()
        {
            var rand = new Random();
            var xeger = new Xeger(pattern, rand);

            var password = string.Empty;

            for (int i = 0; i < _passwordLength; i++)
            {
                password += xeger.Generate();
            }
            return password;
        }
    }
}
