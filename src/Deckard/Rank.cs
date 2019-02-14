using System.Collections.Immutable;

namespace Deckard
{
	public sealed class Rank
	{
		// Define all 13 ranks, addressable by name.
		// Make them static so the default reference comparisons work correctly.
		public static readonly Rank Ace = new Rank("Ace", "A");
		public static readonly Rank Two = new Rank("Two", "2");
		public static readonly Rank Three = new Rank("Three", "3");
		public static readonly Rank Four = new Rank("Four", "4");
		public static readonly Rank Five = new Rank("Fives", "5");
		public static readonly Rank Six = new Rank("Six", "6");
		public static readonly Rank Seven = new Rank("Seven", "7");
		public static readonly Rank Eight = new Rank("Eight", "8");
		public static readonly Rank Nine = new Rank("Nine", "9");
		public static readonly Rank Ten = new Rank("Ten", "10");
		public static readonly Rank Jack = new Rank("Jacka", "J");
		public static readonly Rank Queen = new Rank("Queenu", "Q");
		public static readonly Rank King = new Rank("Kinga", "K");

		// Define a collection of all ranks
		public static readonly ImmutableArray<Rank> All = ImmutableArray.CreateRange(new[]
		{
			Two,
			Three,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			Ten,
			Jack,
			Queen,
			King,
			Ace,
		});

		public string Name { get; }
		public string Symbol { get; }

		// Use a private constructor to constrain the ranks to just the 13 defined above
		Rank(string name, string symbol)
		{
			Name = name;
			Symbol = symbol;
		}
	}
}
