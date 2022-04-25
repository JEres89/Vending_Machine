using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Vending_Machine
{
	public abstract class VendingItem
	{
		public abstract int cost { get; }
		public abstract string name { get; }
		public abstract string description { get; }
		public abstract string useEvent { get; }
		public abstract bool isConsumable { get;  }

		private static readonly List<VendingItem> catalogue = new List<VendingItem>();

		public static VendingItem GetItem(Type itemType, bool inCatalogue)
		{
			if (!inCatalogue || catalogue.Exists(new Predicate<VendingItem>(x => x.GetType() == itemType)))
			{
				ConstructorInfo conInfo = itemType.GetConstructor(new Type[0] { });

				VendingItem v = (VendingItem)conInfo.Invoke(null);

				return v;
			}
			
			else return null;
		}
		public static VendingItem SampleItem(Type itemType)
		{
			return catalogue.Find(x => x.GetType() == itemType);
		}

		public int Examine()
		{
			Console.WriteLine(description);
			return cost;
		}

		public virtual string Use()
		{
			return useEvent;
		}
		protected static void Register(VendingItem article)
		{
			catalogue.Add(article);
		}
	}
}
