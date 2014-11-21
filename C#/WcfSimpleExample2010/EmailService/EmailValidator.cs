using System;
using System.Text.RegularExpressions;

namespace EmailService
{
    public class EmailValidator : IEmailValidator
    {
        public bool ValidateAddress(string emailAddress)
        {
            Console.WriteLine("Validating: {0}", emailAddress);
            string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";
            return Regex.IsMatch(emailAddress, pattern);
        }
    }
}
