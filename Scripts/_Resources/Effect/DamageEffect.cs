using Godot;
using System.Collections.Generic;

public partial class DamageEffect : Effect
{
    public int amount = 0;

	public DamageEffect(int value, AudioStream sfx)
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

		if(target is Enemy)
		{
			Enemy enemy = (Enemy)target;
			enemy.TakeDamage(amount);
		}
		else if(target is Player)
		{
			Player player = (Player)target;
			player.TakeDamage(amount);
		}
		
		if(this.sound != null) { AudioManager.instance.sfxPlayer.Play(sound); }
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
				Enemy enemy = (Enemy)target;
				enemy.TakeDamage(amount);
			}
			else if(target is Player)
			{
				Player player = (Player)target;
				player.TakeDamage(amount);
			}
			AudioManager.instance.sfxPlayer.Play(sound);
		}
    }
}
