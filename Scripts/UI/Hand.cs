using Godot;
using System;

public partial class Hand : HBoxContainer
{
    [Export] public CardStats spawnCard;
    const string CARD_UI_PATH = "res://Scenes/CardUI.tscn";
    int cardsPlayedThisTurn = 0;

    public override void _Ready()
	{
		//EventManager.instance.CardPlayed += OnCardPlayed;
        AddCard(spawnCard);
        AddCard(spawnCard);
        //AddCard(spawnCard);
        //AddCard(spawnCard);
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
		//newCardUI.characterStats = this.characterStats;
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

	void OnCardPlayed(CardStats _card)
	{
		cardsPlayedThisTurn++;
	}
}
