using Godot;
using System;
using System.Diagnostics;

public partial class BattleUI : CanvasLayer
{
	public Hand hand { get; private set; }
	public ManaUI manaUI { get; private set; }
	public Button endTurnButton { get; private set; }
	public Label remainingCards { get; private set; }
	public PlayerStats playerStats { get; private set; }
	public Button abilityButton { get; private set; }

	public override void _Ready()
	{
		GetSceneNodes();

		this.endTurnButton.Pressed += OnEndTurnButtonPressed;
		this.abilityButton.Pressed += () => 
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.PlayerAbilityActivate);
		};

		EventManager.instance.CardDrawn += SetRemainingCards;
		EventManager.instance.PlayerDeckReshuffled += SetRemainingCards;
		EventManager.instance.PlayerHandDrawn += OnPlayerHandDrawn;
		EventManager.instance.PlayerAbilityused += OnPlayerAbilityUsed;
	}

	private void GetSceneNodes()
	{
		this.hand = GetNode<Hand>("%HBoxContainerHand");
		this.manaUI = GetNode<ManaUI>("%ManaUI");
		this.endTurnButton = GetNode<Button>("%ButtonEndTurn");
		this.remainingCards = GetNode<Label>("%LabelRemaingCards");
		this.abilityButton = GetNode<Button>("%ButtonAbility");
	}

	public void SetPlayerStats(PlayerStats value)
	{
		playerStats = value;
		hand.SetPlayerStats(value);
		manaUI.SetPlayerStats(value);

		SetRemainingCards();
	}

	private void SetRemainingCards()
	{
		this.remainingCards.Text = this.playerStats.deck.cards.Count.ToString();
	}

	private void OnEndTurnButtonPressed()
    {
		EventManager.instance.EmitSignal(EventManager.SignalName.PlayerTurnEnded);
		this.endTurnButton.Disabled = true;
		this.abilityButton.Disabled = true;
    }

	private void OnPlayerHandDrawn()
    {
        this.endTurnButton.Disabled = false;
		this.abilityButton.Disabled = false;
    }
	private void OnPlayerAbilityUsed()
	{
		this.abilityButton.Disabled = true;
	}
}
