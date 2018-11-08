using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Deckard
{
	public interface IShuffler
	{
		IEnumerable<Card> Shuffle(IEnumerable<Card> deck);
	}

	// Cheesy but dead-simple shuffler implmentation
	public class GuidShuffler : IShuffler
	{
		public IEnumerable<Card> Shuffle(IEnumerable<Card> deck)
			=> deck
				.Select(card => (guid: Guid.NewGuid(), card))
				.OrderBy(taggedCard => taggedCard.guid)
				.Select(taggedCard => taggedCard.card);
	}

	// Traditional Knuth shuffle
	public class KnuthShuffler : IShuffler
	{
		public IEnumerable<Card> Shuffle(IEnumerable<Card> deck)
		{
			// Since IEnumerables are immutable and can only be read forward, we'll
			// shuffle the indicies first, then assign cards to a new collection
			// based on the order of those indicies.
			var cardIndicies = Enumerable
				.Range(0, deck.Count())
				.ToArray();

			// Generate an array of random indicies constrained to how many elements will
			// be left in the indicies array at that index. We'll just use bytes and assume
			// decks won't ever exceed 255 cards.
			var rng = RandomNumberGenerator.Create();
			var randomIndicies = new byte[cardIndicies.Length];
			rng.GetBytes(randomIndicies);

			// Go through each index, from the end to the front, taking a random index that
			// we haven't gotten to yet and swapping it with the last element.
			var newIndicies = new int[cardIndicies.Length];
			for(var index = cardIndicies.Length - 1; index >= 1; index--)
			{
				var randomIndex = randomIndicies[index] % index;

				var swapTemp = cardIndicies[index];
				cardIndicies[index] = cardIndicies[randomIndex];
				cardIndicies[randomIndex] = swapTemp;
			}

			// Run through the now shuffled indicies and return the corresponding card. This
			// LINQ is a little ugly, but I'm hoping MS did a better job optimizing the lookups
			// than the naive implementation I'd put here.
			return cardIndicies
				.Zip(deck, (index, card) => (index, card))
				.OrderBy(taggedCard => taggedCard.index)
				.Select(taggedCard => taggedCard.card);
		}
	}
}
