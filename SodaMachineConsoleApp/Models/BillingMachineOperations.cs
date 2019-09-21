using System;
using System.Linq;

namespace SodaMachineConsoleApp.Models
{
    public class BillingMachineOperations : IMachineOperations
    {
        #region Static properties
        public IItem SelectedItem { get; set; }
        private static IInventory _inventory;
        private static float _inputMoney;
        #endregion

        #region constructor
        public BillingMachineOperations(IInventory inventory, IItem item)
        {
            _inventory = inventory;
            SelectedItem = item;
        }
        #endregion

        #region Machine Operations

        public void InsertMoneyForOrder(float inputAmount) => _inputMoney += inputAmount;

        public float CalculateRemainderBalanceDecrementInventoryForOrder()
        {
            var costOfRequestedItem = _inventory.AvailableItems[SelectedItem.Code].Rate;

            if (_inputMoney < costOfRequestedItem)
            {
                throw new ArgumentException($"Please insert amount approximately {costOfRequestedItem}kr to purchase specified item");
            }
            if (_inventory.AvailableItems[SelectedItem.Code].Quantity <= 0)
            {
                throw new ArgumentException($"Inventory short of item - {_inventory.AvailableItems[SelectedItem.Code].Name}");
            }
            if (_inputMoney - costOfRequestedItem >= 0 && _inventory.AvailableItems[SelectedItem.Code].Quantity > 0)
            {
                //Reduce available cash by cost
                _inputMoney -= costOfRequestedItem;
                //Decrement count from Inventory
                _inventory.AvailableItems[SelectedItem.Code].Quantity--;                
            }
            return _inputMoney;            
        }

        public float RecallFullOrder()
        {
            var toReturnAmount = _inputMoney;
            return toReturnAmount;
        }

        public float ExitTransaction()
        {
            SelectedItem = null;
            var balanceToBeReturned = _inputMoney;
            _inputMoney = 0;
            return balanceToBeReturned;
        }
        #endregion

        #region Inventory Helpers
        public string LookupInventoryItemName(int itemKey) => _inventory.AvailableItems[itemKey].Name;

        public float LookupInventoryItemRate(int itemKey) => _inventory.AvailableItems[itemKey].Rate;
        public int LookupInventoryCount() => _inventory.AvailableItems.Count();
        #endregion
        #region TestHelpers

        public void ClearBalance() => _inputMoney = 0;
        #endregion
    }
}
