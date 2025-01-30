using LanguageExt;
using static LanguageExt.Prelude;

namespace FunctionalStructures;

public static class Helper
{
    public static bool IsRandomlyTrue()
    {
        return Random.Shared.Next(2) == 0;
    }
    
    public static bool IsRandomlyTrue(int truthPercentage)
    {
        if (truthPercentage is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(truthPercentage), "Percentage must be between 0 and 100.");
        return Random.Shared.Next(100) < truthPercentage;
    }
    
    public static Unit PrintRegistrationResult(this Either<string, UserRegistration> @this)
    {
        Console.WriteLine(@this.Match(
            registration => $"User {registration} was registered successfully!",
            errorMessage => errorMessage
        ));
        return unit;
    }
}