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
        public string NumberOfPodsToBeFilled { get; private set; }
        public string TotalCountOfPodsUsedByEachDrug { get; private set; }
        public List<RxRegistrationCommandDrug> RxRegistrationCommandDrugs { get; private set; }

        public PrescriptionRegistrationCommand(string commandString) : base(commandString)
        {

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

    class RxRegistrationCommandDrug
    {
        public string CassetteId { get; private set; }
        public string DrugName { get; private set; }
        public string QuantityPerCassette { get; private set; }
    }
}
