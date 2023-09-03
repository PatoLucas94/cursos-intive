using System;

namespace Spv.Usuarios.Bff.Common.Dtos.Client.GoogleClient.Output
{
    public class ApiGoogleValidarTokenCaptchaV3ModelOutput
    {
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public string hostname { get; set; }
        public DateTime challenge_ts { get; set; }
    }
}
