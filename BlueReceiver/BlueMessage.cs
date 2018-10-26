using System;
using System.Collections.Generic;
using System.Text;

namespace BlueReceiver
{
    public class BlueMessage
    {
        public string DisplayName { get; set; }
        public string Job { get; set; }
        public string MaxTries { get; set; }
        public string Timeout { get; set; }
        public CommandDef Data { get; set; }
    }

    public class CommandDef
    {
        public string CommandName { get; set; }
        public string Command { get; set; }
    }
}
