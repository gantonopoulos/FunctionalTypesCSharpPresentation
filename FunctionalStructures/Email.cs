using System.Text.RegularExpressions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FunctionalStructures;

public readonly struct Email: IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Either<string, Email> Create(string email)
    {
        if (email is null)
            throw new ArgumentNullException(nameof(email), "Email cannot be null");

        if (string.Empty.Equals(email))
            return Left("Email cannot be empty!");
        
        if (!EmailRegex.IsMatch(email))
            return Left($"Invalid email format:{email}");

        return Right(new Email(email));
    }

    public static implicit operator string(Email email) => email.Value;

    public override string ToString() => Value;

    public bool Equals(Email other) => Value == other.Value;

    public override bool Equals(object? obj) => obj is Email other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();
}