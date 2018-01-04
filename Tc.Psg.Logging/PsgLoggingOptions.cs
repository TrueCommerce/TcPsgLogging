namespace Tc.Psg.Logging
{
    public class PsgLoggingOptions
    {
        public PsgLoggingOptions()
        {
            SeqServerUrl = "http://seq.tcpsg.net:80";
        }

        public string SeqApiKey { get; set; }
        public string SeqServerUrl { get; set; }
    }
}
