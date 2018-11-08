using System;
using System.Linq;
using Autofac;

namespace Deckard.Cli
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Use the shuffler indicated in the command line arguments, or default to "knuth" if none was provided
			var selectedShuffler = args.Length < 1 || string.IsNullOrEmpty(args[0])
				? "knuth"
				: args[0];

			// Validate the selected shuffler
			if(!new[] { "guid", "knuth" }.Contains(selectedShuffler))
			{
				Console.WriteLine($"You must specify either \"guid\" or \"knuth\". The default is \"knuth\".");
				return;
			}

			// Build our DI registrations
			var builder = new ContainerBuilder();

			builder
				.Register(c => new DeckGenerator());

			builder
				.Register(c => new GuidShuffler())
				.Named<IShuffler>("guid");

			builder
				.Register(c => new KnuthShuffler())
				.Named<IShuffler>("knuth");

			builder
				.Register(c => new Dealer(
					deckGenerator: c.Resolve<DeckGenerator>(),
					shuffler: c.ResolveNamed<IShuffler>(selectedShuffler)));

			// Resolve out a Dealer and deal some shuffled cards
			using(var container = builder.Build())
			{
				Console.WriteLine($"Using the \"{selectedShuffler}\" shuffler.");

				var dealer = container.Resolve<Dealer>();
				var cards = dealer.Deal();

				var index = 1;
				foreach(var card in cards)
				{
					RenderCard(card);
					Console.Write(" ");

					// Write out seven columns, like solitaire
					if(index % 7 == 0)
						Console.WriteLine();

					index++;
				}
			}

			Console.WriteLine();
		}

		static void RenderCard(Card card)
		{
			// Toggle the color based on the suit
			var originalColor = Console.ForegroundColor;
			if(card.Suit == Suit.Hearts || card.Suit == Suit.Diamonds)
				Console.ForegroundColor = ConsoleColor.Red;

			Console.Write($"{card.Rank.Symbol,2}{card.Suit.Symbol}");

			Console.ForegroundColor = originalColor;
		}
	}
}
