using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Superkatten.Katministratie.Application.Helpers;

class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            // Normalize the domain
            email = Regex.Replace(
                email, 
                @"(@)(.+)$", 
                domainMapper,
                RegexOptions.None, 
                TimeSpan.FromMilliseconds(200)
            );

            // Examines the domain part of the email and normalizes it.
            static string domainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            var result = Regex.IsMatch(
                email,
                @"/^[^@\s]+@[^@\s]+\.[^@\s]+$/g",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
            return result;
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
