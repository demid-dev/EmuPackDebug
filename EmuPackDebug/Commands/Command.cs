using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    abstract class Command
    {
    }

    static class CommandValues
    {
        static public int IndexStartIndex { get; private set; }
        static public int IndexLength { get; private set; }

        static CommandValues()
        {
            IndexStartIndex = 0;
            IndexLength = 2;
        }
    }

    abstract class CommandResponse
    {
        public string CommandId { get; protected set; }
        public string SendFrom { get; private set; }
        public string SendTo { get; private set; }
        public string DataLength { get; protected set; }
        public string ResponseCode { get; protected set; }
        public string Response
        {
            get
            {
                return CommandId + SendFrom + SendTo + DataLength + ResponseCode;
            }
        }

        public CommandResponse()
        {
            SendFrom = CommandResponseValues.SendFrom;
            SendTo = CommandResponseValues.SendTo;
        }
    }

    static class CommandResponseValues
    {
        static public string SendFrom { get; set; }
        static public string SendTo { get; set; }

        static CommandResponseValues()
        {
            SendFrom = "M1";
            SendTo = "C1";
        }
    }
}
