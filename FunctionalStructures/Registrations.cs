
using LanguageExt;
using static LanguageExt.Prelude;

namespace FunctionalStructures;

public class Registrations
{
    private readonly System.Collections.Generic.HashSet<UserRegistration> _users = [];

    public Either<string, UserRegistration> RegisterUser(Option<string> name, string email)
    {
        return Email.Create(email)
            .Map(e => UserRegistration.Create(name, e))
            .Bind(VerifyEmailIsReal)
            .Bind(Save);
    }

    public Option<UserRegistration> GetUserRegistration(Email email)
    {
        UserRegistration? existing = _users.SingleOrDefault(registration => registration.Email.Equals(email));
        return existing is null
            ? None
            : Some(existing);
    }

    private Either<string, UserRegistration> VerifyEmailIsReal(UserRegistration toCheck)
    {
        try
        {
            return EmailLocator.IsRealEmail(toCheck.Email)
                ? toCheck
                : $"The provided Email:{toCheck.Email} could not be verified.";
        }
        catch (HttpRequestException)
        {
            return $"Error while trying to request email verification for: {toCheck}";
        }
        catch (TimeoutException)
        {
            return $"Timeout while trying to request email verification for: {toCheck}";
        }
        catch (Exception e)
        {
            return $"An unexpected error occurred while trying to request email " +
                   $"verification for: {toCheck}. Error: {e.Message}";
        }
    }

    private Either<string, UserRegistration> Save(UserRegistration registration)
    {
        return _users.Add(registration)
            ? registration
            : $"There exists already a registration with Email: {registration.Email}.";
    }
}