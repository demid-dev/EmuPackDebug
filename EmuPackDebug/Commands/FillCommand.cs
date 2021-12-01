using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class FillCommand : Command
    {
        public string PrescriptionStartNotification { get; private set; }
        public string PrescriptionId { get; private set; }
        public string PodPosition { get; private set; }
        public string DispensingStartNotification { get; private set; }
        public string CassetteStartNotification { get; private set; }
        public string TotalNumberOfCassettes { get; private set; }
        public List<CassetteUsedInFilling> CassettesUsedInFilling { get; private set; }

        public FillCommand(string commandString) : base(commandString)
        {
            PrescriptionStartNotification = ReadCommandField(commandString,
                FillCommandValues.PrescriptionStartNotificationStartIndex,
                FillCommandValues.PrescriptionStartNotificationLength);
            PrescriptionId = ReadCommandField(commandString,
                FillCommandValues.PrescriptionIdStartIndex,
                FillCommandValues.PrescriptionIdLength);
            PodPosition = ReadCommandField(commandString,
                FillCommandValues.PodPositionStartIndex,
                FillCommandValues.PodPositionLength);
            DispensingStartNotification = ReadCommandField(commandString,
                FillCommandValues.DispensingStartNotificationStartIndex,
                FillCommandValues.DispensingStartNotificationLength);
            CassetteStartNotification = ReadCommandField(commandString,
                FillCommandValues.CassetteStartNotificationStartIndex,
                FillCommandValues.CassetteStartNotificationLength);
            TotalNumberOfCassettes = ReadCommandField(commandString,
                FillCommandValues.TotalNumberOfCassettesStartIndex,
                FillCommandValues.TotalNumberOfCassettesLength);
            CassettesUsedInFilling = new List<CassetteUsedInFilling>();
            ReadCassetteUsedInFillingData(commandString);

            string command = CommandId + SendFrom + SendTo + DataLength + PrescriptionStartNotification
            + PrescriptionId + PodPosition + DispensingStartNotification
            + CassetteStartNotification + TotalNumberOfCassettes;

            CassettesUsedInFilling.ForEach((cassette) =>
            {
                command += cassette.CassetteID + cassette.QuantityOfDrug;
            });

            Console.WriteLine(command);

            IsCommandValid = ValidateCommand(commandString);
            Console.WriteLine("Valid:" + IsCommandValid);
        }

        public override CommandExecutionObject Execute(MachineState machineState)
        {
            Func<FillCommandResponse> commandToExecute = () =>
            {
                if (!IsCommandValid)
                {
                    return new FillCommandResponse(CommandResponseCodes.WrongCommandFormat);
                }
                if (!ValidateCommandByMachine(machineState))
                {
                    
                    return new FillCommandResponse(CommandResponseCodes.MachineBlockedCommand);
                }
                ExecuteFilling(machineState);

                return new FillCommandResponse(CommandResponseCodes.Sucess);
            };

            int executionTime = FillCommandValues.ExecutionTime;
            bool machineMechanicalPartUsed = true;

            return new CommandExecutionObject(commandToExecute,
                executionTime, machineMechanicalPartUsed);
        }

        public override bool ValidateCommand(string commandString)
        {
            if (!base.ValidateCommand(commandString))
                return false;
            if (CommandId != FillCommandValues.CommandId)
                return false;
            if (PrescriptionStartNotification != FillCommandValues.PrescriptionStartNotification)
                return false;
            if (!ValidateNumericalField(PrescriptionId,
                FillCommandValues.PrescriptionIdLength,
                FillCommandValues.PrescriptionIdMinValue,
                FillCommandValues.PrescriptionIdMaxValue))
                return false;
            if (!FillCommandValues.PodPositionPossibleValues.Contains(PodPosition))
                return false;
            if (DispensingStartNotification != FillCommandValues.DispensingStartNotification)
                return false;
            if (CassetteStartNotification != FillCommandValues.CassetteStartNotification)
                return false;
            if (!ValidateNumericalField(TotalNumberOfCassettes,
                FillCommandValues.TotalNumberOfCassettesLength,
                FillCommandValues.TotalNumberOfCassettesMinValue,
                FillCommandValues.TotalNumberOfCassettesMaxValue))
                return false;

            bool cassetteInformationIsValid = true;
            CassettesUsedInFilling.ForEach(cassette =>
            {
                if (!ValidateNumericalField(cassette.CassetteID,
                    FillCommandValues.CassetteIDLength,
                    FillCommandValues.CassetteIDMinValue,
                    FillCommandValues.CassetteIDMaxValue))
                    cassetteInformationIsValid = false;
                if (!ValidateNumericalField(cassette.QuantityOfDrug,
                    FillCommandValues.QuantityOfDrugLength,
                    FillCommandValues.QuantityOfDrugMinValue,
                    FillCommandValues.QuantityOfDrugMaxValue))
                    cassetteInformationIsValid = false;
            });

            return cassetteInformationIsValid;
        }

        private void ReadCassetteUsedInFillingData(string commandString)
        {
            bool totalNumberParsed = int.TryParse(GetNumberWithoutPadding(TotalNumberOfCassettes),
                out int totalNumberOfCassettes);

            if (!totalNumberParsed)
                return;

            int cassetteInformationLength = FillCommandValues.CassetteIDLength + FillCommandValues.QuantityOfDrugLength;

            for (int i = 0; i < totalNumberOfCassettes; i++)
            {
                string cassetteId = ReadCommandField(commandString,
                    FillCommandValues.CassetteIDStartIndex + i * cassetteInformationLength,
                    FillCommandValues.CassetteIDLength);
                string quantityOfDrug = ReadCommandField(commandString,
                    FillCommandValues.QuantityOfDrugStartIndex + i * cassetteInformationLength,
                    FillCommandValues.QuantityOfDrugLength);
                CassetteUsedInFilling cassetteUsedInFilling = new CassetteUsedInFilling(cassetteId,
                    quantityOfDrug);
                CassettesUsedInFilling.Add(cassetteUsedInFilling);
            }
        }

        protected override bool ValidateCommandByMachine(MachineState machineState)
        {
            int prescriptionId = Convert.ToInt32(GetNumberWithoutPadding(PrescriptionId));
            RegistredPrescription registredPrescription = machineState.RegistredPrescriptions
                .Find(prescription => prescription.Id == prescriptionId);

            if (machineState.DrawerOpened || !machineState.Adaptor.AdaptorInsideMachine)
                return false;

            if (registredPrescription == null)
                return false;

            bool allCassettesExist = true;
            CassettesUsedInFilling.ForEach(cassette =>
            {
                int cassetteId = Convert.ToInt32(cassette.CassetteID);
                allCassettesExist = allCassettesExist && registredPrescription.RegistredCassettes.Any(cas
                    => cas.CassetteId == cassetteId);
            });
            if (!allCassettesExist)
                return false;

            bool allCassettesQuantityOfDrugsValid = true;
            CassettesUsedInFilling.ForEach(cassette =>
            {
                int cassetteId = Convert.ToInt32(cassette.CassetteID);
                int quantityOfDrug = Convert.ToInt32(cassette.QuantityOfDrug);

                allCassettesQuantityOfDrugsValid = allCassettesQuantityOfDrugsValid &&
                registredPrescription.RegistredCassettes.Find(cas => cas.CassetteId == cassetteId)
                .DrugQuantity > quantityOfDrug;
            });
            if (!allCassettesQuantityOfDrugsValid)
                return false;

            return true;
        }

        private void ExecuteFilling(MachineState machineState)
        {
            int prescriptionId = Convert.ToInt32(GetNumberWithoutPadding(PrescriptionId));
            RegistredPrescription registredPrescription = machineState.RegistredPrescriptions
                .Find(prescription => prescription.Id == prescriptionId);
            DrugCell drugCell = machineState.Adaptor.DrugPack.DrugCells
                .Find(cell => cell.CellName == PodPosition);

            CassettesUsedInFilling.ForEach(cassette =>
            {
                int cassetteId = Convert.ToInt32(cassette.CassetteID);
                int drugsQuantity = Convert.ToInt32(cassette.QuantityOfDrug);
                Cassette registredCassette = registredPrescription.RegistredCassettes
                    .Find(cas=> cas.CassetteId == cassetteId);
                string drugName = registredCassette.DrugName;

                registredCassette.DecreaseDrugQuantity(drugsQuantity);
                DrugInCell existentDrug = drugCell.DrugsInCell
                    .Find(d => d.DrugName == drugName);
                if(existentDrug != null)
                {
                    existentDrug.AddDrugQuantity(drugsQuantity);
                }
                else
                {
                    DrugInCell drug = new DrugInCell(drugName, drugsQuantity);
                    drugCell.DrugsInCell.Add(drug);
                }
            });
        }
    }

    class CassetteUsedInFilling
    {
        public string CassetteID { get; private set; }
        public string QuantityOfDrug { get; private set; }

        public CassetteUsedInFilling(string cassetteId, string quantityOfDrug)
        {
            CassetteID = cassetteId;
            QuantityOfDrug = quantityOfDrug;
        }
    }

    static class FillCommandValues
    {
        static public string CommandId { get; private set; }
        static public string PrescriptionStartNotification { get; private set; }
        static public int PrescriptionStartNotificationStartIndex { get; private set; }
        static public int PrescriptionStartNotificationLength { get; private set; }
        static public int PrescriptionIdMinValue { get; private set; }
        static public int PrescriptionIdMaxValue { get; private set; }
        static public int PrescriptionIdStartIndex { get; private set; }
        static public int PrescriptionIdLength { get; private set; }
        static public string[] PodPositionPossibleValues { get; private set; }
        static public int PodPositionStartIndex { get; private set; }
        static public int PodPositionLength { get; private set; }
        static public string DispensingStartNotification { get; private set; }
        static public int DispensingStartNotificationStartIndex { get; private set; }
        static public int DispensingStartNotificationLength { get; private set; }
        static public string CassetteStartNotification { get; private set; }
        static public int CassetteStartNotificationStartIndex { get; private set; }
        static public int CassetteStartNotificationLength { get; private set; }
        static public int TotalNumberOfCassettesMinValue { get; private set; }
        static public int TotalNumberOfCassettesMaxValue { get; private set; }
        static public int TotalNumberOfCassettesStartIndex { get; private set; }
        static public int TotalNumberOfCassettesLength { get; private set; }
        static public int CassetteIDMinValue { get; private set; }
        static public int CassetteIDMaxValue { get; private set; }
        static public int CassetteIDStartIndex { get; private set; }
        static public int CassetteIDLength { get; private set; }
        static public int QuantityOfDrugMinValue { get; private set; }
        static public int QuantityOfDrugMaxValue { get; private set; }
        static public int QuantityOfDrugStartIndex { get; private set; }
        static public int QuantityOfDrugLength { get; private set; }
        static public int ExecutionTime { get; private set; }

        static FillCommandValues()
        {
            CommandId = "FL";
            PrescriptionStartNotification = "P";
            PrescriptionStartNotificationStartIndex = 11;
            PrescriptionStartNotificationLength = 1;
            PrescriptionIdMinValue = 0;
            PrescriptionIdMaxValue = 9999;
            PrescriptionIdStartIndex = 12;
            PrescriptionIdLength = 4;
            PodPositionPossibleValues = new string[] {
            "A0", "A1", "A2", "A3", "A4", "A5", "A6",
            "B0", "B1", "B2", "B3", "B4", "B5", "B6",
            "C0", "C1", "C2", "C3", "C4", "C5", "C6",
            "D0", "D1", "D2", "D3", "D4", "D5", "D6" };
            PodPositionStartIndex = 16;
            PodPositionLength = 2;
            DispensingStartNotification = "M";
            DispensingStartNotificationStartIndex = 18;
            DispensingStartNotificationLength = 1;
            CassetteStartNotification = "C";
            CassetteStartNotificationStartIndex = 19;
            CassetteStartNotificationLength = 1;
            TotalNumberOfCassettesMinValue = 1;
            TotalNumberOfCassettesMaxValue = 40;
            TotalNumberOfCassettesStartIndex = 20;
            TotalNumberOfCassettesLength = 2;
            CassetteIDMinValue = 1;
            CassetteIDMaxValue = 40;
            CassetteIDStartIndex = 22;
            CassetteIDLength = 2;
            QuantityOfDrugMinValue = 1;
            QuantityOfDrugMaxValue = 99;
            QuantityOfDrugStartIndex = 24;
            QuantityOfDrugLength = 2;
            ExecutionTime = 5000;
        }
    }

    class FillCommandResponse : CommandResponse
    {
        public FillCommandResponse(CommandResponseCodes code) : base(code)
        {
            CommandId = FillCommandValues.CommandId;
            DataLength = "00002";
        }
    }
}
