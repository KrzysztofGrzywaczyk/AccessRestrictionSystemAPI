using AccessControlSystem.SharedKernel.ApiModels.Paging;
using FluentAssertions;
using System.Text;

namespace AccessControlSystem.SharedKernel.UnitTests.ApiModels.Paging.CursorValueTests
{
    public class ConvertFromTests
    {
        [Fact]
        public void WhenCursorHasDefaultValue_ShouldReturnEmptyString()
        {
            // act
            var result = CursorValue.ConvertFrom<long?>(null);

            // assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void WhenCursorDoesNotHaveDefaultValue_ShouldReturnBase64EncodedString()
        {
            // arrange
            long cursor = 15;
            var base64Bytes = Encoding.UTF8.GetBytes(cursor.ToString());

            // act
            var result = CursorValue.ConvertFrom<long?>(cursor);

            // assert
            result.Should().Be(Convert.ToBase64String(base64Bytes));
        }
    }
}