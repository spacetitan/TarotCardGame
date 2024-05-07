using System.Collections.Generic;
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
	private TextureRect cardArt;
	private Label cardName;
	private Label cardTags;
	private Label cardDesc;
	private Label cardMana;
	public Area2D playArea;
	#endregion

	public List<Node2D> targets = new List<Node2D>();
	public Node2D burner = null;
	public Tween tween;

	public bool isPlayable{ get; private set;} = false;
	public bool isDisabled{ get; private set;} = true;

	public override void _Ready()
	{
		cardStateMachine = new CardStateMachine(this);
		GetSceneNodes();
		ConnectEventSignals();

		this.originalIndex = this.GetIndex();
		
		if(this.cardStats != null)
		{
			this.SetCardStats(this.cardStats);
		}
	}

	private void GetSceneNodes()
	{
		this.cardArt = GetNode<TextureRect>("%TextureRectCardArt");
		this.cardName = GetNode<Label>("%LabelCardName");
		this.cardTags = GetNode<Label>("%LabelCardTags");
		this.cardDesc = GetNode<Label>("%LabelCardDesc");
		this.cardMana = GetNode<Label>("%LabelCardMana");
		this.playArea = GetNode<Area2D>("%Area2DPlayArea");
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
	}

	public void SetPlayerStats(PlayerStats value)
	{
		this.playerStats = value;
		this.playerStats.StatsChanged += OnStatsChanged;
		SetPlayable(playerStats.CanPlayCard(this));
	}

	private void OnStatsChanged()
	{
		this.isPlayable = playerStats.CanPlayCard(this);
		SetPlayable(playerStats.CanPlayCard(this));
	}

	public void SetPlayable(bool value)
	{
		this.isPlayable = value;
		// if(!isPlayable)
		// {
		// 	costLabel.AddThemeColorOverride("font_color", Colors.Red);
		// 	icon.Modulate = new Color(1,1,1,.05f);
		// }
		// else
		// {
		// 	costLabel.RemoveThemeColorOverride("font_color");
		// 	icon.Modulate = new Color(1,1,1,1);
		// }
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
	public void Discard(){}
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
		//GD.Print("Adding Target");
		if(area.IsInGroup("Burner"))
		{
			//GD.Print("Adding burner");
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
		EventManager.instance.CardDragStarted -= OnCardDragOrAimingStarted;
		EventManager.instance.CardDragEnded -= OnCardDragOrAimingEnded;
		EventManager.instance.CardAimStarted -= OnCardDragOrAimingStarted;
		EventManager.instance.CardAimEnded -= OnCardDragOrAimingEnded;
		//EventManager.instance.CardPlayed -= OnCardPlayed;
	}
	#endregion
}
