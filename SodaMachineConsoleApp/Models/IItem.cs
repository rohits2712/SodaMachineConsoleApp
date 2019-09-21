namespace SodaMachineConsoleApp.Models
{
    public interface IItem
    {
        string Name { get; set; }
        int Code { get; set; }
        float Rate { get; set; }
        int Quantity { get; set; }
    }


}
