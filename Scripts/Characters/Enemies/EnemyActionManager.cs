using System.Collections.Generic;
using Godot;

public partial class EnemyActionManager : Node
{
	public Enemy body { get; private set;}
	public Node2D target { get; private set;}
	public List<EnemyAction> enemyActions{ get; private set; } = new List<EnemyAction>();

	private int totalWeight = 0;

	public EnemyActionManager(Enemy enemy)
	{
		this.body = enemy;
		SetActions(this.body);
		SetTargets(this.body.GetTree().GetFirstNodeInGroup("Player") as Node2D);
	}

	private void SetTargets(Node2D value)
	{
		target = value;

		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			enemyAction.target = target;
		}
	}

	private void SetActions(Enemy enemy)
	{
		if(enemy.stats.actions == null || enemy.stats.actions.Length < 1)
		{
			GD.Print("No actions set.");
			return;
		}

		this.enemyActions.Clear();

		foreach (EnemyAction enemyAction in enemy.stats.actions)
		{
			enemyAction.enemy = this.body;
			enemyAction.InitializeAction();
			this.enemyActions.Add(enemyAction);
		}

		SetupChances();
	}

	private void SetupChances()
	{
		this.totalWeight = 0;

		EnemyAction action;
		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			action = enemyAction;

			if((action == null) || action.type != EnemyActionType.CHANCE)
			{
				continue;
			}

			totalWeight += action.chanceWeight;
			action.accumulatedweight = totalWeight;
		}
	}

	public EnemyAction GetAction()
	{
		EnemyAction action = GetFirstConditionalAction();

		if(action != null)
		{
			return action;
		}

		return GetChanceBasedAction();
	}

	public EnemyAction GetFirstConditionalAction()
	{
		EnemyAction action;

		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			action = enemyAction;

			if((action == null) || action.type != EnemyActionType.CONDITIIONAL)
			{
				continue;
			}

			if(action.IsPerformable())
			{
				return action;
			}
		}
		return null;
	}

	public EnemyAction GetChanceBasedAction()
	{
		EnemyAction action;
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		int roll = rng.RandiRange(0, totalWeight-1);
		
		if(roll == totalWeight)
		{
			GD.Print("Out of bounds");
		}

		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			action = enemyAction;

			if((action == null) || action.type != EnemyActionType.CHANCE)
			{
				continue;
			}

			if(action.accumulatedweight > roll)
			{
				return action;
			}
		}
		GD.Print("No Action, returning null");
		return null;
	}
}
