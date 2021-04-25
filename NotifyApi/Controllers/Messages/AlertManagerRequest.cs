using System;

namespace TGS.NotifyApi.Controllers.Messages
{
    public class AlertManagerRequest
    {
        public string Receiver { get; set; }
        public string Status { get; set; }
        public string ExternalURL { get; set; }
        public _Alert[] Alerts { get; set; }

        public _CommonLabel CommonLabels { get; set; }
        public _Annotation CommonAnnotations { get; set; }

        public class _Alert
        {
            public string Status { get; set; }
            public _Annotation Annotations { get; set; }
            public _CommonLabel Labels { get; set; }
            public DateTimeOffset StartsAt { get; set; }
            public DateTimeOffset EndsAt { get; set; }
            public string GeneratorURL { get; set; }
            public string Fingerprint { get; set; }
        }

        public class _Annotation
        {
            public string Description { get; set; }
            public string Summary { get; set; }
        }

        public class _CommonLabel
        {
            public string Alertname { get; set; }
            public string Instance { get; set; }
            public string Severity { get; set; }
        }
    }
}
