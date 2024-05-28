
namespace AccessControlSystem.Api.Models.DataTransferObjects;

public class RemovedAndWithAccessesAccessCards
{
    public AccessCardValueList? RemovedAccessCardValues { get; set; }

    public AccessCardValueList? AccessCardValuesWithAccess { get; set; }
}
