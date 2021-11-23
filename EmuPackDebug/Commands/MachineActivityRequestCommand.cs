using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class MachineActivityRequestCommand : Command
    {
        public string DataStartNotification { get; private set; }

        public MachineActivityRequestCommand(string commandString) : base(commandString)
        {

        }

        public override CommandExecutionObject Execute(MachineState machineState)
        {
            throw new NotImplementedException();
        }
    }

    static class MachineActivityRequestValues
    {
        static public string DataStartNotification { get; private set; }
        static public int DataStartNotificationStartIndex { get; private set; }
        static public int DataStartNotificationLength { get; private set; }
        static public string[] DrawerStatusPossibleValues { get; private set; }
        static public int DrawerStatusStartIndex { get; private set; }
        static public int DrawerStatusLength { get; private set; }

        static MachineActivityRequestValues()
        {
            DataStartNotification = "D";
            DataStartNotificationLength = 1;
            DataStartNotificationStartIndex = 11;
            DrawerStatusPossibleValues = new string[] { "00", "01" };
            DrawerStatusStartIndex = 12;
            DrawerStatusLength = 2;
        }
    }
}
