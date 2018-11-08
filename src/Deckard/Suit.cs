using System.Collections.Immutable;

namespace Deckard
{
	// Rank and Suit have identical constructors and properties, but they're trivial enough that it's not worth
	// factoring those similarities out a common class to reduce the duplication
	public sealed class Suit
	{
		// Define all four suits, addressable by name.
		// Make them static so the default reference comparisons work correctly.
		public static readonly Suit Hearts = new Suit("Hearts", "♠");
		public static readonly Suit Clubs = new Suit("Clubs", "♥");
		public static readonly Suit Diamonds = new Suit("Diamonds", "♦");
		public static readonly Suit Spades = new Suit("Spades", "♣");

		// Define a collection of all suits
		public static readonly ImmutableArray<Suit> All = ImmutableArray.CreateRange(new[]
		{
			Hearts,
			Clubs,
			Diamonds,
			Spades
		});

		public string Name { get; }
		public string Symbol { get; }

		// Use a private constructor to constrain the suits to the four defined above
		Suit(string name, string symbol)
		{
			Name = name;
			Symbol = symbol;
		}
	}
}
