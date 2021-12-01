using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    abstract class Command
    {
        public string CommandId { get; protected set; }
        public string SendFrom { get; protected set; }
        public string SendTo { get; protected set; }
        public string DataLength { get; protected set; }
        public bool IsCommandValid { get; protected set; }

        public Command(string commandString)
        {
            CommandId = ReadCommandField(commandString,
                CommandValues.IndexStartIndex,
                CommandValues.IndexLength);
            SendFrom = ReadCommandField(commandString,
                CommandValues.SendFromStartIndex,
                CommandValues.SendFromLength);
            SendTo = ReadCommandField(commandString,
                CommandValues.SendToStartIndex,
                CommandValues.SendToLength);
            DataLength = ReadCommandField(commandString,
                CommandValues.DataLengthStartIndex,
                CommandValues.DataLengthLength);
        }

        public virtual string ReadCommandField(string commandString, int startIndex, int length)
        {
            return string.Join("", commandString.Skip(startIndex).Take(length));
        }

        public virtual bool ValidateCommand(string commandString)
        {
            if (SendFrom != CommandValues.SendFrom)
                return false;
            if (SendTo != CommandValues.SendTo)
                return false;
            if (!ValidateDataLength(commandString))
                return false;

            return true;
        }
        public abstract CommandResponse Execute(MachineState machineState);

        private bool ValidateDataLength(string commandString)
        {
            if (DataLength.Length != CommandValues.DataLengthLength) return false;

            string dataLength = GetNumberWithoutPadding(DataLength);

            bool dataLengthParsed = int.TryParse(dataLength, out int dataLengthNumber);

            if (!dataLengthParsed) return false;

            return (commandString.Length - CommandValues.NoDataLength) == dataLengthNumber;
        }

        protected virtual string GetNumberWithoutPadding(string number)
        {
            while (number.Length > 1 && number[0] == '0')
            {
                number = number.Remove(0, 1);
            }
            return number;
        }

        protected virtual bool ValidateNumericalField(string field, int fieldLength, int minValue, int maxValue)
        {
            if (!ValidateFieldByLength(field, fieldLength))
                return false;

            bool fieldParsed = int.TryParse(GetNumberWithoutPadding(field),
                out int numericalField);

            if (!fieldParsed
                || numericalField < minValue
                || numericalField > maxValue)
                return false;

            return true;
        }

        protected virtual bool ValidateFieldByLength(string field, int length)
        {
            return field.Length == length;
        }

        protected virtual bool ValidateCommandByMachine(MachineState machineState) {
            return true;
        }
    }

    static class CommandValues
    {
        static public int IndexStartIndex { get; private set; }
        static public int IndexLength { get; private set; }
        static public int SendFromStartIndex { get; private set; }
        static public int SendFromLength { get; private set; }
        static public string SendFrom { get; private set; }
        static public int SendToStartIndex { get; private set; }
        static public int SendToLength { get; private set; }
        static public string SendTo { get; private set; }
        static public int DataLengthStartIndex { get; private set; }
        static public int DataLengthLength { get; private set; }
        static public int NoDataLength { get; private set; }

        static CommandValues()
        {
            IndexStartIndex = 0;
            IndexLength = 2;
            SendFromStartIndex = 2;
            SendFromLength = 2;
            SendFrom = "C1";
            SendToStartIndex = 4;
            SendToLength = 2;
            SendTo = "M1";
            DataLengthStartIndex = 6;
            DataLengthLength = 5;
            NoDataLength = 11;
        }
    }

    enum CommandResponseCodes
    {
        Sucess,
        WrongCommandFormat,
        MachineBlockedCommand
    }

    abstract class CommandResponse
    {
        private string _response;

        public string CommandId { get; protected set; }
        public string SendFrom { get; private set; }
        public string SendTo { get; private set; }
        public string DataLength { get; protected set; }
        public string ResponseCode { get; protected set; }
        public string Response
        {
            get
            {
                if(_response == null)
                {
                    return CommandId + SendFrom + SendTo + DataLength + ResponseCode;
                }
                else
                {
                    return _response;
                }
            }
            protected set
            {
                _response = value;
            }
        }

        public CommandResponse(CommandResponseCodes commandResponseCode)
        {
            SendFrom = CommandResponseValues.SendFrom;
            SendTo = CommandResponseValues.SendTo;
            CommandResponseValues.ResponseCodes.TryGetValue(commandResponseCode, out string code);
            ResponseCode = code;
        }
    }

    static class CommandResponseValues
    {
        static public string SendFrom { get; private set; }
        static public string SendTo { get; private set; }
        static public Dictionary<CommandResponseCodes, string> ResponseCodes { get; private set; }

        static CommandResponseValues()
        {
            SendFrom = "M1";
            SendTo = "C1";
            ResponseCodes = new Dictionary<CommandResponseCodes, string>
            {
                [CommandResponseCodes.Sucess] = "00",
                [CommandResponseCodes.WrongCommandFormat] = "01",
                [CommandResponseCodes.MachineBlockedCommand] = "02"
            };
        }
    }
}
