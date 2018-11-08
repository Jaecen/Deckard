using System;

namespace Deckard
{
	// A simple data object that defines a card as a combination of suit and rank
	public class Card
	{
		// Use readonly auto-properties to make the values immutable, but still play nicely with things like serialization
		public Suit Suit { get; }
		public Rank Rank { get; }

		public Card(Suit suit, Rank rank)
		{
			// Enforce that a suit and a rank must be provided
			Suit = suit ?? throw new ArgumentNullException(nameof(suit));
			Rank = rank ?? throw new ArgumentNullException(nameof(rank));
		}
	}
}
