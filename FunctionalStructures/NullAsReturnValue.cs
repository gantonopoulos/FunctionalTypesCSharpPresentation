namespace FunctionalStructures;

public class UserRegistration
{
    public string? Name { get; }

    public string Email { get; }
    
    public UserRegistration(string? name, string email)
    {
        Name = name;
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
    
    public override string ToString()
    {
        return $"Name: {PrintName()}, Email:{Email}";
    }

    private string PrintName() => $"{Name ?? "Unknown"}";
}

public class UserService
{
    private readonly Dictionary<int, UserRegistration> _users = new()
    {
        { 1, new UserRegistration("Alice", "alice@philips.com") },
        { 2, new UserRegistration("Bob", "bob@philips.com") },
        { 3, new UserRegistration(null, "kanenas@philips.com") },
        // { 4, null }
    };

    // Function that sometimes returns null
    public UserRegistration? GetUserProfile(int userId)
    {
        return _users.TryGetValue(userId, out var user) ? user : null;
    }
}

public static class NullAsReturnValue
{
    public static void Run()
    {
        var service = new UserService();

        var profile1 = service.GetUserProfile(1);
        Console.WriteLine(profile1 != null ? profile1.ToString() : "User not found");

        var profile2 = service.GetUserProfile(3);
        Console.WriteLine(profile2 != null ? profile2.ToString() : "User not found");
        
        var profile3 = service.GetUserProfile(4);
        Console.WriteLine(profile3 != null ? profile3.ToString() : "User not found");
        
    }
}