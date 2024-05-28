using AccessControlSystem.SharedKernel.ApiModels;
using AccessControlSystem.SharedKernel.JsonSerialization;
using FluentValidation;
using MediatR;

namespace AccessControlSystem.SharedKernel.UnitTests.Behaviors;

#pragma warning disable SA1649
#pragma warning disable SA1402
public class DummyRequest : IRequest<DummyResponse>
{
    public int Count { get; set; }

    [SensitiveData]
    public string Password { get; set; } = string.Empty;
}

public class DummyResponse : ServiceResultBase
{
}

public class DummyValidator : AbstractValidator<DummyRequest>
{
    public DummyValidator()
    {
        RuleFor(x => x.Count)
            .Equal(0);
    }
}
#pragma warning restore SA1402
#pragma warning restore SA1649
