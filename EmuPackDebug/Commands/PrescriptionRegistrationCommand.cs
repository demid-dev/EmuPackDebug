using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class PrescriptionRegistrationCommand : Command
    {
        public string RegistrationStartNotification { get; private set; }
        public string PrescriptionId { get; private set; }
        public string TotalNumberOfRegistredCassettes { get; private set; }
        public List<RxRegistrationCommandDrug> RxRegistrationCommandDrugs { get; private set; }

        public PrescriptionRegistrationCommand(string commandString) : base(commandString)
        {
            RegistrationStartNotification = ReadCommandField(commandString,
                PrescriptionRegistrationCommandValues.RegistrationStartNotificationStartIndex,
                PrescriptionRegistrationCommandValues.RegistrationStartNotificationLength);
            PrescriptionId = ReadCommandField(commandString,
                PrescriptionRegistrationCommandValues.PrescriptionIdStartIndex,
                PrescriptionRegistrationCommandValues.PrescriptionIdLength);
            TotalNumberOfRegistredCassettes = ReadCommandField(commandString,
                PrescriptionRegistrationCommandValues.TotalNumberOfRegistredCassettesStartIndex,
                PrescriptionRegistrationCommandValues.TotalNumberOfRegistredCassettesLength);
            RxRegistrationCommandDrugs = new List<RxRegistrationCommandDrug>();
            ReadDrugsUsedInRegistration(commandString);

            string command = CommandId + SendFrom + SendTo + DataLength + RegistrationStartNotification
            + PrescriptionId + TotalNumberOfRegistredCassettes;

            RxRegistrationCommandDrugs.ForEach((drug) =>
            {
                command += drug.CassetteId + drug.DrugName + drug.QuantityPerCassette;
            });

            Console.WriteLine(command);

            IsCommandValid = ValidateCommand(commandString);
            Console.WriteLine("Valid: " + IsCommandValid);
        }

        public override CommandExecutionObject Execute(MachineState machineState)
        {
            Func<PrescriptionRegistrationCommandResponse> commandToExecute = () =>
            {
                if (!IsCommandValid)
                {
                    return new PrescriptionRegistrationCommandResponse(CommandResponseCodes.WrongCommandFormat);
                }
                if (!ValidateCommandByMachine(machineState))
                {
                    return new PrescriptionRegistrationCommandResponse(CommandResponseCodes.MachineBlockedCommand);
                }
                RegisterPrescription(machineState);

                return new PrescriptionRegistrationCommandResponse(CommandResponseCodes.Sucess);
            };

            int executionTime = PrescriptionRegistrationCommandValues.ExecutionTime;
            bool machineMechanicalPartUsed = false;

            return new CommandExecutionObject(commandToExecute,
                executionTime, machineMechanicalPartUsed);
        }

        public override bool ValidateCommand(string commandString)
        {
            if (!base.ValidateCommand(commandString))
                return false;
            if (RegistrationStartNotification !=
                PrescriptionRegistrationCommandValues.RegistrationStartNotification)
                return false;
            if (!ValidateNumericalField(PrescriptionId,
                PrescriptionRegistrationCommandValues.PrescriptionIdLength,
                PrescriptionRegistrationCommandValues.PrescriptionIdMinValue,
                PrescriptionRegistrationCommandValues.PrescriptionIdMaxValue))
                return false;
            if (!ValidateNumericalField(TotalNumberOfRegistredCassettes,
                PrescriptionRegistrationCommandValues.TotalNumberOfRegistredCassettesLength,
                PrescriptionRegistrationCommandValues.TotalNumberOfRegistredCassettesMinValue,
                PrescriptionRegistrationCommandValues.TotalNumberOfRegistredCassettesMaxValue))
                return false;

            bool drugsInformationValid = true;
            RxRegistrationCommandDrugs.ForEach(drug =>
            {
                if (!ValidateNumericalField(drug.CassetteId,
                    PrescriptionRegistrationCommandValues.CassetteIDLength,
                    PrescriptionRegistrationCommandValues.CassetteIDMinValue,
                    PrescriptionRegistrationCommandValues.CassetteIDMaxValue))
                    drugsInformationValid = false;
                if (!ValidateFieldByLength(drug.DrugName,
                    PrescriptionRegistrationCommandValues.DrugNameLength))
                    drugsInformationValid = false;
                if (!ValidateNumericalField(drug.QuantityPerCassette,
                    PrescriptionRegistrationCommandValues.QuantityPerCassetteLength,
                    PrescriptionRegistrationCommandValues.QuantityPerCassetteMinValue,
                    PrescriptionRegistrationCommandValues.QuantityPerCassetteMaxValue))
                    drugsInformationValid = false;
            });

            return drugsInformationValid;
        }

        private void ReadDrugsUsedInRegistration(string commandString)
        {
            bool totalNumberParsed = int.TryParse(GetNumberWithoutPadding(TotalNumberOfRegistredCassettes),
                out int totalNumberOfDrugs);

            if (!totalNumberParsed)
                return;

            int drugsInformationLength = PrescriptionRegistrationCommandValues.CassetteIDLength
                + PrescriptionRegistrationCommandValues.DrugNameLength
                + PrescriptionRegistrationCommandValues.QuantityPerCassetteLength;

            for (int i = 0; i < totalNumberOfDrugs; i++)
            {
                string cassetteId = ReadCommandField(commandString,
                    PrescriptionRegistrationCommandValues.CassetteIDStartIndex + i * drugsInformationLength,
                    PrescriptionRegistrationCommandValues.CassetteIDLength);
                string drugName = ReadCommandField(commandString,
                    PrescriptionRegistrationCommandValues.DrugNameStartIndex + i * drugsInformationLength,
                    PrescriptionRegistrationCommandValues.DrugNameLength);
                string quantityPerCassette = ReadCommandField(commandString,
                    PrescriptionRegistrationCommandValues.QuantityPerCassetteStartIndex + i * drugsInformationLength,
                    PrescriptionRegistrationCommandValues.QuantityPerCassetteLength);
                RxRegistrationCommandDrug drug = new RxRegistrationCommandDrug(cassetteId,
                    drugName, quantityPerCassette);

                RxRegistrationCommandDrugs.Add(drug);
            }
        }

        protected override bool ValidateCommandByMachine(MachineState machineState)
        {
            int prescriptionId = Convert.ToInt32(GetNumberWithoutPadding(PrescriptionId));

            bool prescriptionExist = machineState.RegistredPrescriptions.Any(prescription =>
                prescription.Id == prescriptionId);

            return !prescriptionExist;
        }

        private void RegisterPrescription(MachineState machineState)
        {
            int prescriptionId = Convert.ToInt32(GetNumberWithoutPadding(PrescriptionId));
            List<Cassette> cassettes = new List<Cassette>();
            RxRegistrationCommandDrugs.ForEach(drug =>
            {
                int cassetteId = Convert.ToInt32(drug.CassetteId);
                string drugName = drug.DrugName;
                int drugQuantity = Convert.ToInt32(drug.QuantityPerCassette);
                Cassette cassette = new Cassette(cassetteId, drugName, drugQuantity);
                cassettes.Add(cassette);
            });

            RegistredPrescription prescription = new RegistredPrescription(prescriptionId);
            prescription.RegistredCassettes.AddRange(cassettes);
            machineState.RegistredPrescriptions.Add(prescription);
        }
    }

    class RxRegistrationCommandDrug
    {
        public string CassetteId { get; private set; }
        public string DrugName { get; private set; }
        public string QuantityPerCassette { get; private set; }

        public RxRegistrationCommandDrug(string cassetteId,
            string drugName, string quantityPerCassette)
        {
            CassetteId = cassetteId;
            DrugName = drugName;
            QuantityPerCassette = quantityPerCassette;
        }
    }

    static class PrescriptionRegistrationCommandValues
    {
        static public string CommandId { get; private set; }
        static public string RegistrationStartNotification { get; private set; }
        static public int RegistrationStartNotificationStartIndex { get; private set; }
        static public int RegistrationStartNotificationLength { get; private set; }
        static public int PrescriptionIdMinValue { get; private set; }
        static public int PrescriptionIdMaxValue { get; private set; }
        static public int PrescriptionIdStartIndex { get; private set; }
        static public int PrescriptionIdLength { get; private set; }
        static public int TotalNumberOfRegistredCassettesMinValue { get; private set; }
        static public int TotalNumberOfRegistredCassettesMaxValue { get; private set; }
        static public int TotalNumberOfRegistredCassettesStartIndex { get; private set; }
        static public int TotalNumberOfRegistredCassettesLength { get; private set; }
        static public int CassetteIDMinValue { get; private set; }
        static public int CassetteIDMaxValue { get; private set; }
        static public int CassetteIDStartIndex { get; private set; }
        static public int CassetteIDLength { get; private set; }
        static public int DrugNameStartIndex { get; private set; }
        static public int DrugNameLength { get; private set; }
        static public int QuantityPerCassetteMinValue { get; private set; }
        static public int QuantityPerCassetteMaxValue { get; private set; }
        static public int QuantityPerCassetteStartIndex { get; private set; }
        static public int QuantityPerCassetteLength { get; private set; }
        static public int ExecutionTime { get; private set; }

        static PrescriptionRegistrationCommandValues()
        {
            CommandId = "PR";
            RegistrationStartNotification = "I";
            RegistrationStartNotificationStartIndex = 11;
            RegistrationStartNotificationLength = 1;
            PrescriptionIdMinValue = 0;
            PrescriptionIdMaxValue = 9999;
            PrescriptionIdStartIndex = 12;
            PrescriptionIdLength = 4;
            TotalNumberOfRegistredCassettesMinValue = 1;
            TotalNumberOfRegistredCassettesMaxValue = 40;
            TotalNumberOfRegistredCassettesStartIndex = 16;
            TotalNumberOfRegistredCassettesLength = 2;
            CassetteIDMinValue = 1;
            CassetteIDMaxValue = 40;
            CassetteIDStartIndex = 18;
            CassetteIDLength = 2;
            DrugNameStartIndex = 20;
            DrugNameLength = 30;
            QuantityPerCassetteMinValue = 1;
            QuantityPerCassetteMaxValue = 99999;
            QuantityPerCassetteStartIndex = 50;
            QuantityPerCassetteLength = 5;
            ExecutionTime = 0;
        }
    }

    class PrescriptionRegistrationCommandResponse: CommandResponse
    {
        public PrescriptionRegistrationCommandResponse(CommandResponseCodes code) : base(code)
        {
            CommandId = PrescriptionRegistrationCommandValues.CommandId;
            DataLength = "00002";
        }
    }
}
