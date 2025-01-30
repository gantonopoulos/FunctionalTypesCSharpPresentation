using LanguageExt.Common;

namespace FunctionalStructures.ErrorDefinitions;

public record EmailCannotBeEmpty : Error
{
    public override bool Is<E>() => false;

    public override ErrorException ToErrorException() => throw new NotImplementedException();

    public override string Message => "Email cannot be empty!";
    public override bool IsExceptional => false; 
    public override bool IsExpected => true;
    public static EmailCannotBeEmpty Create() => new();
}

public record InvalidEmailFormat(string Email): Error
{
    public override bool Is<E>() => false;

    public override ErrorException ToErrorException() => throw new NotImplementedException();

    public override string Message => $"Invalid Email format! {Email}";
    public override bool IsExceptional => false; 
    public override bool IsExpected => true;
    public static InvalidEmailFormat Create(string email) => new(email);
}

public record EmailDoesNotExist(string Email) : Error
{
    public override bool Is<E>() => false;

    public override ErrorException ToErrorException() => throw new NotImplementedException();

    public override string Message => $"The provided Email:{Email} could not be verified.";
    public override bool IsExceptional => false; 
    public override bool IsExpected => true;
    public static EmailDoesNotExist Create(string email) => new(email);
}

public record EmailVerificationTimeout : Error
{
    public override bool Is<E>() => false;

    public override ErrorException ToErrorException() => throw new NotImplementedException();

    public override string Message => "Timeout while trying to request email verification.";
    public override bool IsExceptional => false; 
    public override bool IsExpected => true;
    public static EmailVerificationTimeout Create() => new();
}


public record RegistrationExists(string Email) : Error
{
    public override bool Is<E>() => false;

    public override ErrorException ToErrorException() => throw new NotImplementedException();

    public override string Message => $"There exists already a registration with Email: {Email}.";
    public override bool IsExceptional => false; 
    public override bool IsExpected => true;
    public static RegistrationExists Create(string email) => new(email);
}