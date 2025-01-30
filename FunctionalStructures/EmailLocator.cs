using static FunctionalStructures.Helper;

namespace FunctionalStructures;

public static class EmailLocator
{
    public static bool IsRealEmail(Email toCheck)
    {
        if (IsRandomlyTrue(10))
        {
            throw new TimeoutException("Timeout while trying to request email verification.");
        }

        if (IsRandomlyTrue(10))
        {
            throw new HttpRequestException("Error while trying to request email verification.");
        }
        
        return IsRandomlyTrue();
    }
}