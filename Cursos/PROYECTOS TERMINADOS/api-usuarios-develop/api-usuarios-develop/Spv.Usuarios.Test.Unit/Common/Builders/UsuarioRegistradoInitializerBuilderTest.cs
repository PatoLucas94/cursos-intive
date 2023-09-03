using System;
using FluentAssertions;
using Spv.Usuarios.Test.Infrastructure.Builders;
using Xunit;

namespace Spv.Usuarios.Test.Unit.Common.Builders
{
    public class UsuarioRegistradoInitializerBuilderTest : UsuarioRegistradoInitializerBuilder
    {
        public UsuarioRegistradoInitializerBuilderTest() : base()
        {

        }

        string documentNumberTestValue = "12345678";
        bool activeTestValue = true;
        string cardNumberTestValue = "lksdjfgao9348y";
        DateTime expiredDateTimeTestValue = DateTime.MinValue;
        int forgotPasswordAttemptsTestValue = 1;
        string channelKey = "Channel";
        int DocumentTypeIdTestValue = 80;

        [Fact]
        public void WithoutDefaultValuesTest()
        {
            var usuarioRegistrado = GetUsuarioRegistradoWithoutDefaultValues(
                documentNumberTestValue,
                activeTestValue,
                cardNumberTestValue,
                expiredDateTimeTestValue,
                forgotPasswordAttemptsTestValue,
                channelKey);

            usuarioRegistrado.DocumentNumber.Should().Be(documentNumberTestValue);
            usuarioRegistrado.Active.Should().Be(activeTestValue);
            usuarioRegistrado.CardNumber.Should().Be(cardNumberTestValue);
            usuarioRegistrado.ExpiredDateTime.Should().Be(expiredDateTimeTestValue);
            usuarioRegistrado.ForgotPasswordAttempts.Should().Be(forgotPasswordAttemptsTestValue);
            usuarioRegistrado.ChannelKey.Should().Be(channelKey);
            usuarioRegistrado.DocumentTypeId.Should().Be(default(int));
        }

        [Fact]
        public void WithDefaultValuesTest()
        {
            var usuarioRegistrado = GetUsuarioRegistradoWithDefaultValues(
                documentNumberTestValue,
                activeTestValue,
                cardNumberTestValue,
                expiredDateTimeTestValue,
                forgotPasswordAttemptsTestValue,
                channelKey);

            usuarioRegistrado.DocumentNumber.Should().Be(documentNumberTestValue);
            usuarioRegistrado.Active.Should().Be(activeTestValue);
            usuarioRegistrado.CardNumber.Should().Be(cardNumberTestValue);
            usuarioRegistrado.ExpiredDateTime.Should().Be(expiredDateTimeTestValue);
            usuarioRegistrado.ForgotPasswordAttempts.Should().Be(forgotPasswordAttemptsTestValue);
            usuarioRegistrado.ChannelKey.Should().Be(channelKey);
            usuarioRegistrado.DocumentTypeId.Should().Be(DocumentTypeIdTestValue);
        }
    }
}
