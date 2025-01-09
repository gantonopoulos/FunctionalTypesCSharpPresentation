using FunctionalStructures.FLib;
using static FunctionalStructures.FLib.FLibHelper;

namespace FunctionalStructures;

public class UserRegistration
{
    public Option<string> Name { get; }

    public Email Email { get; }
    
    public UserRegistration(Option<string> name, Email email)
    {
        Name = name;
        Email = email;
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
        
        var profile1 = service.GetUserProfile(1);
        profile1.Match(
            some: registration => Console.WriteLine(registration.ToString()),
            none: () => Console.WriteLine("User not found")
        );

        var profile3 = service.GetUserProfile(3);
        Console.WriteLine(PrintRegistration(profile3));
        
        var profile4 = service.GetUserProfile(4);
        Console.WriteLine(PrintRegistration(profile4));
        
    }

    private static string PrintRegistration(Option<UserRegistration> registration)
    {
        return registration.Match(
            some: userRegistration => userRegistration.ToString(),
            none: () => "User not found"
        );
    }
}