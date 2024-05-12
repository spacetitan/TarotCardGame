using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyArmor : EnemyAction
{
    [Export] int value = 6;

	public override void PerformAction()
	{
		if(enemy == null || target == null)
		{
			GD.Print("Enemy and/or target is null");
			return;
		}

		GuardEffect block = new GuardEffect(value);
		block.Execute(new List<Node2D>(){enemy});

		this.enemy.GetTree().CreateTimer(0.6, false).Timeout += () => 
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.EnemyActionCompleted, enemy);
		};
	}
}
