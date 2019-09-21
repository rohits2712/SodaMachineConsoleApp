using System;

namespace SodaMachineConsoleApp.Models
{
    public class SodaMachine : ISodaMachine
    {
        private readonly IMachineOperations _sodaMachineOps;
        private IItem _item;
        public SodaMachine(IMachineOperations machineOperations, IItem item)
        {
            _sodaMachineOps = machineOperations;
            _item = item;

        }
        /// <summary>
        /// This is the starter method for the machine
        /// </summary>
        public void Start()
        {
            ShowWelcomeScreen();
            try
            {
                while (true)
                {
                    Console.WriteLine("*****************************************************************************************");
                    Console.WriteLine();
                    Console.WriteLine("Press 1 to Order Drink");
                    Console.WriteLine("Press 2 to Place SMS Order");
                    Console.WriteLine("Press 3 to Cancel and Recall Inserted Money from cash holder");
                    Console.WriteLine("Press 4 to Exit");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("Please specify your choice");
                    var userInput = Console.ReadLine();
                    var InputChoice = 0;
                    var validInput = ValidateInputChoice(userInput, out InputChoice);
                    if (validInput && InputChoice > 0 && InputChoice < 5)
                    {
                        //IMachineOperations ops = FactoryHelper.GetBillingMachineOperations();
                        switch (InputChoice)
                        {
                            case 1:
                            case 2:
                                bool improperInput = false;
                                ShowMenu();
                                Console.WriteLine("Enter code number for Drink you want - Valid code numbers are displayed as in menu");
                                if (int.TryParse(Console.ReadLine(), out var drinkItemCode)
                                    && (drinkItemCode > 0 && (drinkItemCode < _sodaMachineOps.LookupInventoryCount() + 1)))
                                {
                                    try
                                    {
                                        _sodaMachineOps.SelectedItem = _item;
                                        _sodaMachineOps.SelectedItem.Name = _sodaMachineOps.LookupInventoryItemName(drinkItemCode);
                                        _sodaMachineOps.SelectedItem.Code = drinkItemCode;
                                        _sodaMachineOps.SelectedItem.Quantity = 1;
                                        _sodaMachineOps.SelectedItem.Rate = _sodaMachineOps.LookupInventoryItemRate(drinkItemCode);
                                        Console.WriteLine("Enter Money to be placed in cash holder");
                                        ValidateInputChoice(Console.ReadLine(), out var insertedAmount);
                                        if (insertedAmount > 0)
                                        {
                                            _sodaMachineOps.InsertMoneyForOrder(insertedAmount);
                                        }
                                        Console.WriteLine($"Added {insertedAmount}kr to your credit balance");
                                        _sodaMachineOps.CalculateRemainderBalanceDecrementInventoryForOrder();
                                        Console.WriteLine("Please collect purchased item from holder");

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        improperInput = true;
                                        //continue;
                                    }
                                }
                                else { Console.WriteLine("Please enter valid choice"); continue; }
                                Console.WriteLine(InputChoice == 2 ?
                                    !improperInput ? "SMS Order successful" : "SMS Order unsuccessful"
                                    : !improperInput ? "Normal Order successful" : "Normal Order unsuccessful");
                                ExitTransaction(_sodaMachineOps);
                                break;
                            case 3:
                                var returnAvailableTransactionAmount = _sodaMachineOps.RecallFullOrder();
                                if (returnAvailableTransactionAmount > 0)
                                {
                                    ExitTransaction(_sodaMachineOps);
                                }
                                else
                                {
                                    Console.WriteLine($"All amount consumed already in transactions or no amount inserted. Have a nice day!!");
                                }
                                break;
                            case 4:
                                ExitTransaction(_sodaMachineOps);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter valid input");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("**********************************DRINKS MENU*************************************");
            foreach (var item in Inventory.Instance().AvailableItems)
            {
                Console.WriteLine(
                    $"Item name - {item.Value.Name} | Code number - {item.Value.Code} | Rate per item  - {item.Value.Rate}");
            }
            Console.WriteLine("**********************************************************************************");
        }

        private static bool ValidateInputChoice(string userInput, out int inputChoice) => int.TryParse(userInput, out inputChoice);

        private static void ExitTransaction(IMachineOperations ops)
        {
            var returnAmount = ops.ExitTransaction();
            Console.WriteLine(returnAmount > 0
                ? $"Please collect {returnAmount}kr from cash holder"
                : "All inserted money consumed in transactions or no amount inserted.Have a nice day!!!");

            ShowWelcomeScreen();
        }

        private static void ShowWelcomeScreen()
        {
            Console.WriteLine("*****************************************************************************************");
            Console.WriteLine("********************************Welcome to Soda Machine**********************************");
            Console.WriteLine("*****************************************************************************************");
        }
    }
}

