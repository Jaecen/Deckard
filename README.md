# Deckard

This is a card-shuffling demo that just happens to share a name with the [most quit](https://www.imdb.com/title/tt0083658/quotes/qt0378267) Blade Runner in the LAPD.

![My code isn't quite this intense](https://cdn.vox-cdn.com/thumbor/JeGzFccszniU1c7clHnYV50JrGY=/0x0:630x420/1200x480/filters:focal(265x160:365x260)/cdn.vox-cdn.com/uploads/chorus_image/image/56969479/MV5BYjg1Yjk1MTktYzE5Mi00ODkwLWFkZTQtNTYxYTY3ZDVmMWUxXkEyXkFqcGdeQXVyNjUwNzk3NDc_._V1_.0.jpg)

## What's Here?

If you just want to see the shuffling algorithm, you can jump [right here](src/Deckard/IShuffler.cs). However, I created this repo to demo some practices and techniques, so here's what you'll see if you click around a little more:

- The [`Deckard` project](src/Deckard) contains the business logic: definitions of a [Suit](src/Deckard/Suit.cs), [Rank](src/Deckard/Rank.cs), and [Card](src/Deckard/Card.cs); the [deck generation](src/Deckard/DeckGenerator.cs) and [shuffling algorithms](src/Deckard/IShuffler.cs); and a [service aggregator](src/Deckard/Dealer.cs) to pull it all together.
- The [`Deckard.Test` project](test/Deckard.Test) contains unit tests for the deck generator and shufflers.
- The [`Deckard.Cli` prject](src/Deckard.Cli) contains a simple command line interface to run the code. Execute it with either `guid` or `knuth` as the sole parameter to see some high-tech ASCII graphics on your screen.

## How Do I Run It?
Open up `Deckard.sln` in an up-to-date VS2017 instance. Make sure `Deckard.Cli` is set as the default project and hit `Ctrl + F5`. You should see a console window with seven columns of white and red cards.

You can also run the unit tests in Visual Studio and watch them turn green in quick succession! Exciting! Press `Ctrl + R, A` to run all tests in the solution.

## What's Cool About It?

Let me take you on a tour through some of the highlights of what should have been a trivial 10-line app.

First, a bit about the architecture and guiding principles. Through trial and error (mostly error), I've found that one of the best way to manage complexity and enhance flexibility in larger applications is by applying principles from functional programming. This style mostly falls under the banner of [Object Orient Design](https://en.wikipedia.org/wiki/Object-oriented_design), which I've found is very different from what many developers conceptualize when they think of OOP. You might have heard of [the SOLID principles](https://en.wikipedia.org/wiki/SOLID), which were [adapted out of OOD](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod).

In this app, I strictly apply SOLID and other OOD and functional ideas. You'll note that everything is immutable, there is no inheritance, and there is firm separation between classes that contain data and those that expose functionality.

[Shared mutable state is the root of all evil](https://lispcast.com/global-mutable-state/), even outside of multithreaded contexts (where it is, in fact, [the root of all bugs _and_ evil](http://henrikeichenhardt.blogspot.com/2013/06/why-shared-mutable-state-is-root-of-all.html)). To that end, all data outside of the function scope is as immutable as the framework will allow. Properties and fields are `readonly`, and mutable data structures like `List`s and `Dictionary`s are avoided in favor of arrays, `IEnumerable` or the beautiful `System.Collections.Immutable` types.

Take a look at [`Card.cs`](src/Deckard/Card.cs):

```csharp
public class Card
{
	public Suit Suit { get; }
	public Rank Rank { get; }

	public Card(Suit suit, Rank rank)
	{
		Suit = suit ?? throw new ArgumentNullException(nameof(suit));
		Rank = rank ?? throw new ArgumentNullException(nameof(rank));
	}
}
```

Once a `Card` is created, there's no way to modify it. The only way to "change" a card is by creating a new one, which can't affect your callers.

Since there isn't any shared mutable state, the app is built around unidirectional dataflow. All context is pushed down in parameters and returned via the call stack. To enable this, any class that implements behavior (what I call a **service**) only conists of functions and immutable references to other services (injected via DI). The opposite of a service is a **data object** which only carries data and has no behavior.

Take a look at [`Card.cs`](src/Deckard/Card.cs) again. See how it doesn't do anything other than enforce some invariants in the constructor? Now take a look at [`Dealer.cs`](src/Deckard/Dealer.cs) and note how it doesn't store any data, just references to other services.

I could say a lot more on this, but on to the specifics.

[`Card`](src/Deckard/Card.cs), [`Rank`](src/Deckard/Rank.cs), and [`Suit`](src/Deckard/Suit.cs) (along with `IEnumerable<Card>`) make up the basic data structures of this app. `Card` is straightforward. `Rank` and `Suit` are both designed with a locked-in number of instances, expressed as static properties on the class. Because of the private constructor, it's impossible for a dev to create any instances outside of the predefined ones. Restricting the instances in this way elminates who classes of possible errors. It also makes comparisons simple, since we can rely on the default instance comparison that .Net uses instead of having to implement `IEqualityProvider`s for `Rank` and `Suit`.

[`DeckGenerator`](src/Deckard/DeckGenerator.cs) makes use of `yield return` so that only as many cards are generated as are required. The impact is negligible for 52-card decks, but it could generate decks with billions of cards without consuming all of the resources on the machine. Perhaps that would be useful for some statistical simulations.

I created [two implementations behind the `IShuffler` interface](src/Deckard/IShuffler.cs) so that I would have a reason to create an interface. In the CLI app, the implementation is resolved using [Autofac's named services](https://autofaccn.readthedocs.io/en/latest/advanced/keyed-services.html). In the unit tests, we use xUnit's theories and create the instances via reflection.

The [command line app](src/Deckard.Cli/Program.cs) is pretty straightforward, just using Autofac to register the services in the app and then resolve out `Dealer` and its service graph.

Finally, [the unit tests](test/Deckard.Test) confirm basic functionality of `DeckGenerator` and the `IShuffler` implementations. I love using unit tests where they're appropriate, so to that end I try to design all of my services to be mockable. I'm not a fan of test-driven design and I've never found benefit to 100% test coverage. The main value that unit testing delivers for me is in enabling quick feedback loops during initial development and allowing me to refactor mature apps while ensuring I haven't broken any expected functionality.

## Why "Deckard"?
Because the program shuffles a deck of cards. Deck - card. Deckard.

I know. I'm sorry.