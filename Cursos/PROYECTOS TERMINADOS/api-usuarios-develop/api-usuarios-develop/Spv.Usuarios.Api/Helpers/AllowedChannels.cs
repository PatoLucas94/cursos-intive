using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spv.Usuarios.Common.Configurations;

namespace Spv.Usuarios.Api.Helpers
{
    /// <summary>
    /// AllowedChannels
    /// </summary>
    public class AllowedChannels : IAllowedChannels
    {
        private readonly IOptions<ValidChannelsConfigurationOptions> _validChannelsConfigurationOptions;
        private readonly ILogger<AllowedChannels> _logger;

        private IList<string> _allowedChannels;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validChannelsConfigurationOptions"></param>
        /// <param name="logger"></param>
        public AllowedChannels(
            IOptions<ValidChannelsConfigurationOptions> validChannelsConfigurationOptions,
            ILogger<AllowedChannels> logger)
        {
            _validChannelsConfigurationOptions = validChannelsConfigurationOptions;
            _logger = logger;
        }

        /// <summary>
        /// Método que retorna 'true' si el Header X-Canal es valido
        /// </summary>
        /// <param name="pChannel">X-Canal header</param>
        /// <returns></returns>
        public bool IsValidChannel(string pChannel)
        {
            _allowedChannels ??= _validChannelsConfigurationOptions.Value.AllowedChannels ?? new List<string>();

            _logger.LogDebug($"allowedChannels: {string.Join(",", _allowedChannels)}");

            return _allowedChannels.Count == 0 ||
                   _allowedChannels.Any(x => x.ToLower().Equals(pChannel.ToLower(), StringComparison.InvariantCulture));
        }
    }
}
