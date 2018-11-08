using System.Collections.Generic;

namespace Deckard
{
	public class DeckGenerator
	{
		// Create a collection of all combinations of suits and ranks. This collection will
		// come out sorted by suit and then rank.
		public IEnumerable<Card> GenerateDeck()
		{
			foreach(var suit in Suit.All)
				foreach(var rank in Rank.All)
					yield return new Card(suit, rank);
		}
	}
}
