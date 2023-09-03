using FluentAssertions;
using Spv.Usuarios.Bff.ViewModels.CommonController.Input;
using Xunit;

namespace Spv.Usuarios.Bff.Test.Unit.Common.Headers
{
    public class ApiHeadersTest
    {
        private static readonly ApiHeaders HeadersInvalidChannel = new ApiHeaders
        {
            
        };

        [Fact]
        public void XRequestIdHeaderIsOptional()
        {
            //Arrange

            //Act
            var result = HeadersInvalidChannel.ToRequestBody("test");

            //Assert
            result.XRequestId.Should().NotBeNullOrEmpty();
            result.Body.Should().Be("test");
        }
    }
}
