using LaYumba.Functional;
using static LaYumba.Functional.F;

namespace FunctionalStructures;

public class UserRegistration
{
    public Option<string> Name { get; }

    public Email Email { get; }
    
    public bool IsActive { get; }
    
    private UserRegistration(Option<string> name, Email email, bool isActive = true)
    {
        Name = name;
        Email = email;
    }

    public static UserRegistration Create(Option<string> name, Email email, bool isActive = true)
    {
        return new UserRegistration(name, email, isActive);
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
            () => "Unknown",
            someName => someName
        );
    }
}

public static class UserRegistrationExtensions
{
    public static UserRegistration Deactivate(this UserRegistration registration) =>
        UserRegistration.Create(registration.Name, registration.Email, false);
}

public class UserService
{
    private readonly Dictionary<int, UserRegistration> _users = new();

    public Option<Unit> RegisterUser(int id, Option<string> name, string email)
    {
        return Email.Create(email)
            .Map(e => UserRegistration.Create(name, e))
            .Map(u => _users.Add(id, u));
    }
    
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
        Func<UserRegistration, bool> isPhilipsWorker =
            registration => registration.Email.ToString().EndsWith("@philips.com");
        
        var service = new UserService();
        service.RegisterUser(1, "Alice", "alice@philips.com");
        service.RegisterUser(2, Some("Bob"), "bob@philips.com");
        service.RegisterUser(3, None, "kanenas@lavabit.com");
        
        Enumerable.Range(1, 4)
            .Bind(service.GetUserProfile)
            .Where(isPhilipsWorker)
            .Map(UserRegistrationExtensions.Deactivate)
            .ForEach(Console.WriteLine);
    }
    
    private static string PrintRegistration(Option<UserRegistration> registration)
    {
        return registration.Match(
            Some: userRegistration => userRegistration.ToString(),
            None: () => "User not found"
        );
    }
}