using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Machine
{
    class Adaptor
    {
        public bool AdaptorInsideMachine { get; private set; }
        public DrugPack DrugPack { get; private set; }

        public Adaptor()
        {
            AdaptorInsideMachine = true;
            DrugPack = new DrugPack();
        }
    }

    class DrugPack
    {
        public List<DrugCell> DrugCells { get; private set; }

        public DrugPack()
        {
            DrugCells = DrugPackValues.DrugCells;
        }
    }

    static class DrugPackValues
    {
        static public List<DrugCell> DrugCells { get; private set; }

        static DrugPackValues()
        {
            DrugCells = new List<DrugCell>
            {
                new DrugCell("A0"), new DrugCell("A1"), new DrugCell("A2"),
                new DrugCell("A3"), new DrugCell("A4"), new DrugCell("A5"),
                new DrugCell("A6"),
                new DrugCell("B0"), new DrugCell("B1"), new DrugCell("B2"),
                new DrugCell("B3"), new DrugCell("B4"), new DrugCell("B5"),
                new DrugCell("B6"),
                new DrugCell("C0"), new DrugCell("C1"), new DrugCell("C2"),
                new DrugCell("C3"), new DrugCell("C4"), new DrugCell("C5"),
                new DrugCell("C6"),
                new DrugCell("D0"), new DrugCell("D1"), new DrugCell("D2"),
                new DrugCell("D3"), new DrugCell("D4"), new DrugCell("D5"),
                new DrugCell("D6")
            };
        }
    }

    class DrugCell
    {
        public string CellName { get; private set; }
        public List<DrugInCell> DrugsInCell { get; private set; }

        public DrugCell(string cellName)
        {
            CellName = cellName;
            DrugsInCell = new List<DrugInCell>();
        }
    }

    class DrugInCell
    {
        public string DrugName { get; private set; }
        public int DrugQuantity { get; private set; }

        public DrugInCell(string drugName, int drugQuantity)
        {
            DrugName = drugName;
            DrugQuantity = drugQuantity;
        }

        public void AddDrugQuantity(int drugQuantity)
        {
            DrugQuantity += drugQuantity;
        }
    }
}
