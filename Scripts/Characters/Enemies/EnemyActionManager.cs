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
		SetTarget(this.body.GetTree().GetFirstNodeInGroup("Player") as Node2D);
	}

	private void SetTarget(Node2D value)
	{
		target = value;

		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			enemyAction.target = target;
		}
	}

	private void SetActions(Enemy enemy)
	{
		if(enemy.actions == null || enemy.actions.Length < 1)
		{
			GD.Print("No actions set.");
			return;
		}

		this.enemyActions.Clear();

		foreach (EnemyAction value in enemy.actions)
		{
			this.enemyActions.Add(value);
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

		foreach(EnemyAction value in this.enemyActions)
		{
			action = value;

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

		foreach(EnemyAction value in this.enemyActions)
		{
			action = value;

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
