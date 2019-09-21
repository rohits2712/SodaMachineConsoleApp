using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SodaMachineConsoleApp.Models;

namespace SodaMachineBillingOperationTests
{
    [TestClass]
    public class UnitTestsClass
    {
        IInventory _inventory;
        IItem _item;
        IMachineOperations _billingMachineOperations;
        ISodaMachine _sodaMachine;

        [TestInitialize]
        public void TestInit()
        {
            var availableItems = new Dictionary<int, IItem>();
            //Initialize the test inventory
            availableItems = new Dictionary<int, IItem>
            {
                {1, new Beverage() {Code = 1, Name = "Coke", Rate = 5, Quantity = 100}},
                {2, new Beverage() {Code = 2, Name = "Fanta", Rate = 5, Quantity = 500}},
                {3, new Beverage() {Code = 3, Name = "Sprite", Rate = 5, Quantity = 600}}
            };
            _inventory = Substitute.For<IInventory>();
            _inventory.AvailableItems.ReturnsForAnyArgs(availableItems);
            _item = new Beverage() { Code = 1, Name = "Fanta", Rate = 5, Quantity = 1 };
            _billingMachineOperations = new BillingMachineOperations(_inventory, _item);
            _sodaMachine = new SodaMachine(_billingMachineOperations, _item);
            _billingMachineOperations.ClearBalance();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _inventory = null;
            _item = null;
            _billingMachineOperations = null;
            _sodaMachine = null;

        }

        [TestMethod]
        public void Test_CalculateRemainderMoneyForOrderWithExactAmount()
        {
            //set selected item and place inventory
            _billingMachineOperations.InsertMoneyForOrder(5);
            var remainder = _billingMachineOperations.CalculateRemainderBalanceDecrementInventoryForOrder();
            Assert.AreEqual(Math.Round(0.0f), remainder);
        }

        [TestMethod]
        public void Test_CalculateRemainderMoneyForOrderWithGreaterThanExactAmount()
        {
            var billingMachineOperations = new BillingMachineOperations(_inventory, _item);
            billingMachineOperations.InsertMoneyForOrder(100);
            var remainingMoney = billingMachineOperations.CalculateRemainderBalanceDecrementInventoryForOrder();
            Assert.AreEqual(95, remainingMoney);
        }

        [TestMethod]
        //User should not be able to buy and value to be returned should be same as value they entered in cash holder
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateRemainderMoneyForOrderWithLesserThanExactAmount()
        {
            //set selected item and place inventory
            _billingMachineOperations.InsertMoneyForOrder(4);
            var remainingMoney = _billingMachineOperations.CalculateRemainderBalanceDecrementInventoryForOrder();
        }

        [TestMethod]
        //User should not be able to buy when item not present in inventory and value to be returned should be same as value they entered in cash holder
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateRemainderMoneyForOrderWhenInventoryDoesNotHaveItem()
        {
            _inventory.AvailableItems[_item.Code].Quantity = 0;
            _billingMachineOperations.InsertMoneyForOrder(5);
            var remainingMoney = _billingMachineOperations.CalculateRemainderBalanceDecrementInventoryForOrder();
        }


        [TestMethod]
        public void Test_RecallFullOrder()
        {
            _billingMachineOperations.InsertMoneyForOrder(100);
            var returnableAmount = _billingMachineOperations.RecallFullOrder();
            Assert.AreEqual(100, returnableAmount);
        }

        [TestMethod]
        public void Test_ExitTransaction()
        {
            //set selected item and place inventory
            _billingMachineOperations.InsertMoneyForOrder(100);
            var returnableAmount = _billingMachineOperations.ExitTransaction();
            Assert.AreEqual(100, returnableAmount);
        }




    }
}
