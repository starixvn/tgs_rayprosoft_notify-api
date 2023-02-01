using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.Notification.HealthCheckModels
{
    internal record Category
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset OnStateFrom { get; set; }
        public DateTimeOffset LastExecuted { get; set; }
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public Entry[] Entries { get; set; }
    }

    internal record Entry
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? Uri { get; set; }
        public TimeSpan Duration { get; set; }
        public string[]? Tags { get; set; }
    }

}
