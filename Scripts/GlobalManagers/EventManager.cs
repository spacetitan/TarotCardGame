using Godot;

public partial class EventManager : Node
{
    public static EventManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public override void _Ready()
	{
		init();
	}

    #region Card Events
	[Signal] public delegate void CardDrawnEventHandler();
	[Signal] public delegate void CardPlayedEventHandler(CardStats card);
	[Signal] public delegate void CardBurnedEventHandler(CardStats card);
	[Signal] public delegate void CardDragStartedEventHandler(CardUI card);
	[Signal] public delegate void CardDragEndedEventHandler();
	[Signal] public delegate void CardAimStartedEventHandler(CardUI card);
	[Signal] public delegate void CardAimEndedEventHandler();
	[Signal] public delegate void CardDescShowEventHandler();
	[Signal] public delegate void CardDescHideEventHandler();
	#endregion

	#region Player Events
	[Signal] public delegate void PlayerHandDrawnEventHandler();
	[Signal] public delegate void PlayerHandDiscardedEventHandler();
	[Signal] public delegate void PlayerDeckReshuffledEventHandler();
	[Signal] public delegate void PlayerAbilityActivateEventHandler();
	[Signal] public delegate void PlayerAbilityusedEventHandler();
	[Signal] public delegate void PlayerAbilityAimStartedEventHandler(Player player);
	[Signal] public delegate void PlayerAbilityAimEndedEventHandler();
    [Signal] public delegate void PlayerTurnStartedEventHandler();
	[Signal] public delegate void PlayerTurnEndedEventHandler();
	[Signal] public delegate void PlayerHitEventHandler();
	[Signal] public delegate void PlayerDiedEventHandler();
	#endregion

	#region Enemy Events
    [Signal] public delegate void EnemyTurnStartedEventHandler();
	[Signal] public delegate void EnemyTurnEndedEventHandler();
    [Signal] public delegate void EnemyActionCompletedEventHandler(Enemy enemy);
    //[Signal] public delegate void EnemyHitEventHandler();
	[Signal] public delegate void EnemyDiedEventHandler(Enemy enemy);
	#endregion 

	#region Battle Events
    [Signal] public delegate void BattleStartedEventHandler();
	[Signal] public delegate void BattleEndedEventHandler(bool win, WinPrize winPrize);
	#endregion
}
