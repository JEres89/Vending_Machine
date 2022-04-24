/* 
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Vending_Machine
{
	public class Money
	{
		public static readonly int[] VALUES =
		{
			1,
			5,
			10,
			20,
			50,
			100,
			500,
			1000
		};

		public readonly int valueIndex;
		public readonly string description;

		private static readonly Money[] moneyInstances =
		{
			new Money(0, "{0} Coin{1} of 1 Swedish Krona(SEK)"),
			new Money(1, "{0} Coin{1} of 5 Swedish Kronor(SEK)"),
			new Money(2, "{0} Coin{1} of 10 Swedish Kronor(SEK)"),
			new Money(3, "{0} Bill{1} of 20 Swedish Kronor(SEK)"),
			new Money(4, "{0} Bill{1} of 50 Swedish Kronor(SEK)"),
			new Money(5, "{0} Bill{1} of 100 Swedish Kronor(SEK)"),
			new Money(6, "{0} Bill{1} of 500 Swedish Kronor(SEK)"),
			new Money(7, "{0} Bill{1} of 1000 Swedish Kronor(SEK)")
		};

		public static Money GetMoney(int valueIndex)
		{
			if (valueIndex < 0 || valueIndex > 7)
			{
				throw new Exception("Invalid money value index (out of bounds): {valueIndex}");
			}

			return moneyInstances[valueIndex];
		}

		public static int[] GetBills(int value)
		{
			int[] returnBills = new int[VALUES.Length];

			if (value != 0)
			{
				for (int i = VALUES.Length - 1; i >= 0; i--)
				{
					int remainder = value % VALUES[i];

					returnBills[i] = (value - remainder) / VALUES[i];

					value = remainder;
				}
			}

			return returnBills;
		}
		public static int Count(int[] values)
		{
			int total = 0;

			for (int i = 0; i < ((VALUES.Length < values.Length) ? VALUES.Length : values.Length); i++)
			{
				total += VALUES[i] * values[i];
			}

			return total;
		}
		//public static void ShowMoney(int[] values)
		//{
		//	string[] moneyLines = GetMoneyStrings(values);

		//	for (int i = 0; i < moneyInstances.Length; i++)
		//	{
		//		if (values[i] != 0)
		//		{
		//			Console.WriteLine(moneyLines[i]);
		//		}
		//	}
		//}
		
		public static string[] GetMoneyStrings(Wallet wallet)
		{
			return GetMoneyStrings(wallet, false, 0, 8);
		}
		public static string[] GetMoneyStrings(Wallet wallet, bool onlyNonZero)
		{
			return GetMoneyStrings(wallet, onlyNonZero, 0, 8);
		}
		public static string[] GetMoneyStrings(Wallet wallet, int first, int last)
		{
			return GetMoneyStrings(wallet, false, first, last);
		}
		// first is first index, last is total number (length)
		public static string[] GetMoneyStrings(Wallet wallet, bool onlyNonZero, int first, int last)
		{
			int[] values = wallet.ShowMoney(false);
			List<string> moneyStrings = new List<string>();

			for (int i = first; i < first+last; i++)
			{
				if (onlyNonZero && values[i] == 0)
				{
					continue;
				}
				string s = string.Format(moneyInstances[i].description,
					values[i],
					values[i] == 1 ? " " : "s");
				moneyStrings.Add(s);
			}
			return moneyStrings.ToArray();
		}
		private Money(int valueIndex, string description)
		{
			this.valueIndex = valueIndex;
			this.description = description;
		}

		public int Value()
		{
			return VALUES[valueIndex];
		}
	}
}
