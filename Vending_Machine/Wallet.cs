using System;
using System.Collections.Generic;
using System.Text;

namespace Vending_Machine
{
	public class Wallet
	{
		private readonly int[] values;

		public int Balance { get; private set; } = 0;

		public Wallet(int[] values)
		{
			this.values = Money.GetBills(0);
			SetMoney(values);
		}
		public Wallet(int value)
		{
			values = Money.GetBills(value);
		}
		public Wallet() : this(0)
		{
		}

		public int SetMoney(int value)
		{
			return SetMoney(Money.GetBills(value));
		}
		public int SetMoney(int[] newValues)
		{

			for (int i = 0; i < ((values.Length < newValues.Length) ? values.Length : newValues.Length); i++)
			{
				values[i] = newValues[i] < 0 ? 0 : newValues[i];
			}

			return UpdateBalance();
		}
		public int AddMoney(Money value)
		{
			values[value.valueIndex]++;
			return UpdateBalance();
		}
		public int AddMoney(Wallet newValues)
		{
			Wallet a = this;
			a += newValues;
			return Balance;
		}
		public Money TakeMoney(Money value)
		{
			if (values[value.valueIndex]-- < 0)
			{
				values[value.valueIndex]++;
				return null;
			}
			UpdateBalance();
			return value;
		}
		public Wallet TakeMoney(int[] takeValues)
		{
			Wallet takeRequest = new Wallet(takeValues);

			Wallet w = this;

			w -= takeRequest;

			return takeRequest;
		}
		private int UpdateBalance()
		{
			return Balance = Money.Count(values);
		}
		public int[] ShowMoney(bool printExtras)
		{
			if(printExtras)
			{
				//Console.WriteLine("This wallet contains:");
				string[] moneyLines = Money.GetMoneyStrings(this, true, 0, values.Length);
				foreach (string s in moneyLines)
				{
					Console.WriteLine(s);
				}
				Console.WriteLine("for a total value of {0} Swedish Kronor(SEK)", Balance);
			}

			return values;
		}

		/*
		 * Transfers all money from Wallet $b to $a
		 * 
		 */
		public static Wallet operator +(Wallet a, Wallet b)
		{
			for (int i = 0; i < a.values.Length; i++)
			{
				a.values[i] += b.values[i];
				b.values[i] = 0;
			}
			a.UpdateBalance();
			b.UpdateBalance();
			return a;
		}
		/*
		 * Deduces money in Wallet $a up to the number specified in $b
		 * but not more than what is present in $a,
		 * and leaves $b with exactly what was deduced from $a.
		 * 
		 */
		public static Wallet operator -(Wallet a, Wallet b)
		{

			for (int i = 0; i < a.values.Length; i++)
			{
				if (a.values[i] < b.values[i])
				{
					b.values[i] = a.values[i];
					a.values[i] = 0;
				}
				else
				{
					a.values[i] -= b.values[i];
				}
			}
			a.UpdateBalance();
			b.UpdateBalance();
			return a;
		}
	}
}
