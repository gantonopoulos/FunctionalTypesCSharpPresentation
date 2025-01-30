using FunctionalStructures.ErrorDefinitions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace FunctionalStructures;

public class Registrations
{
    private readonly System.Collections.Generic.HashSet<UserRegistration> _users = [];

    public Either<Error, UserRegistration> RegisterUser(Option<string> name, string email)
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

    private Either<Error, UserRegistration> VerifyEmailIsReal(UserRegistration toCheck)
    {
        try
        {
            return EmailLocator.IsRealEmail(toCheck.Email)
                ? toCheck
                : EmailDoesNotExist.Create(toCheck.Email);
        }
        catch (HttpRequestException e)
        {
            return Error.New($"Error while trying to request email verification for: {toCheck}", e as Exception);
        }
        catch (TimeoutException e)
        {
            return Error.New($"Timeout while trying to request email verification for: {toCheck}", e as Exception);
        }
        catch (Exception e)
        {
            return Error.New(
                $"An unexpected error occurred while trying to request email verification for: {toCheck}",
                e);
        }
    }

    private Either<Error, UserRegistration> Save(UserRegistration registration)
    {
        return _users.Add(registration)
            ? registration
            : RegistrationExists.Create(registration.Email);
    }
}