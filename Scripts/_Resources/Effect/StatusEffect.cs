using Godot;
using System;
using System.Collections.Generic;

public partial class StatusEffect : Effect
{
    public Status status;

    public StatusEffect(Status status)
	{
        this.status = status;
	}

    public override void Execute(Node2D target)
    {
        if(target == null)
        {
            return;
        }

        if(target is Enemy)
        {
            Enemy enemy = target as Enemy;
            enemy.statusManager.AddStatus((Node2D)target, status);
        }
        else if(target is Player)
        {
            Player player = target as Player;
            player.statusManager.AddStatus((Node2D)target, status);
        }
        AudioManager.instance.sfxPlayer.Play(sound);
    }

    public override void Execute(List<Node2D> targets)
    {
        foreach (Node target in targets)
		{
			if(target == null)
			{
				continue;
			}

			if(target is Enemy)
			{
				Enemy enemy = target as Enemy;
				enemy.statusManager.AddStatus((Node2D)target, status);
			}
			else if(target is Player)
			{
                Player player = target as Player;
				player.statusManager.AddStatus((Node2D)target, status);
			}
			AudioManager.instance.sfxPlayer.Play(sound);
		}
    }
}
