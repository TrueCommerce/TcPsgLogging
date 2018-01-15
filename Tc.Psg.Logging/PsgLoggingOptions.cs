namespace Tc.Psg.Logging
{
    /// <summary>
    /// Options used to configure PSG Logging.
    /// </summary>
    public class PsgLoggingOptions
    {
        public PsgLoggingOptions()
        {
            SeqServerUrl = "http://seq.tcpsg.net:80";
        }

        /// <summary>
        /// The API Key to use when sending events to the Seq log server.
        /// </summary>
        public string SeqApiKey { get; set; }

        /// <summary>
        /// The URL of the central Seq log server.
        /// </summary>
        public string SeqServerUrl { get; set; }
    }
}
