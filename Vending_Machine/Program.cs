using System;
using System.Collections.Generic;

namespace Vending_Machine
{
	class Program
	{
		static void Main(string[] args)
		{
			int[] values = { 8,7,6,5,4,3,2,1 };
			Wallet customerMoney = new Wallet(values);

			VendingMachine v = new VendingMachine();

			v.Resupply(new Candybar(), 42);
			v.Resupply(new Record(), 13);
			v.Resupply(new Newspaper(), 19);

			LinkedList<VendingItem> purchases = new LinkedList<VendingItem>(v.Interact().Browse(customerMoney));
			LinkedListNode<VendingItem> active = purchases.First;


			while (active != null)
			{
				var next = active.Next == null ? purchases.First : active.Next;
				var previous = active.Previous == null ? purchases.Last : active.Previous;

				Console.Clear();
				Console.WriteLine("You have a {0}. It's worth {1}.", active.Value.name, active.Value.cost);
				Console.WriteLine("\nWould you like to use it? (Y)");
				Console.WriteLine("<- {0}                                 {1} ->", previous.Value.name, next.Value.name);
				Console.WriteLine("Exit (X)");

				ConsoleKey key = Console.ReadKey(true).Key;
				switch (key)
				{
					case ConsoleKey.Y:
						Console.WriteLine(active.Value.Use());
						Console.ReadKey();
						if (active.Value.isConsumable)
						{
							purchases.Remove(active.Value);
							if (active == next)
							{
								active = null;
								break;
							}
							active = next;
						}
						break;
					case ConsoleKey.LeftArrow:
						active = previous;
						break;
					case ConsoleKey.RightArrow:
						active = next;
						break;
					case ConsoleKey.X:
						return;
					default:
						break;
				}
			}
		}

		private static bool Navigate(ConsoleKey key, out int i)
		{
			switch (key)
			{
				case ConsoleKey.Y:
					i = 0;
					return true;
				case ConsoleKey.LeftArrow:
					i = -1;
					return false;
				case ConsoleKey.RightArrow:
					i = 1;
					return false;
				case ConsoleKey.X:

				default:
					i = 0;
					return false;
			}
		}
	}
}
