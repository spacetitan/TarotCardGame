using Godot;
using System.Collections.Generic;

public partial class DamageEffect : Effect
{
    public int amount = 0;

	public DamageEffect(int value)
	{
		amount = value;
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
