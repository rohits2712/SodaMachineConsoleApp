using System.Collections.Generic;

namespace SodaMachineConsoleApp.Models
{
    public class Inventory : IInventory
    {
        public Dictionary<int, IItem> AvailableItems { get; set; }
        private static volatile Inventory _inventory;
        private static readonly object PadLock = new object();

        public static Inventory Instance()
        {
            if (_inventory != null) return _inventory;
            lock (PadLock)
            {
                if (_inventory == null) _inventory = new Inventory();
            }

            return _inventory;
        }
        public Inventory()
        {
            InitializeInventory();
        }

        private void InitializeInventory()
        {
            //can be fetched directly from Repository or database
            AvailableItems = new Dictionary<int, IItem>
            {
                {1, new Beverage() {Code = 1, Name = "Coke", Rate = 5, Quantity = 100}},
                {2, new Beverage() {Code = 2, Name = "Fanta", Rate = 5, Quantity = 500}},
                {3, new Beverage() {Code = 3, Name = "Sprite", Rate = 5, Quantity = 600}}
            };
        }
    }


}
