using System;
using System.Collections.Generic;
using System.Text;

namespace Vending_Machine
{
	public class Candybar : VendingItem
	{
		public override int cost { get; } = 3;
		public override string name { get; } = "Candybar";
		public override string description { get; } = "A chocolate bar filled with almond nougat.";
		public override string useEvent { get; } = 
			"You put the bar in your mouth and begins to chew, wrappings and all";
		public override bool isConsumable { get; } = true;

		static Candybar()
		{
			VendingItem.Register(new Candybar());
		}

		//public Candybar(){}
	}

	public class Record : VendingItem
	{
		public override int cost { get; } = 19;
		public override string name { get; } = "Record";
		public override string description { get; } = "A CD with your favourite artist!";
		public override string useEvent { get; } = 
			"Unfortunately you don't own a CD-player anymore, " +
			"instead you throw the CD around like a frisbee.";
		public override bool isConsumable { get; } = false;

		static Record()
		{
			VendingItem.Register(new Record());
		}
		//public Record(){}
	}

	public class Newspaper : VendingItem
	{
		public override int cost { get; } = 9;
		public override string name { get; } = "Newspaper";
		public override string description { get; } = string.Format("A newspaper for {0}.", DateTime.Today.ToShortDateString());
		public override string useEvent { get; } = 
			"You flip through the newspaper looking for something interesting to read, " +
			"only to end up reading the comic-strips and solving a sudoku.\n" +
			"You then leave the paper on the nearest available seat for someone else to read.";
		public override bool isConsumable { get; } = true;

		static Newspaper()
		{
			VendingItem.Register(new Newspaper());
		}
		//public Newspaper(){}
	}
}
