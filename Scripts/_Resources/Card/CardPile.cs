using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class CardPile : Resource
{
    [Signal] public delegate void CardPileSizeChangedEventHandler(int cardsAmount);
    [Export] public CardStats[] cardStates;
    public List<CardStats> cards{ get; private set; } = new List<CardStats>();
    private Random rand;

    public bool isEmpty()
    {
        return cards == null || cards.Count < 1;
    }

    public CardStats DrawCard()
    {
        CardStats drawnCard = cards[0];

        this.cards.RemoveAt(0);

        EmitSignal(SignalName.CardPileSizeChanged, cards.Count);
        return drawnCard;
    }

    public void AddCard(CardStats card)
    {
        this.cards.Add(card);
        EmitSignal(SignalName.CardPileSizeChanged, cards.Count);
    }

    public void Shuffle()
    {
        this.rand = new Random();
        this.cards = this.cards.OrderBy(x => rand.Next()).ToList();
    }

    public void Clear()
    {
        this.cards.Clear();
        EmitSignal(SignalName.CardPileSizeChanged, cards.Count);
    }

    public void InitDeck()
    {
        this.cards.Clear();

        foreach(CardStats card in cardStates)
        {
            this.cards.Add(card);
        }
    }
}
