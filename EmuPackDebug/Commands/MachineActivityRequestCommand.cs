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
        public string DrawerStatus { get; private set; }

        public MachineActivityRequestCommand(string commandString) : base(commandString)
        {
            DataStartNotification = ReadCommandField(commandString,
                MachineActivityRequestValues.DataStartNotificationStartIndex,
                MachineActivityRequestValues.DataStartNotificationLength);
            DrawerStatus = ReadCommandField(commandString,
                MachineActivityRequestValues.DrawerStatusStartIndex,
                MachineActivityRequestValues.DrawerStatusLength);

            string command = CommandId + SendFrom + SendTo + DataLength
            + DataStartNotification + DrawerStatus;
            Console.WriteLine(command);
            IsCommandValid = ValidateCommand(commandString);
            Console.WriteLine("Is command valid: " + IsCommandValid);
        }

        public override bool ValidateCommand(string commandString)
        {
            if (!base.ValidateCommand(commandString))
                return false;
            if (DataStartNotification != MachineActivityRequestValues.DataStartNotification)
                return false;
            if (!MachineActivityRequestValues.DrawerStatusPossibleValues.Contains(DrawerStatus))
                return false;

            return true;
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
