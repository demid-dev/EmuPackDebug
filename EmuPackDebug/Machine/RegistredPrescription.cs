using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Machine
{
    class RegistredPrescription
    {
        public int Id { get; private set; }
        public List<Cassette> RegistredCassettes { get; private set;  }

        public RegistredPrescription(int id)
        {
            Id = id;
            RegistredCassettes = new List<Cassette>();
        }
    }
}
