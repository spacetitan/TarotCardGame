using Godot;

public partial class CardInteractState : RefCounted
{
    [Signal]
	public delegate void TransitionRequestedEventHandler(CardInteractState fromState, int toState);
	[Export] public CardStates state = CardStates.NONE;
	public CardUI cardUI;

	public virtual void Enter(){}

	public virtual void Exit(){}

	public virtual void OnInput(InputEvent inputEvent){}

	public virtual void OnGuiInput(InputEvent inputEvent){}

	public virtual void OnMouseEntered(){}

	public virtual void OnMouseExited(){}
}

public partial class CardDefaultState : CardInteractState
{

}

public partial class CardClickedState : CardInteractState
{

}

public partial class CardDraggingState : CardInteractState
{

}

public partial class CardAimingState : CardInteractState
{

}

public partial class CardReleasedState : CardInteractState
{
    
}
