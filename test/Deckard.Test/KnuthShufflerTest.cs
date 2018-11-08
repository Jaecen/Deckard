using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace Deckard.Test
{
	public class KnuthShufflerTest
	{
		public class When_Shuffling
		{
			// Use a simple static deck for unit tests to isolate the system under test
			static readonly IEnumerable<Card> Deck = new[]
			{
				new Card(Suit.Hearts, Rank.Jack),
				new Card(Suit.Hearts, Rank.Queen),
				new Card(Suit.Hearts, Rank.King),
				new Card(Suit.Spades, Rank.Jack),
				new Card(Suit.Spades, Rank.Queen),
				new Card(Suit.Spades, Rank.King),
			};

			[Theory]
			[InlineData(typeof(KnuthShuffler))]
			[InlineData(typeof(GuidShuffler))]
			public void Should_Randomize_Order(Type shufflerType)
			{
				// A decent test injection framework like AutoMoq would do this much better, but we'll
				// just hack it in here to avoid pulling in another library.
				var shuffler = (IShuffler)TypeDescriptor.CreateInstance(null, shufflerType, null, null);
				var shuffledDeck = shuffler.Shuffle(Deck);

				var shuffledDeckEnumerator = shuffledDeck.GetEnumerator();

				// It's tough to prove that a sequence is random, so we'll just
				// prove that it's not sorted anymore.
				foreach(var suit in Suit.All)
					foreach(var rank in Rank.All)
					{
						shuffledDeckEnumerator.MoveNext();

						// Exit the test on the first card we find that's out of order
						if(suit != shuffledDeckEnumerator.Current.Suit
							|| rank != shuffledDeckEnumerator.Current.Rank)
						{
							return;
						}
					}

				throw new Xunit.Sdk.XunitException("Sequence is not randomized");
			}

			[Theory]
			[InlineData(typeof(KnuthShuffler))]
			[InlineData(typeof(GuidShuffler))]
			public void Should_Have_Distinct_Cards(Type shufflerType)
			{
				var shuffler = (IShuffler)TypeDescriptor.CreateInstance(null, shufflerType, null, null);
				var shuffledDeck = shuffler.Shuffle(Deck);

				var originalCardCount = Deck.Count();

				var distinctShuffledCardCount = shuffledDeck
					.Distinct()
					.Count();

				Assert.Equal(originalCardCount, distinctShuffledCardCount);
			}
		}
	}
}
