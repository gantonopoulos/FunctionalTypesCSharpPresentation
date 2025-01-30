using LanguageExt;

namespace FunctionalStructures;

public class UserRegistration:IEquatable<UserRegistration>
{
    public Option<string> Name { get; }

    public Email Email { get; }
    
    private UserRegistration(Option<string> name, Email email)
    {
        Name = name;
        Email = email;
    }

    public static UserRegistration Create(Option<string> name, Email email)
    {
        return new UserRegistration(name, email);
    }

    public override string ToString()
    {
        return $"Name: {PrintName()}, Email:{Email}";
    }

    private string PrintName()
    {
        return Name.Match
        (
            someName => someName,
            () => "Unknown"
        );
    }

    public bool Equals(UserRegistration? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name.Equals(other.Name) && Email.Equals(other.Email);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((UserRegistration)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Name, Email);
}