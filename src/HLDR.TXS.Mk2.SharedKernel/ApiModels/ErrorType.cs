
namespace AccessControlSystem.SharedKernel.ApiModels;

public static class ErrorType
{
    public static string InternalServerError => nameof(InternalServerError);

    public static string ValidationError => nameof(ValidationError);

    public static string ConflictError => nameof(ConflictError);

    public static string ResourceNotFound => nameof(ResourceNotFound);

    public static string AuthorizationError => nameof(AuthorizationError);

    public static string IotcError => nameof(IotcError);
}