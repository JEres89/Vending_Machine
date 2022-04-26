using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vending_Machine
{
	public class VendingPanel
	{
		readonly I_Vending machine;

		private const string instructions = "Instructions: Use ARROWKEYS UP and DOWN to navigate the list,\n" +
				"LEFT and RIGHT to switch access to and from the money slot and end transaction button,\n" +
				"and ENTER to select the highlighted choice.";

		private static int padding = 2;
		private static int panel_end;

		private static int list_rowStart = 2;
		private static int list_rowEnd;
		private static int list_columnStart = 0;
		private static int list_width = 42;

		private static int balance_rowStart = list_rowStart + padding;
		private static int balance_rowEnd = balance_rowStart + 4;
		private static int balance_columnStart = list_width + padding;

		private static int money_rowStart = balance_rowEnd + padding;
		private static int money_rowEnd = money_rowStart + 6;
		private static int money_columnStart = list_width + padding;
		private static int money_columnEnd = money_columnStart + 10;

		private static int abort_rowStart = money_rowEnd + padding * 2;
		private static int abort_columnStart = list_width + padding * 2;

		private static int wallet_rowStart = money_rowStart;
		private static int wallet_columnStart = money_columnEnd + padding;


		private const int insertMoney = -3;
		private const int endTransaction = -2;

		private List<VendingItem> tray;
		private (Type itemType, int n)[] slots;
		private string[] slotStrings;

		public VendingPanel(I_Vending machine)
		{
			this.machine = machine;
		}

		public List<VendingItem> Browse(Wallet customerWallet)
		{
			Console.SetWindowSize(100, 50);
			Console.SetBufferSize(100, 50);
			Console.CursorVisible = false;

			tray = new List<VendingItem>();
			UpdateSlotInfo();
			list_rowEnd = list_rowStart + slotStrings.Length * 2 + padding;

			int selection = 0;
			ConsoleKey key = ConsoleKey.NoName;
			bool keyIsValid = false;
			while (true)
			{
				Console.Clear();
				bool hasSelected = false;

				while (!hasSelected)
				{
					DrawPanel(slotStrings, selection);
					if(!keyIsValid) key = Console.ReadKey(true).Key;

					hasSelected = Navigate(key, slots.Length, selection, out selection);
					keyIsValid = false;
				}

				Console.SetCursorPosition(0, panel_end);
				switch (selection)
				{
					case int i when (i >= 0 && i < slots.Length):
						if(Buy(slots[selection].itemType))
						{
							UpdateSlotInfo();
							selection = selection >= slots.Length ? slots.Length-1 : selection;
						}
						key = Console.ReadKey(true).Key;
						keyIsValid = true;
						break;
					case endTransaction:
						customerWallet.AddMoney(ReturnMoney());
						return tray;
					case insertMoney:
						InsertMoney(customerWallet);
						break;
					default:
						break;
				}
			}
		}
		public void Maintenance()
		{
			// options to resupply, change pricings, currency etc
		}
		private void UpdateSlotInfo()
		{
			slots = machine.ShowAll();
			slotStrings = Stringify(slots);
		}
		private bool Buy(Type itemType)
		{
			VendingItem item = machine.Purchase(itemType);
			if (item == null)
			{
				Console.WriteLine("Your balance is not sufficient for that item, please insert more money.");
				return false;
			}

			tray.Add(item);
			Console.WriteLine("Thank you for your purchase!");
			return true;
		}
		private Wallet ReturnMoney()
		{
			Wallet returnMoney = machine.EndTransaction();
			Console.WriteLine("{0} Swedish Kronor(SEK) returned.", returnMoney.Balance);
			Console.WriteLine("Your returned face-values are:");
			returnMoney.ShowMoney(true);
			Console.ReadKey();
			return returnMoney;
		}
		private void InsertMoney(Wallet customerWallet)
		{
			List<string> moneyStrings = new List<string>(Money.GetMoneyStrings(customerWallet, true));
			int[] customerMoney = customerWallet.ShowMoney(false);
			int selection = 0;
			ConsoleKey[] acceptableInput = { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter };

			while (true)
			{
				bool hasSelected = false;
				//DrawWallet(moneyStrings, selection);
				while (!hasSelected)
				{
					DrawWallet(moneyStrings.ToArray(), selection);
					ConsoleKey key = Console.ReadKey().Key;
					if (acceptableInput.Contains(key))
					{
						hasSelected = Navigate(key, moneyStrings.Count + 1, selection, out selection);
					}
				}

				if (selection < moneyStrings.Count)
				{
					int moneyIndex = TranslateStringIndex(customerMoney, selection);

					machine.InsertMoney(customerWallet.TakeMoney(Money.GetMoney(moneyIndex)));
					if (customerMoney[moneyIndex] == 0)
					{
						moneyStrings.RemoveAt(selection);
					}
					else
					{
						string updatedString = Money.GetMoneyStrings(customerWallet, moneyIndex, 1)[0];
						moneyStrings[selection] = updatedString;
					}
				}
				else
				{
					return;
				}
			}
		}

		private void DrawPanel(string[] slots, int selection)
		{
			Console.SetCursorPosition(0, list_rowStart);
			string selector;

			for (int i = 0; i < slots.Length; i++)
			{
				selector = selection == i ? "> >" : "   ";
				Console.WriteLine(slots[i], selector);
				Console.WriteLine();
			}

			int x = balance_columnStart;
			int y = balance_rowStart;
			DrawAt("___________", x, y++);
			DrawAt("| Balance |", x, y++);

			string s = string.Format(
				"|{1}{0} kr |",
				machine.Balance(),
				new string(' ', 5 - machine.Balance().ToString().Length));
			DrawAt(s, x, y++);
			DrawAt("|_________|", x, y++);

			x = money_columnStart;
			y = money_rowStart;
			selector = selection == insertMoney ? "===========" : new string(' ', 11);
			DrawAt(selector, x, y++);
			DrawAt("/---------\\", x, y++);
			DrawAt("|   (|)   |", x, y++);
			DrawAt("| _______ |", x, y++);
			DrawAt("||=======||", x, y++);
			DrawAt("|_________|", x, y++);
			DrawAt(selector, x, y++);

			x = abort_columnStart;
			y = abort_rowStart;
			selector = selection == endTransaction ? "=======" : new string(' ', 7);
			DrawAt(selector, x - 1, y++);
			DrawAt(",---,", x, y++);
			DrawAt("| X |", x, y++);
			DrawAt("'---'", x, y++);
			DrawAt(selector, x - 1, y++);

			panel_end = y + padding;

			DrawAt(instructions, 0, panel_end+3);

		}
		private void DrawWallet(string[] moneyStrings, int selection)
		{
			string selector;
			string fullString = "";
			int x = wallet_columnStart;
			int y = wallet_rowStart;
			DrawAt("What value would you like to insert?", x, y++);

			for (int i = 0; i < moneyStrings.Length; i++)
			{
				selector = selection == i ? " > > " : "     ";
				fullString = string.Format("{0}{1}", selector, moneyStrings[i]);
				DrawAt(fullString, x, y++);
			}

			selector = selection == moneyStrings.Length ? " > > " : "     ";
			fullString = string.Format("{0}Done{1}", selector, new string(' ', fullString.Length));
			DrawAt(fullString, x, y++);
			DrawAt(new string(' ', fullString.Length), x, y++);
		}
		private void DrawAt(string s, int x, int y)
		{
			Console.SetCursorPosition(x, y);
			Console.Write(s);

		}
		private string[] Stringify((Type itemType, int n)[] slots)
		{
			string itemLine = "| {0}. {5} {1}{2}{3}kr - Stock: {4} |";
			string[] slotStrings = new string[slots.Length];

			for (int i = 0; i < slots.Length; i++)
			{
				VendingItem item = VendingItem.SampleItem(slots[i].itemType);
				int padding = 15 - (item.name.Length < 13 ? item.name.Length : 13) - item.cost.ToString().Length;

				slotStrings[i] = string.Format(itemLine, i + 1, item.name, new string(' ', padding), item.cost, slots[i].n, "{0}");
				/**
				Console.Write(slotStrings[i].Length + " ");
				Console.ReadKey(true);/**/
			}
			return slotStrings;
		}

		private bool Navigate(ConsoleKey key, int max, int selection, out int newSelection)
		{
			newSelection = selection; 
			bool select = false;

			switch (key)
			{
				case ConsoleKey.Enter:
					select = true;
					break;
				case ConsoleKey.UpArrow:
					newSelection--;
					break;
				case ConsoleKey.DownArrow:
					newSelection++;
					break;
				case ConsoleKey.LeftArrow:
					newSelection = 0;
					break;
				case ConsoleKey.RightArrow:
					newSelection = insertMoney;
					break;
				default:
					break;
			}

			if (selection >= 0 && selection < max)
			{
				newSelection =
					newSelection == -1 ?
						0 :
						newSelection == max ?
							max - 1 :
							newSelection;
			}
			if (selection < -1)
			{
				newSelection =
					newSelection == -1 ?
						endTransaction :
						newSelection == -4 ?
							insertMoney :
							newSelection;
			}
			return select;
		}
		private int TranslateStringIndex(int[] customerMoney, int selection)
		{
			int s = 0;

			for (int i = 0; i < customerMoney.Length; i++)
			{
				if (customerMoney[i] == 0)
				{
					continue;
				}
				else
				{
					if (selection == s)
					{
						return i;
					}
					s++;
				}
			}
			return -1;
		}
	}
}
