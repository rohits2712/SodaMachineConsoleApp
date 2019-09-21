using System;

namespace SodaMachineConsoleApp.Models
{
    public class Beverage : IItem
    {
        private string _name;
        private int _quantity;
        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value.Trim()))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException("Invalid Input");
                }
            }
        }
        public int Code { get; set; }
        public float Rate { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (int.TryParse(value.ToString(), out value))
                {
                    _quantity = value;
                }
                else
                {
                    throw new ArgumentException("Invalid Input quantity");
                }
            }
        }
    }

}
