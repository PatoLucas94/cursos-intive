using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Spv.Usuarios.Api.Helpers;
using Spv.Usuarios.Common.Configurations;

namespace Spv.Usuarios.Test.Unit.Common.Builders
{
    public static class AllowedChannelsBuilder
    {
        public static AllowedChannels CrearAllowedChannels()
        {
            var loggerMock = new Mock<ILogger<AllowedChannels>>();

            var validChannelsConfiguration = new ValidChannelsConfigurationOptions
            {
                AllowedChannels = new List<string> { "HBI", "OBI", "BTA" }
            };

            var validChannelsConfigurationOptions = new Mock<IOptions<ValidChannelsConfigurationOptions>>();

            validChannelsConfigurationOptions.Setup(m => m.Value).Returns(validChannelsConfiguration);

            return new AllowedChannels(validChannelsConfigurationOptions.Object, loggerMock.Object);
        }

        public static AllowedChannels CrearEmptyAllowedChannels()
        {
            var loggerMock = new Mock<ILogger<AllowedChannels>>();
            var validChannelsConfiguration = new ValidChannelsConfigurationOptions
            {
                AllowedChannels = new List<string>()
            };

            var validChannelsConfigurationOptions = new Mock<IOptions<ValidChannelsConfigurationOptions>>();

            validChannelsConfigurationOptions.Setup(m => m.Value).Returns(validChannelsConfiguration);

            return new AllowedChannels(validChannelsConfigurationOptions.Object, loggerMock.Object);
        }
    }
}
