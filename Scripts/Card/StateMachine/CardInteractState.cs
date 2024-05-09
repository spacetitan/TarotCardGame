using System.Diagnostics;
using System.Linq;
using Godot;

public partial class CardInteractState : RefCounted
{
    [Signal]
	public delegate void ChangeStateEventHandler(CardInteractState fromState, int toState);
	public CardStates state;
	public CardUI cardUI;

	public CardInteractState(CardStates initialState)
	{
		this.state = initialState;
	}

	public virtual void Enter()
	{
		
	}

	public virtual void Exit(){}

	public virtual void OnInput(InputEvent inputEvent){}

	public virtual void OnGuiInput(InputEvent inputEvent){}

	public virtual void OnMouseEntered(){}

	public virtual void OnMouseExited(){}
}

public partial class CardDefaultState : CardInteractState
{
	public CardStates states = CardStates.DEFAULT;

    public CardDefaultState(CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		if(cardUI.tween != null && cardUI.tween.IsRunning())
		{
			cardUI.tween.Kill();
		}

		cardUI.EmitSignal(CardUI.SignalName.ReturnToHand, this.cardUI);
		cardUI.PivotOffset = Vector2.Zero;
	}

	public override void Exit(){}

	public override void OnInput(InputEvent inputEvent){}

	public override void OnGuiInput(InputEvent inputEvent)
	{
		if(inputEvent.IsActionPressed("LeftMouse"))
		{
			cardUI.PivotOffset = cardUI.GetGlobalMousePosition() - cardUI.GlobalPosition;
			EmitSignal(SignalName.ChangeState, this, (int)CardStates.CLICKED);
		}
	}

	public override void OnMouseEntered(){}

	public override void OnMouseExited(){}
}

public partial class CardClickedState : CardInteractState
{
	public CardStates states = CardStates.CLICKED;

    public CardClickedState(CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		this.cardUI.playArea.Monitorable = true;
	}

	public override void Exit(){}

	public override void OnInput(InputEvent inputEvent)
	{
		if(inputEvent is InputEventMouseMotion)
		{
			EmitSignal(SignalName.ChangeState, this, (int)CardStates.DRAGGING);
		}
	}

	public override void OnGuiInput(InputEvent inputEvent){}

	public override void OnMouseEntered(){}

	public override void OnMouseExited(){}
}

public partial class CardDraggingState : CardInteractState
{
	public CardStates states = CardStates.DRAGGING;
	const float DRAG_MINIMUM_THRESHOLD = .05f;
	bool minimumDragTimeElapsed = false;

    public CardDraggingState(CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		Node uilayer = cardUI.GetTree().GetFirstNodeInGroup("BattleUI");

		if(uilayer != null)
		{
			cardUI.Reparent(uilayer);
		}

		EventManager.instance.EmitSignal(EventManager.SignalName.CardDragStarted, cardUI);
		this.minimumDragTimeElapsed = false;
		SceneTreeTimer thresholdTimer = cardUI.GetTree().CreateTimer(DRAG_MINIMUM_THRESHOLD, false);
		thresholdTimer.Timeout += () => {minimumDragTimeElapsed = true;};
	}

	public override void Exit()
	{
		EventManager.instance.EmitSignal(EventManager.SignalName.CardDragEnded);
	}

	public override void OnInput(InputEvent inputEvent)
	{
		bool singleTargeted = cardUI.cardStats.isSingleTargeted();
		bool mouseMotion = inputEvent is InputEventMouseMotion;
		bool cancel = inputEvent.IsActionPressed("RightMouse");
		bool confirm = inputEvent.IsActionReleased("LeftMouse") || inputEvent.IsActionPressed("LeftMouse");

		if(singleTargeted && mouseMotion && cardUI.targets.Count > 0)
		{
			EmitSignal(SignalName.ChangeState, this, (int)CardStates.AIMING);
			return;
		}

		if(mouseMotion)
		{
			this.cardUI.GlobalPosition = cardUI.GetGlobalMousePosition() - cardUI.PivotOffset;
		}

		if(cancel)
		{
			EmitSignal(SignalName.ChangeState, this, (int)CardStates.DEFAULT);
			this.cardUI.Burn();
		}
		else if(this.minimumDragTimeElapsed && confirm)
		{
			cardUI.GetViewport().SetInputAsHandled();
			EmitSignal(SignalName.ChangeState, this, (int)CardStates.RELEASED);
		}
	}

	public override void OnGuiInput(InputEvent inputEvent){}

	public override void OnMouseEntered(){}

	public override void OnMouseExited(){}
}

public partial class CardAimingState : CardInteractState
{
	public CardStates states = CardStates.AIMING;
	const float MOUSE_Y_SNAPBACK_THRESHOLD = 525;

    public CardAimingState(CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		cardUI.targets.Clear();

		Vector2 offset = new Vector2(cardUI.hand.Size.X /2, -cardUI.hand.Size.Y + 10);
		offset.X -= cardUI.Size.X / 2;
		cardUI.AnimateToPosition(cardUI.hand.GlobalPosition + offset, 0.2f);
		
		cardUI.playArea.Monitorable = false;
		EventManager.instance.EmitSignal(EventManager.SignalName.CardAimStarted, cardUI);
	}

	public override void Exit()
	{
		EventManager.instance.EmitSignal(EventManager.SignalName.CardAimEnded);
	}

	public override void OnInput(InputEvent inputEvent)
	{
		bool mouseMotion = inputEvent is InputEventMouseMotion;
		bool mouseAtBotton = cardUI.hand.GlobalPosition.Y > MOUSE_Y_SNAPBACK_THRESHOLD;

		if((mouseMotion && mouseAtBotton) || inputEvent.IsActionPressed("RightMouse"))
		{
			EmitSignal(SignalName.ChangeState, this, (int)CardStates.DEFAULT);
		}
		else if(inputEvent.IsActionReleased("LeftMouse") || inputEvent.IsActionPressed("LeftMouse"))
		{
			this.cardUI.GetViewport().SetInputAsHandled();
			EmitSignal(SignalName.ChangeState, this, (int)CardStates.RELEASED);
		}
	}

	public override void OnGuiInput(InputEvent inputEvent){}

	public override void OnMouseEntered(){}

	public override void OnMouseExited(){}
}

public partial class CardReleasedState : CardInteractState
{
	public CardStates states = CardStates.RELEASED;
	bool played = false;

    public CardReleasedState(CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		played = false;

		if(cardUI.burner != null)
		{
			GD.Print("Burning Card");
			cardUI.Burn();
			return;
		}

		if(cardUI.targets.Any() && cardUI.isPlayable)
		{
			played = true;
			cardUI.Play();
		}
		else if(!cardUI.isPlayable)
		{
			GD.Print("Card is not playable");
		}
		else
		{
			GD.Print("No Targets!");
		}
	}

	public override void Exit(){}

	public override void OnInput(InputEvent inputEvent)
	{
		if(played) {return;}

		EmitSignal(SignalName.ChangeState, this, (int)CardStates.DEFAULT);
	}

	public override void OnGuiInput(InputEvent inputEvent){}

	public override void OnMouseEntered(){}

	public override void OnMouseExited(){}
}
