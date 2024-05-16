using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class EnemyAttack : EnemyAction
{
    [Export] int value = 0;
    [Export] int numOfAtt = 1;
    [Export] int hpThreshold = 0;

    bool alreadyUsed = false;

    public override void InitializeAction()
    {
        String intentString;
        if(this.numOfAtt > 1)
        {
            intentString = (value + this.enemy.stats.GetHighestStatNumber()).ToString() + " x " + this.numOfAtt.ToString();
        }
        else
        {
            intentString = (value + this.enemy.stats.GetHighestStatNumber()).ToString();
        }
        this.intent.SetIntent(intentString, null);
    }

    public override bool IsPerformable()
    {
        if(this.type == EnemyActionType.CHANCE)
        {
            return true;
        }

        if(enemy == null || target == null|| alreadyUsed)
        {
            return false;
        }

        bool isLow = enemy.stats.health <= hpThreshold;
        return isLow;
    }

    public override void PerformAction()
    {
        if(enemy == null || target == null || alreadyUsed)
        {
            if(enemy == null)
            {
                GD.Print("Enemy is null");
            }
            else if(target == null)
            {
                GD.Print("target is null");
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

        Vector2 start = enemy.GlobalPosition;
        Vector2 end = target.GlobalPosition + Vector2.Right * 16;
        DamageEffect damage = new DamageEffect(value + this.enemy.stats.GetHighestStatNumber(), this.sound);
        List<Node2D> targetList = new List<Node2D>(){target};

        Tween tween = this.enemy.CreateTween().SetTrans(Tween.TransitionType.Quint);
        tween.TweenProperty(enemy, "global_position", end, .4);

        tween.TweenCallback(Callable.From(() => damage.Execute(targetList)));
        if(this.numOfAtt > 1)
        {
            for(int i = 0; i < this.numOfAtt-1; i++)
            {
                tween.TweenInterval(.35);
                tween.TweenCallback(Callable.From(() => damage.Execute(targetList)));
            }
        }

        tween.TweenInterval(.25);
        tween.TweenProperty(enemy, "global_position", start, .4);

        tween.Finished += () => 
        {
            EventManager.instance.EmitSignal(EventManager.SignalName.EnemyActionCompleted, enemy.GetIndex());
        };
    }
}
