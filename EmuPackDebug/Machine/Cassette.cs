using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Machine
{
    class Cassette
    {
        public int CassetteId { get; private set; }
        public string DrugName { get; private set; }
        public int DrugQuantity { get; private set; }

        public Cassette(int cassetteId, string drugName, int drugQuantity)
        {
            CassetteId = cassetteId;
            DrugName = drugName;
            DrugQuantity = drugQuantity;
        }

        public void DecreaseDrugQuantity(int quantity)
        {
            DrugQuantity -= quantity;
        }
    }
}
