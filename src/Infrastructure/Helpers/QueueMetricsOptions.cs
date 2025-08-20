using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public sealed class QueueMetricsOptions
    {
        public string BaseUrl { get; set; } = "";
        public string VHost { get; set; } = "/";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public List<string> Queues { get; set; } = new();
        public int PollSeconds { get; set; } = 5;
        public string OutputDirectory { get; set; } = "queue-metrics";

        public bool NewFilePerRun { get; set; } = true;           
        public bool SkipIdle { get; set; } = true;                 
        public bool OnlyWhenChanged { get; set; } = true;           
        public int MinWriteIntervalSeconds { get; set; } = 0;       
    }

}
