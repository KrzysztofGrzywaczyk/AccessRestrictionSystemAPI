using AccessControlSystem.Api.Entities;
using AccessControlSystem.Api.Validation.Accesses;
using AccessControlSystem.UnitTests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessControlSystem.Api.UnitTests.Validation;

public class PutAccessesRequestValidatorTestsContext : UnitTestsContextBase
{
    private readonly SqlDbContext _dbContext = SqlDbContextFactory.Create();

    public PutAccessesRequestValidator Validator { get; } = new PutAccessesRequestValidator();

    public void Dispose()
    {
        // TODO release managed resources here
    }

    public Task WithExistingAccessCards(IEnumerable<string> requestAccessCardValues)
    {
        foreach (var requestAccessCardValue in requestAccessCardValues)
        {
            _dbContext.AccessCards.Add(new AccessCard()
            {
                Value = requestAccessCardValue
            });
        }

        return _dbContext.SaveChangesAsync();
    }
}
