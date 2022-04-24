using System;
using Vending_Machine;
using Xunit;

namespace VendingMachineTests
{
	public class VendingMachineShould
	{
		[Fact]
		public void Resupply()
		{
			VendingMachine v = new VendingMachine();

			v.Resupply(new Candybar(), 3);
			int n = v.Resupply(new Candybar(), 4);

			var slots = v.ShowAll();

			bool correctInventory = 
				slots.Length == 1 && 
				slots[0].itemType == typeof(Candybar) &&
				slots[0].n == 7;

			Assert.Equal(7, n);
		}

		[Fact]
		public void ReturnOptimalMoney()
		{
			VendingMachine v = new VendingMachine();

			for (int i = 0; i < 999; i++)
			{
				v.InsertMoney(Money.GetMoney(0));
			}
			int balance = v.Balance();
			Wallet w = v.EndTransaction();

			int[] bills = w.ShowMoney(false);
			int[] expectedBills = new int[]
			{
				4, //1kr
				1, //5kr
				0, //10kr
				2, //20kr
				1, //50kr
				4, //100kr
				1, //500kr
				0  //1000kr
			};

			Assert.Equal(balance, w.Balance);
			Assert.Equal(expectedBills, bills);
		}

		[Fact]
		public void SellProduct()
		{
			VendingMachine v = new VendingMachine();

			v.Resupply(new Candybar(), 3);

			for (int i = 0; i < 999; i++)
			{
				v.InsertMoney(Money.GetMoney(0));
			}

			VendingItem item = v.Purchase(typeof(Candybar));

			Wallet w = v.EndTransaction();

			Assert.Equal(typeof(Candybar), item.GetType());
			Assert.Equal(999 - item.cost, w.Balance);
			Assert.Equal(2, v.ShowAll()[0].n);
		}
	}
}
