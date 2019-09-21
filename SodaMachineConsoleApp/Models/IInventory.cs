using System.Collections.Generic;

namespace SodaMachineConsoleApp.Models
{
    public interface IInventory
    {
        Dictionary<int, IItem> AvailableItems { get; set; }
    }
}
