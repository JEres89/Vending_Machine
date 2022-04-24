using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Vending_Machine
{
	public class VendingMachine : I_Vending
	{

		private LinkedList<(Type itemType,int n)> inventory;
		private VendingPanel panel;
		private Wallet balance;

		public int Balance() { return balance.Balance; }

		public VendingMachine()
		{
			inventory = new LinkedList<(Type itemType, int n)>();
			panel = new VendingPanel(this);
			balance = new Wallet();
		}

		public VendingPanel Interact()
		{
			return panel;
		}

		public (Type itemType, int n)[] ShowAll()
		{
			var slots = from slot in inventory
				   where slot.n > 0
				   select slot;
			return slots.ToArray();
		}

		public int InsertMoney(Money money)
		{
			return balance.AddMoney(money);
		}

		private bool UseMoney(int cost)
		{
			if (cost <= balance.Balance)
			{
				balance.SetMoney(balance.Balance - cost);
				return true;
			}

			return false;
		}

		public VendingItem Purchase(Type itemType)
		{
			(Type itemType, int n) slot;
			try
			{
				slot = inventory.First<(Type, int)>(MatchItemType(itemType));
			}
			catch (Exception)
			{
				return null;
			}

			if (slot.n > 0)
			{
				VendingItem itemToBuy = VendingItem.GetItem(slot.itemType, true);
				if (UseMoney(itemToBuy.cost))
				{
					UpdateSlotInventory(slot, -1);
					return itemToBuy;
				}
			}
			return null;
		}

		public Wallet EndTransaction()
		{
			Wallet returnMoney = new Wallet(Money.GetBills(balance.Balance));

			balance.SetMoney(0);
			return returnMoney;
		}

		public int Resupply(VendingItem item, int amount)
		{
			(Type itemType, int n) slotValue = inventory.FirstOrDefault(MatchItemType(item.GetType())); 
											 //inventory.FirstOrDefault(new Func<(Type,int),bool>(x => x.Item1 == item.GetType()));

			if (slotValue.itemType == null)
			{
				slotValue = (item.GetType(), amount);
				inventory.AddLast(slotValue);
			}
			else
			{
				slotValue.n = UpdateSlotInventory(slotValue, amount);
			}

			return slotValue.n;
		}

		private int UpdateSlotInventory((Type itemType, int n) slotValue, int change)
		{
			LinkedListNode<(Type itemType, int n)> slot = inventory.Find(slotValue);
			slotValue.n += change;
			slot.Value = slotValue;

			return slotValue.n;
		}
		
		private static Func<(Type itemType, int n), bool> MatchItemType(Type desiredType)
		{
			return x => x.itemType == desiredType;
		}
	}
}
