using Godot;
using System;

public partial class Hand : HBoxContainer
{
    [Export] public CardStats spawnCard;

	public Player player { get; private set; }
	public PlayerStats playerStats { get; private set; }

    const string CARD_UI_PATH = "res://Scenes/UI/CardUI.tscn";
    int cardsPlayedThisTurn = 0;

    public override void _Ready()
	{
		EventManager.instance.CardPlayed += OnCardPlayed;
		EventManager.instance.CardBurned += OnCardBurned;
	}

	public void SetPlayerStats(Player player)
	{
		this.player = player;
		this.playerStats = player.stats;
	}

    public void AddCard(CardStats cardStats)
	{
		var cardScene = GD.Load<PackedScene>(CARD_UI_PATH);
		var newCard = cardScene.Instantiate();
		AddChild(newCard);

		CardUI newCardUI = newCard as CardUI;
		newCardUI.ReturnToHand += OnCardReturnedtoHand;
		newCardUI.SetCardStats(cardStats);
		newCardUI.Reparent(this);
		newCardUI.SetPlayerStats(this.player);
		newCardUI.SetHand(this);

		EventManager.instance.EmitSignal(EventManager.SignalName.CardDrawn);
	}

	public void DiscardCard(CardUI card)
	{
		card.Discard();
	}

	public void DisableHand()
	{
		foreach(CardUI card in GetChildren())
		{
			card.SetDisabled(true);
		}
	}

	void OnCardReturnedtoHand(CardUI child)
	{
		child.Reparent(this);
		int newIndex = Math.Clamp(child.originalIndex - cardsPlayedThisTurn, 0, this.GetChildCount());
        child.originalIndex = newIndex;
		MoveChild(child, newIndex);
	}

	void OnCardPlayed(CardStats card)
	{
		cardsPlayedThisTurn++;

		if(card.isExhaust) { return; }

		this.playerStats.discard.AddCard(card);
	}

	void OnCardBurned(CardStats card)
	{
		this.playerStats.discard.AddCard(card);
	}
}
