using SodaMachineConsoleApp.Models;
using Unity;

namespace SodaMachineConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IItem, Beverage>();
            container.RegisterType<IMachineOperations, BillingMachineOperations>();
            container.RegisterType<IInventory, Inventory>();
            var sodaMachine = container.Resolve<SodaMachine>();
            sodaMachine.Start();
        }
    }



}
