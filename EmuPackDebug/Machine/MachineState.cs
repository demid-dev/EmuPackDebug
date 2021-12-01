using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Machine
{
    class MachineState
    {
        public List<RegistredPrescription> RegistredPrescriptions { get; private set; }
        public bool DrawerOpened { get; private set; }
        public bool DispensingInProgress { get; private set; }
        public Adaptor Adaptor { get; private set; }

        public MachineState()
        {
            Adaptor = new Adaptor();
            RegistredPrescriptions = new List<RegistredPrescription>();
        }
    }
}
