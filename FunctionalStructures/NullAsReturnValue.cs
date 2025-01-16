using FunctionalStructures.FLib;
using static FunctionalStructures.FLib.FLibHelper;

namespace FunctionalStructures;

public class UserRegistration
{
    public Option<string> Name { get; }

    public Email Email { get; }
    
    public bool IsActive { get; }
    
    public UserRegistration(Option<string> name, Email email, bool isActive = true)
    {
        Name = name;
        Email = email;
    }
    
    public override string ToString()
    {
        string PrintActive(bool isActive)
        {
            return isActive ? "Yes" : "No";
        }

        return $"Name: {PrintName()}, Email:{Email}, Active: {PrintActive(IsActive)}";
    }

    private string PrintName()
    {
        return Name.Match
        (
            someName => someName,
            () => "Unknown"
        );
    }
}

public static class UserRegistrationExtensions
{
    public static UserRegistration Deactivate(this UserRegistration registration) =>
        new(registration.Name, registration.Email, false);
}

public class UserService
{
    private readonly Dictionary<int, UserRegistration> _users = new()
    {
        { 1, new UserRegistration("Alice", new Email("alice@philips.com")) },
        { 2, new UserRegistration(Some("Bob"), new Email("bob@philips.com")) },
        { 3, new UserRegistration(None, new Email("kanenas@lavabit.com")) },
        // { 4, null }
    };

    // Function that sometimes returns null
    public Option<UserRegistration> GetUserProfile(int userId)
    {
        return _users.TryGetValue(userId, out var user) ? Some(user) : None;
    }
}

public static class NullAsReturnValue
{
    public static void Run()
    {
        var service = new UserService();
        Enumerable.Range(1, 4)
            .Map(service.GetUserProfile)
            .Map(profile => profile.Map(UserRegistrationExtensions.Deactivate))
            .Map(PrintRegistration)
            .ForEach(Console.WriteLine);
    }
    
    private static string PrintRegistration(Option<UserRegistration> registration)
    {
        return registration.Match(
            some: userRegistration => userRegistration.ToString(),
            none: () => "User not found"
        );
    }
}