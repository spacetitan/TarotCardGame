using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class CardStateMachine : Node
{
	[Export] CardStates initialState;

	List<CardInteractState> cardStates = new List<CardInteractState>();

    public void InitialStateMachine()
    {
        this.cardStates.Add(new CardDefaultState());
		this.cardStates.Add(new CardClickedState());
		this.cardStates.Add(new CardDraggingState());
		this.cardStates.Add(new CardAimingState());
		this.cardStates.Add(new CardReleasedState());
    }
}
