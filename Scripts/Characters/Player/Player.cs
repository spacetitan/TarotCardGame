using System.Collections.Generic;
using Godot;

public partial class Player : Node2D
{
	[Export] public PlayerStats stats { get; private set; }
	[Export] public Sprite2D playerSprite { get; private set; }
	[Export] public StatsUI statsUI { get; private set; }

	public Hand hand{ get; private set; }
	const float HAND_DRAW_INTERVAL = .25F;
	const float HAND_DISCARD_INTERVAL = .15F;
	private Material WHITE_SPRITE_MATERIAL = ResourceLoader.Load<Material>("res://Materials/DamageMaterial.tres");
	private Material GREEN_SPRITE_MATERIAL = ResourceLoader.Load<Material>("res://Materials/HealMaterial.tres");

	private bool isDead = false;

	public override void _Ready()
	{
		GetSceneNodes();
		ConnectEventSignals();

		if(this.stats != null)
		{
			SetPlayerStats(stats);
		}
	}

	private void GetSceneNodes()
	{
		this.playerSprite = GetNode<Sprite2D>("%PlayerSprite");
		this.statsUI = GetNode<StatsUI>("%PlayerUI");
		this.SetHand(UIManager.instance.battle.hand);
	}

	private void ConnectEventSignals()
	{
		EventManager.instance.PlayerAbilityActivate += ActivateAbility;
	}

	private void DisconnectSignals()
	{
		this.stats.StatsChanged -= UpdateStatsUI;

		EventManager.instance.PlayerAbilityActivate -= ActivateAbility;
	}

	public void SetPlayerStats(PlayerStats playerStats)
	{
		this.stats = null;
		this.stats = playerStats;
		this.stats.SetPlayer(this);
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
		if(this.stats.health <=0 || isDead)
		{
			return;
		}

		this.playerSprite.Material = WHITE_SPRITE_MATERIAL;

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(()=>{VFXManager.instance.Shake(this, 16, .15f);})); // must be less than interval time
		tween.TweenCallback(Callable.From(()=>{stats.TakeDamage(damage);}));
		tween.TweenInterval(0.17f); // this is the interval time
		tween.Finished += ()=>
		{
			this.playerSprite.Material = null;

			if(stats.health <=0)
			{
				DestroyPlayer();
			}
		};
	}

	public void Heal(int value)
	{
		this.playerSprite.Material = GREEN_SPRITE_MATERIAL;

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(()=>{VFXManager.instance.Shake(this, 16, .15f);}));
		tween.TweenCallback(Callable.From(()=>{stats.Heal(value);}));
		tween.TweenInterval(0.17f); // this is the interval time
		tween.Finished += ()=>
		{
			this.playerSprite.Material = null;

			if(stats.health <=0)
			{
				DestroyPlayer();
			}
		};
	}

	private void DestroyPlayer()
	{
		isDead = true;
		DisconnectSignals();
		EventManager.instance.EmitSignal(EventManager.SignalName.PlayerDied);
		QueueFree();
	}

	private void ActivateAbility()
	{
		if(!this.stats.ability.abilityUsed)
		{
			if(this.stats.ability.targetType == Target.NONE)
			{
				GD.Print("No target type set");
				return;
			}

			if(this.stats.ability.targetType == Target.SINGLE)
			{
				EventManager.instance.EmitSignal(EventManager.SignalName.PlayerAbilityAimStarted, this);
			}
			else
			{
				this.stats.ability.ApplyEffects(this.stats.ability.GetTargets(new List<Node2D>{this}), this.stats);
			}
		}
	}

	public void StartBattle(PlayerStats playerStats)
	{
		this.stats = playerStats;
		this.stats.deck = (CardPile)playerStats.deck.Duplicate(true);
		this.stats.deck.InitDeck();
		this.stats.deck.Shuffle();
		this.stats.discard = new CardPile();
	}

	public void StartTurn()
	{
		this.stats.ResetArmor();
		this.stats.ResetMana();
		this.stats.ability.abilityUsed = false;
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
			tween.TweenCallback(Callable.From(() => this.stats.discard.AddCard(card.cardStats)));
			tween.TweenCallback(Callable.From(() => this.hand.DiscardCard(card)));
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
