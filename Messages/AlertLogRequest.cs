using System;

namespace TGS.NotifyApi.Messages
{
    public class AlertLogRequest
    {
        public string Type { get; set; }
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string Prefix { get; set; }
        public string Exception { get; set; }
        public string Stacktrace { get; set; }
        public string ServerName { get; set; }
        public int ResponseCode { get; set; }
        public DateTimeOffset RecordedOn { get; set; }
    }
}
