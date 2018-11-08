using System;
using System.Collections.Generic;

namespace Deckard
{
	// A trivial service to demonstrate composing other services together.
	public class Dealer
	{
		readonly DeckGenerator DeckGenerator;
		readonly IShuffler Shuffler;

		public Dealer(
			DeckGenerator deckGenerator,
			IShuffler shuffler)
		{
			DeckGenerator = deckGenerator ?? throw new ArgumentNullException(nameof(deckGenerator));
			Shuffler = shuffler ?? throw new ArgumentNullException(nameof(shuffler));
		}

		public IEnumerable<Card> Deal()
			=> Shuffler.Shuffle(DeckGenerator.GenerateDeck());
	}
}
