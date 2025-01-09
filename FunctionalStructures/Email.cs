using System.Text.RegularExpressions;

namespace FunctionalStructures;

public readonly struct Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(value));
        }

        if (!EmailRegex.IsMatch(value))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }

        Value = value;
    }
    
    public static implicit operator string(Email email) => email.Value;

    public override string ToString() => Value;

}