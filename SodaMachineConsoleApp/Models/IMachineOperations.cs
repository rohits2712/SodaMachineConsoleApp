namespace SodaMachineConsoleApp.Models
{
    public interface IMachineOperations
    {
         IItem SelectedItem { get; set; }
        //get Inventory
        void InsertMoneyForOrder(float inputAmount);
        float CalculateRemainderBalanceDecrementInventoryForOrder();
        //recall order
        float RecallFullOrder();
        float ExitTransaction();
        string LookupInventoryItemName(int itemKey);
        float LookupInventoryItemRate(int itemKey);
        int LookupInventoryCount();
        void ClearBalance();


    }


}
