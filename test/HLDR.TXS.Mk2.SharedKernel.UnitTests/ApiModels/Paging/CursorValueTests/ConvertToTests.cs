using AccessControlSystem.SharedKernel.ApiModels.Paging;
using FluentAssertions;
using System.Text;

namespace AccessControlSystem.SharedKernel.UnitTests.ApiModels.Paging.CursorValueTests
{
    public class ConvertToTests
    {
        [Fact]
        public void WhenValueIsOfNonNullableType_ShouldThrowInvalidOperationException()
        {
            // act
            Action action = () => CursorValue.ConvertTo<long>(string.Empty);
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void WhenValueIsOfClassButNotImplementingICursorInterface_ShouldThrowInvalidOperationException()
        {
            // act
            Action action = () => CursorValue.ConvertTo<object>(string.Empty);
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void WhenValueIsOfClassAndImplementingICursorInterface_ShouldReturnCursor()
        {
            // arrange
            var originalValue = "15";
            var value = Convert.ToBase64String(Encoding.UTF8.GetBytes(originalValue));

            // act
            var cursor = CursorValue.ConvertTo<Cursor>(value);

            // assert
            cursor.Should().BeOfType<Cursor>();
        }

        [Fact]
        public void WhenValueIsEmpty_ShouldReturnDefaultCursor()
        {
            // act
            var result = CursorValue.ConvertTo<long?>(string.Empty);

            // assert
            result.Should().Be(default(long?));
        }

        [Fact]
        public void WhenValueIsNotEmpty_ShouldReturnCursorValue()
        {
            // arrange
            var originalValue = "15";
            var value = Convert.ToBase64String(Encoding.UTF8.GetBytes(originalValue));

            // act
            var result = CursorValue.ConvertTo<long?>(value);

            // assert
            result.Should().Be(long.Parse(originalValue));
        }

        [Fact]
        public void WhenValueIsNotBase64Encoded_ShouldReturnDefaultValue()
        {
            // arrange
            var originalValue = "15";

            // act
            var result = CursorValue.ConvertTo<long?>(originalValue);

            // assert
            result.Should().Be(default(long?));
        }

        private class Cursor : ICursor
        {
            public ICursor FromString(string value)
            {
                return new Cursor();
            }
        }
    }
}