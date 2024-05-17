using Godot;
using System;
using System.Collections.Generic;

public partial class GenerateManaEffect : Effect
{
    public int amount = 0;

	public GenerateManaEffect(int value, AudioStream sfx)
	{
		this.amount = value;
		this.sound = sfx;
	}

	public override void Execute(Node2D target)
    {
		if(target == null)
		{
			return;
		}

        if(target is Player)
		{
			Player player = target as Player;
			player.stats.AddMana(amount);
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

            if(target is Player)
			{
				Player player = target as Player;
				player.stats.AddMana(amount);
			}
			AudioManager.instance.sfxPlayer.Play(sound);
		}
    }
}
