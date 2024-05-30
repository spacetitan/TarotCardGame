using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyGuard : EnemyAction
{
	[Export] int value = 0;
    [Export] int hpThreshold = 0;

    bool alreadyUsed = false;

    public override void InitializeAction()
    {
        String intentString = (value + this.enemy.stats.strength).ToString();
        this.intent.SetIntent(intentString, null);
    }

    public override bool IsPerformable()
    {
        if(this.type == EnemyActionType.CHANCE)
        {
            return true;
        }

        if(enemy == null || alreadyUsed)
        {
            return false;
        }

        bool isLow = enemy.stats.health <= hpThreshold;
        return isLow;
    }

    public override void PerformAction()
    {
        if(enemy == null || alreadyUsed)
        {
            if(enemy == null)
            {
                GD.Print("Enemy is null");
            }
            else
            {
                GD.Print("this ability has already been used");
            }
            return;
        }

        if(this.type == EnemyActionType.CONDITIIONAL)
        {
            alreadyUsed = true;
        }

        GuardEffect guard = new GuardEffect(value + this.enemy.stats.strength, this.sound);
        guard.Execute(new List<Node2D>(){enemy});

        this.enemy.GetTree().CreateTimer(0.6, false).Timeout += () => 
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.EnemyActionCompleted, enemy);
		};
    }
}
