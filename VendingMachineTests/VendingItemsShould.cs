using System;
using Vending_Machine;
using Xunit;

namespace VendingMachineTests
{
	public class VendingItemsShould
	{

		[Fact]
		public void ReturnAnItem()
		{
			VendingItem v = VendingItem.GetItem(typeof(Candybar), false);

			Assert.True(v.GetType()==typeof(Candybar));
		}


	}
}
