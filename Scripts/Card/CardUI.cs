using System.Collections.Generic;
using System.Diagnostics;
using Godot;

[GlobalClass]
public partial class CardUI : Control
{
	[Signal] public delegate void ReturnToHandEventHandler(CardUI card);
	public int originalIndex = 0;
	[Export] public CardStats cardStats{ get; private set; }
	private PlayerStats playerStats;
	private CardStateMachine cardStateMachine;

	#region Scene Nodes
	public Panel backgroundPanel { get; private set; }
	private TextureRect cardArt;
	private Label cardName;
	private Label cardTags;
	private Label cardDesc;
	private Label cardMana;
	public Area2D playArea { get; private set; }
	public HBoxContainer hand { get; private set; }
	public CardToolTip cardToolTip { get; private set; }
	public ColorRect cardTypeColorRect { get; private set; }
	public ColorRect cardTypeTTColorRect { get; private set; }
	#endregion

	public StyleBox cardstyleDefault { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/StyleBox/Card/CardDefault.tres");
	public StyleBox cardstyleDrag { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/StyleBox/Card/CardDragging.tres");
	public StyleBox cardstyleHover { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/StyleBox/Card/CardHover.tres");
	public StyleBox cardstyleDisabled { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/StyleBox/Card/CardDisabled.tres");

	public List<Node2D> targets = new List<Node2D>();
	public Node2D burner = null;
	public Tween tween;

	public bool isPlayable{ get; private set;} = false;
	public bool isDisabled{ get; private set;} = true;

	public override void _Ready()
	{
		GetSceneNodes();
		ConnectEventSignals();

		this.originalIndex = this.GetIndex();
		
		if(this.cardStats != null)
		{
			this.SetCardStats(this.cardStats);
		}

		cardStateMachine = new CardStateMachine(this);
	}

	private void GetSceneNodes()
	{
		this.backgroundPanel = GetNode<Panel>("%PanelBackground");
		this.cardArt = GetNode<TextureRect>("%TextureRectCardArt");
		this.cardName = GetNode<Label>("%LabelCardName");
		this.cardTags = GetNode<Label>("%LabelCardTags");
		this.cardDesc = GetNode<Label>("%LabelCardDesc");
		this.cardMana = GetNode<Label>("%LabelCardMana");
		this.playArea = GetNode<Area2D>("%Area2DPlayArea");
		this.cardToolTip = GetNode<CardToolTip>("%PanelTooltip");
		this.cardTypeColorRect = GetNode<ColorRect>("%ColorRectCardType");
		this.cardTypeTTColorRect = GetNode<ColorRect>("%ColorRectTTCardType");
	}

	public void SetCardStats(CardStats cardStats)
	{
		this.cardStats = cardStats;
		SetCardUi(this.cardStats);
	}

	public void SetCardUi(CardStats cardStats)
	{
		this.cardArt.Texture = cardStats.cardArt;
		this.cardName.Text = cardStats.cardName;
		this.cardTags.Text = "Type: " + cardStats.cardType.ToString() + " Tag: " + cardStats.cardTag.ToString();
		this.cardDesc.Text = cardStats.cardDesc;
		this.cardMana.Text = "Cost: " + cardStats.cardCost.ToString() + " Gen: " + cardStats.cardGen.ToString();

		this.cardToolTip.SetToolTip(cardStats);

		SetCardColor();
	}

	public void SetCardColor()
	{
		Color color = ToolsManager.GetCharColor(this.cardStats.cardType);
		this.cardTypeColorRect.Color = color;
		this.cardTypeTTColorRect.Color = color;
	}

	public void SetPlayerStats(PlayerStats value)
	{
		this.playerStats = value;
		this.playerStats.StatsChanged += OnStatsChanged;
		SetPlayable(playerStats.CanPlayCard(this));
	}

	public void SetHand(HBoxContainer value)
	{
		this.hand = value;
	}

	private void OnStatsChanged()
	{
		this.isPlayable = playerStats.CanPlayCard(this);
		SetPlayable(playerStats.CanPlayCard(this));
	}

	public void SetPlayable(bool value)
	{
		this.isPlayable = value;
		if(!isPlayable)
		{
			this.cardMana.AddThemeColorOverride("font_color", Colors.Red);
			this.cardArt.Modulate = new Color(1,1,1,.05f);
		}
		else
		{
			this.cardMana.RemoveThemeColorOverride("font_color");
			this.cardArt.Modulate = new Color(1,1,1,1);
		}
	}
	public void SetDisabled(bool value)
	{
		this.isDisabled = value;
	}
	public void Play()
	{
		if(this.cardStats == null)
		{
			return;
		}

		cardStats.Play(playerStats, targets);
		DestroyCard();
	}
	public void Burn()
	{
		if(this.cardStats == null)
		{
			return;
		}

		cardStats.Burn(playerStats);
		DestroyCard();
	}
	public void Discard()
	{
		this.playerStats.discard.AddCard(this.cardStats);
		DestroyCard();
	}
	public void DestroyCard()
	{
		DisconnectEventSignals();
		QueueFree();
	}

	public override void _Input(InputEvent inputEvent)
	{
		this.cardStateMachine.OnInput(inputEvent);
	}

	void OnGuiInput(InputEvent inputEvent)
	{
		this.cardStateMachine.OnGuiInput(inputEvent);
	}

	void OnMouseEntered()
	{
		this.cardStateMachine.OnMouseEntered();
	}

	void OnMouseExited()
	{
		this.cardStateMachine.OnMouseExited();
	}

	void OnPlayAreaEntered(Area2D area)
	{
		if(area.IsInGroup("Burner"))
		{
			burner = area;
		}

		if(!targets.Contains(area))
		{
			targets.Add(area as Node2D);
		}
	}
	void OnPlayAreaExited(Area2D area)
	{
		burner = null;
		targets.Remove(area as Node2D);
	}

	void OnCardDragOrAimingStarted(CardUI card)
	{
		if(card == this)
		{
			return;
		}

		this.isDisabled = true;
	}

	void OnCardDragOrAimingEnded()
	{
		this.isDisabled = false;
	}

	public void AnimateToPosition(Vector2 newPosition, float duration)
	{
		tween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(this, "global_position", newPosition, duration);
	}

	#region Event Signal methods
	private void ConnectEventSignals()
	{
		EventManager.instance.CardDragStarted += OnCardDragOrAimingStarted;
		EventManager.instance.CardDragEnded += OnCardDragOrAimingEnded;
		EventManager.instance.CardAimStarted += OnCardDragOrAimingStarted;
		EventManager.instance.CardAimEnded += OnCardDragOrAimingEnded;
		//EventManager.instance.CardPlayed += OnCardPlayed;
	}
	private void DisconnectEventSignals()
	{
		this.playerStats.StatsChanged -= OnStatsChanged;
		EventManager.instance.CardDragStarted -= OnCardDragOrAimingStarted;
		EventManager.instance.CardDragEnded -= OnCardDragOrAimingEnded;
		EventManager.instance.CardAimStarted -= OnCardDragOrAimingStarted;
		EventManager.instance.CardAimEnded -= OnCardDragOrAimingEnded;
		//EventManager.instance.CardPlayed -= OnCardPlayed;
	}
	#endregion
}
