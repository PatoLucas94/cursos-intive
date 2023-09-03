namespace Spv.Usuarios.Bff.Common.ExternalResponses
{
    public class ApiNotificacionesErrorResponse
    {
        /// <summary>
        /// ApiNotificacionesErrorResponse
        /// </summary>
        public ApiNotificacionesErrorResponse()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ApiNotificacionesErrorResponse(string timestamp, int status, string error, string message, string path)
        {
            this.timestamp = timestamp;
            this.status = status;
            this.error = error;
            this.message = message;
            this.path = path;
        }

        /// <summary>
        /// Error Timestamp
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// Http Status Code
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// Error Title
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public string path { get; set; }
    }
}
