using System;
using System.Collections.Generic;
using System.Text;

namespace Vending_Machine
{
	public interface I_Vending
	{

		public int Balance();

		public VendingPanel Interact();

		public (Type itemType, int n)[] ShowAll();

		public int InsertMoney(Money money);

		public VendingItem Purchase(Type itemType);

		public Wallet EndTransaction();

		public int Resupply(VendingItem item, int amount);
	}
}
