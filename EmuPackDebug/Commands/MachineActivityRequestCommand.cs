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
        public string ClearingWarningsInitiated { get; private set; }

        public MachineActivityRequestCommand(string commandString) : base(commandString)
        {
            DataStartNotification = ReadCommandField(commandString,
                MachineActivityRequestValues.DataStartNotificationStartIndex,
                MachineActivityRequestValues.DataStartNotificationLength);
            DrawerStatus = ReadCommandField(commandString,
                MachineActivityRequestValues.DrawerStatusStartIndex,
                MachineActivityRequestValues.DrawerStatusLength);
            ClearingWarningsInitiated = ReadCommandField(commandString,
                MachineActivityRequestValues.ClearingWarningsInitiatedStartIndex,
                MachineActivityRequestValues.ClearingWarningsInitiatedLength);

            string command = CommandId + SendFrom + SendTo + DataLength
            + DataStartNotification + DrawerStatus + ClearingWarningsInitiated;
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
            if (!MachineActivityRequestValues.ClearingWarningsInitiatedPossibleValues
                .Contains(ClearingWarningsInitiated))
                return false;

            return true;
        }

        public override CommandResponse Execute(MachineState machineState)
        {
            if (!IsCommandValid)
                return new MachineActivityRequestCommandResponse(CommandResponseCodes.WrongCommandFormat);
            if(DrawerStatus == MachineActivityRequestValues.DrawerStatusPossibleValues[0])
            {
                machineState.ChangeDrawerStatus(false);
            }
            else
            {
                machineState.ChangeDrawerStatus(true);
            }
            if (DrawerStatus == MachineActivityRequestValues.ClearingWarningsInitiatedPossibleValues[1])
            {
                machineState.ClearWarningCassettesIds();
            }

            return new MachineActivityRequestCommandResponse(CommandResponseCodes.Sucess);
        }
    }

    static class MachineActivityRequestValues
    {
        static public string CommandId { get; private set; }
        static public string DataStartNotification { get; private set; }
        static public int DataStartNotificationStartIndex { get; private set; }
        static public int DataStartNotificationLength { get; private set; }
        static public string[] DrawerStatusPossibleValues { get; private set; }
        static public int DrawerStatusStartIndex { get; private set; }
        static public int DrawerStatusLength { get; private set; }
        static public string[] ClearingWarningsInitiatedPossibleValues { get; private set; }
        static public int ClearingWarningsInitiatedStartIndex { get; private set; }
        static public int ClearingWarningsInitiatedLength { get; private set; }

        static MachineActivityRequestValues()
        {
            CommandId = "MR";
            DataStartNotification = "D";
            DataStartNotificationLength = 1;
            DataStartNotificationStartIndex = 11;
            DrawerStatusPossibleValues = new string[] { "00", "01" };
            DrawerStatusStartIndex = 12;
            DrawerStatusLength = 2;
            ClearingWarningsInitiatedPossibleValues = new string[] { "00", "01" };
            ClearingWarningsInitiatedStartIndex = 14;
            ClearingWarningsInitiatedLength = 2;
        }
    }

    class MachineActivityRequestCommandResponse : CommandResponse
    {
        public MachineActivityRequestCommandResponse(CommandResponseCodes code) : base(code)
        {
            CommandId = MachineActivityRequestValues.CommandId;
            DataLength = "00002";
        }
    }
}
