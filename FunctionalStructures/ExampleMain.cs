using LanguageExt;
using static LanguageExt.Prelude;

namespace FunctionalStructures;

public static class ExampleMain
{
    public static void Run()
    {
        var service = new Registrations();
        
        List<(Option<string> Name, string Email)> usersToRegister =
        [
            ("Alice", "alice@philips.com"),
            (Some("Bob"), "bob@philips.com"),
            (None, "kanenas@lavabit.com")
        ];

        _ = Right<string, (Option<string> Name, string Email)>(usersToRegister.First())
            .Bind(entry => service.RegisterUser(entry.Name, entry.Email))
            .PrintRegistrationResult();

        // usersToRegister
        //     .Map(entry => service.RegisterUser(entry.Name, entry.Email))
        //     .Select(result =>
        //         result.Match<string>(
        //             registration => $"User {registration} was registered successfully!",
        //             errorMessage => errorMessage))
        //     .ForEach(Console.WriteLine);
    }

    
}