using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class EnemyAttack : EnemyAction
{
    [Export] int value = 7;
    [Export] int numOfAtt = 1;
    [Export] int hpThreshold = 0;

    bool alreadyUsed = false;

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
            GD.Print("Enemy and/or target is null or this ability has already been used");
            return;
        }

        if(this.type == EnemyActionType.CONDITIIONAL)
        {
            alreadyUsed = true;
        }

        Vector2 start = enemy.GlobalPosition;
        Vector2 end = target.GlobalPosition + Vector2.Right * 32;
        DamageEffect damage = new DamageEffect(value);
        List<Node2D> targetList = new List<Node2D>(){target};

        Tween tween = this.enemy.CreateTween().SetTrans(Tween.TransitionType.Quint);
        tween.TweenProperty(enemy, "global_position", end, .4);

        for(int i = 0; i < this.numOfAtt; i++)
        {
            tween.TweenCallback(Callable.From(() => damage.Execute(targetList)));
        }

        tween.TweenInterval(.25);
        tween.TweenProperty(enemy, "global_position", start, .4);

        tween.Finished += () => 
        {
            EventManager.instance.EmitSignal(EventManager.SignalName.EnemyActionCompleted, enemy);
        };
    }
}