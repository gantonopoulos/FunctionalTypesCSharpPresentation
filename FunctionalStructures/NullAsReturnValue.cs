using LaYumba.Functional;
using static LaYumba.Functional.F;

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

    public static Option<UserRegistration> Create(Option<string> name, Email email, bool isActive = true)
    {
        return Some(new UserRegistration(name, email, isActive));
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
        new(registration.Name, registration.Email, false);
}

public class UserService
{
    private readonly Dictionary<int, UserRegistration> _users;

    public UserService()
    {
        _users = new Dictionary<int, UserRegistration>();
        Email.Create("alice@philips.com")
            .Bind(r => UserRegistration.Create("Alice", r))
            .Map(alice => _users.Add(1, alice));
        Email.Create("bob@philips.com")
            .Bind(r => UserRegistration.Create(Some("Bob"), r))
            .Map(bob => _users.Add(2, bob));
        Email.Create("kanenas@lavabit.com")
            .Bind(r => UserRegistration.Create(None, r))
            .Map(ulysses => _users.Add(3, ulysses));
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