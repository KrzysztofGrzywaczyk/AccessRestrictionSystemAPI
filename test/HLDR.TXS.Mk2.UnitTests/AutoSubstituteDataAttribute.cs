using AutoFixture;
using AutoFixture.Xunit2;

namespace AccessControlSystem.UnitTests;

public class AutoSubstituteDataAttribute() : AutoDataAttribute(() => new Fixture().Customize(new AutoFixture.AutoNSubstitute.AutoNSubstituteCustomization { ConfigureMembers = true }))
{
}