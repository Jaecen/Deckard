using System.Linq;
using Xunit;

namespace Deckard.Test
{
	public class DeckGeneratorTest
	{
		public class When_Generating_Tests
		{
			[Fact]
			public void Should_Contain_52_Cards()
			{
				var deckGenerator = new DeckGenerator();
				var deck = deckGenerator.GenerateDeck();

				var cardCount = deck.Count();

				Assert.Equal(52, cardCount);
			}

			[Fact]
			public void Should_Have_Distinct_Cards()
			{
				var deckGenerator = new DeckGenerator();
				var deck = deckGenerator.GenerateDeck();

				var cardCount = deck.Count();

				var distinctCardCount = deck
					.Distinct()
					.Count();

				Assert.Equal(cardCount, distinctCardCount);
			}

			[Fact]
			public void Should_Have_Four_Of_Each_Rank()
			{
				var deckGenerator = new DeckGenerator();
				var deck = deckGenerator.GenerateDeck();

				var rankCounts = deck
					.GroupBy(
						card => card.Rank,
						(rank, cards) => cards.Count());

				Assert.True(rankCounts.All(rankCount => rankCount == 4));
			}

			[Fact]
			public void Should_Have_13_Of_Each_Suit()
			{
				var deckGenerator = new DeckGenerator();
				var deck = deckGenerator.GenerateDeck();

				var suitCounts = deck
					.GroupBy(
						card => card.Suit,
						(rank, cards) => cards.Count());

				Assert.True(suitCounts.All(rankCount => rankCount == 13));
			}

			[Fact]
			public void Should_Have_Sorted_Cards()
			{
				var deckGenerator = new DeckGenerator();
				var deck = deckGenerator.GenerateDeck();

				// We need to scope the deck enumerator separately from where we iterate it,
				// so a foreach won't work. We'll just manually advance the enumerator.
				var deckEnumerator = deck.GetEnumerator();

				// This check depends on the specific way suits and ranks are combined to create cards.
				// That makes it a bad unit test, but I wanted to ensure the cards are sorted for
				// purposes of the demo.
				foreach(var suit in Suit.All)
					foreach(var rank in Rank.All)
					{
						deckEnumerator.MoveNext();
						Assert.Equal(suit, deckEnumerator.Current.Suit);
						Assert.Equal(rank, deckEnumerator.Current.Rank);
					}
			}
		}
	}
}
