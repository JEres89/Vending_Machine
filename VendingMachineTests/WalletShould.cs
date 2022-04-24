using System;
using Vending_Machine;
using Xunit;

namespace VendingMachineTests
{
	public class WalletShould
	{
		readonly static int[] values;
		readonly static int sum;
		
		static WalletShould()
		{
			values = new int[Money.VALUES.Length];

			for (int i = 0; i < values.Length; i++)
			{
				values[i] = i + 1;
			}

			for (int i = 0; i < values.Length; i++)
			{
				sum += values[i] * Money.VALUES[i];

			}

		}

		[Fact]
		public void KeepMoney()
		{
			Wallet w = new Wallet(values);

			Assert.Equal(values, w.ShowMoney(false));
			Assert.Equal(sum, w.Balance);

			w.AddMoney(new Wallet(values));

			Assert.Equal(CombineValues(values, values), w.ShowMoney(false));

			w.SetMoney(values);

			Assert.Equal(values, w.ShowMoney(false));
		}

		[Fact]
		public void UpdateBalanceAndValues()
		{
			Wallet w = new Wallet(values);
			int addedBalance = 1234;
			int[] addedValues = Money.GetBills(addedBalance);
			Wallet u = new Wallet(addedValues);

			int expectedBalance = sum + addedBalance;
			int actualBalance = w.AddMoney(u);

			int[] expectedValues = CombineValues(values, addedValues);
			int[] actualValues = w.ShowMoney(false);

			Assert.Equal(expectedBalance, actualBalance);
			Assert.Equal(expectedValues, actualValues);
		}

		private int[] CombineValues(int[] a, int[] b)
		{
			int[] newValues = new int[8];

			for (int i = 0; i < 8; i++)
			{
				newValues[i] = a[i] + b[i];
			}

			return newValues;
		}
	}
}
