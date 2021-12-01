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
        public Adaptor Adaptor { get; private set; }
        public List<string> WarningCassettesIds { get; private set; }

        public MachineState()
        {
            Adaptor = new Adaptor();
            RegistredPrescriptions = new List<RegistredPrescription>();
            WarningCassettesIds = new List<string>();
        }

        public void ReinitilizeState()
        {
            RegistredPrescriptions = new List<RegistredPrescription>();
            DrawerOpened = false;
            WarningCassettesIds = new List<string>();
        }

        public void ChangeDrawerStatus(bool opened)
        {
            DrawerOpened = opened;
        }

        public void ClearWarningCassettesIds()
        {
            WarningCassettesIds = new List<string>();
        }
    }
}
