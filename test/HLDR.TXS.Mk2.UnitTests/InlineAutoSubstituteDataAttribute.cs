using AutoFixture.Xunit2;

namespace AccessControlSystem.UnitTests;

/// <summary>
/// Used to create parameterized repeatable tests using autodata and inline data. Note that the inline data must be the first parameters of the test method.
/// </summary>
public class InlineAutoSubstituteDataAttribute(params object[] objects) : InlineAutoDataAttribute(new AutoSubstituteDataAttribute(), objects)
{
}