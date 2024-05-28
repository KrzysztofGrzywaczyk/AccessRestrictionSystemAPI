
namespace AccessControlSystem.SharedKernel.ApiModels.Paging;

public interface ICursor
{
    ICursor FromString(string value);
}