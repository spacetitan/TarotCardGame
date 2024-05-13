using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyGuard : EnemyAction
{
	[Export] int value = 15;
    [Export] int hpThreshold = 6;

    bool alreadyUsed = false;

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

        GuardEffect guard = new GuardEffect(value, this.sound);
        guard.Execute(new List<Node2D>(){enemy});

        this.enemy.GetTree().CreateTimer(0.6, false).Timeout += () => 
		{
			EventManager.instance.EmitSignal(EventManager.SignalName.EnemyActionCompleted, enemy.GetIndex());
		};
    }
}
