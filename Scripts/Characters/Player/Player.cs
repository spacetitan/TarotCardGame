using Godot;
using System;

public partial class Player : Node2D
{
	[Export] public PlayerStats stats { get; private set; }
	[Export] public Sprite2D playerSprite { get; private set; }
	[Export] public PlayerStatsUI statsUI { get; private set; }

	public Hand hand{ get; private set; }
	const float HAND_DRAW_INTERVAL = .25F;
	const float HAND_DISCARD_INTERVAL = .15F;

	public override void _Ready()
	{
		GetSceneNodes();

		if(this.stats != null)
		{
			SetPlayerStats(stats);
		}
	}

	private void GetSceneNodes()
	{
		this.playerSprite = GetNode<Sprite2D>("%PlayerSprite");
		this.statsUI = GetNode<PlayerStatsUI>("%PlayerUI");
		this.SetHand(UIManager.instance.battle.hand);
	}

	public void SetPlayerStats(PlayerStats playerStats)
	{
		this.stats = null;
		this.stats = playerStats;
		this.stats.StatsChanged -= UpdateStatsUI;
		this.stats.StatsChanged += UpdateStatsUI;

		this.playerSprite.Texture = this.stats.art;
		UpdateStatsUI();
	}

	private void UpdateStatsUI()
	{
		this.statsUI.UpdateStats(stats);
	}

	public void TakeDamage(int damage)
	{
		if(this.stats.health <=0)
		{
			return;
		}

		//sprite2D.Material = WHITE_SPRITE_MATERIAL;

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(()=>{VFXManager.instance.Shake(this, 16, .15f);}));
		tween.TweenCallback(Callable.From(()=>{stats.TakeDamage(damage);}));
		tween.TweenInterval(0.15f);
		tween.Finished += ()=>
		{
			//sprite2D.Material = null;

			if(stats.health <=0)
			{
				EventManager.instance.EmitSignal(EventManager.SignalName.PlayerDied);
				QueueFree();
			}
		};
	}

	public void StartBattle(PlayerStats playerStats)
	{
		this.stats = playerStats;
		this.stats.deck = (CardPile)playerStats.deck.Duplicate(true);
		this.stats.deck.InitDeck();
		this.stats.deck.Shuffle();
		this.stats.discard = new CardPile();
		StartTurn();
	}

	private void StartTurn()
	{
		this.stats.ResetArmor();
		this.stats.ResetMana();
		DrawCards(this.stats.handSize);
	}

	#region Hand/Card
	public void SetHand(Hand value)
	{
		this.hand = value;
	}

	public void DrawCard()
	{
		ReshuffleDeck();
		hand.AddCard(this.stats.deck.DrawCard());
		ReshuffleDeck();
	}

	public void DrawCards(int amount)
	{
		Tween tween = CreateTween();

		for(int i = 0; i < amount; i++)
		{
			tween.TweenCallback(Callable.From(DrawCard));
			tween.TweenInterval(HAND_DRAW_INTERVAL);
		}

		tween.Finished += () => 
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.PlayerHandDrawn);
		};
	}

	public void DiscardCard(CardStats card)
	{
		stats.discard.AddCard(card);
	}

	public void DiscardHand()
	{
		if(hand.GetChildren().Count < 1)
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.PlayerHandDiscarded);
			return;
		}

		Tween tween = CreateTween();

		foreach(CardUI card in hand.GetChildren())
		{
			tween.TweenCallback(Callable.From(() => stats.discard.AddCard(card.cardStats)));
			tween.TweenCallback(Callable.From(() => hand.DiscardCard(card)));
			tween.TweenInterval(HAND_DISCARD_INTERVAL);
		}

		tween.Finished += () => 
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.PlayerHandDiscarded);
		};
	}

	public void ReshuffleDeck()
	{
		if(!stats.deck.isEmpty())
		{
			return;
		}

		while(!stats.discard.isEmpty())
		{
			stats.deck.AddCard(stats.discard.DrawCard());
		}

		stats.deck.Shuffle();
	}
	#endregion
}
