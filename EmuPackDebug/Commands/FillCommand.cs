using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class FillCommand: Command
    {
        public string PrescriptionStartNotification { get; private set; }
        public string PrescriptionId { get; private set; }
        public string EndFlag { get; private set; }
        public string PodPosition { get; private set; }
        public string DispensingStartNotification { get; private set; }
        public string CassetteStartNotification { get; private set; }
        public string NumberOfDrugKinds { get; private set; }
        public List<CassetteUsedInFilling> CassettesUsedInFilling { get; private set; }

        public FillCommand(string commandString): base(commandString)
        {
            IsCommandValid = ValidateCommand(commandString);
        }

        public override CommandExecutionObject Execute(MachineState machineState)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateCommand(string commandString)
        {
            return base.ValidateCommand(commandString);
        }
    }

    class CassetteUsedInFilling
    {
        public string CassetteID { get; private set; }
        public string QuantityOfDrug { get; private set; }
    }

    static class FillCommandValues
    {
        static public string PrescriptionStartNotification { get; private set; }
        static public int PrescriptionStartNotificationStartIndex { get; private set; }
        static public int PrescriptionStartNotificationLength { get; private set; }
        static public int PrescriptionIdMinValue { get; private set; }
        static public int PrescriptionIdMaxValue { get; private set; }
        static public int PrescriptionIdStartIndex { get; private set; }
        static public int PrescriptionIdLength { get; private set; }
        static public string[] EndFlagPossibleValues { get; private set; }
        static public int EndFlagStartIndex { get; private set; }
        static public int EndFlagLength { get; private set; }
        static public string[] PodPositionPossibleValues { get; private set; }
        static public int PodPositionStartIndex { get; private set; }
        static public int PodPositionLength { get; private set; }
        static public string DispensingStartNotification { get; private set; }
        static public int DispensingStartNotificationStartIndex { get; private set; }
        static public int DispensingStartNotificationLength { get; private set; }
        static public int CassetteStartNotificationStartIndex { get; private set; }
        static public int CassetteStartNotificationLength { get; private set; }
        static public int NumberOfDrugKindsStartIndex { get; private set; }
        static public int NumberOfDrugKindsLength { get; private set; }
        static public int CassetteIDStartIndex { get; private set; }
        static public int CassetteIDLength { get; private set; }
        static public int QuantityOfDrugStartIndex { get; private set; }
        static public int QuantityOfDrugLength { get; private set; }

        static  FillCommandValues()
        {

        }
    }

    class FillCommandResposne : CommandResponse
    {
        
    }

}
