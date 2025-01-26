using System.Text.RegularExpressions;
using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace FunctionalStructures;

public readonly struct Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Option<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return None;
            //throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        if (!EmailRegex.IsMatch(email))
        {
            return None;
            //throw new ArgumentException("Invalid email format.", nameof(email));
        }

        return Some(new Email(email));
    }
    
    public static implicit operator string(Email email) => email.Value;

    public override string ToString() => Value;

}