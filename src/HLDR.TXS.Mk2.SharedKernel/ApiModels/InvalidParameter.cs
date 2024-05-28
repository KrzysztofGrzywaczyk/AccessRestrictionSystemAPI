
namespace AccessControlSystem.SharedKernel.ApiModels;

public class InvalidParameter(string name, string reason)
{
    public string Name { get; } = name;

    public string Reason { get; } = reason;
}