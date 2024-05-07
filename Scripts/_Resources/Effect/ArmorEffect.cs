using Godot;
using System.Collections.Generic;

public partial class ArmorEffect : Effect
{
    public int amount = 0;

	public ArmorEffect(int value)
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

			// if(target is Enemy)
			// {
			// 	Enemy enemy = (Enemy)target;
			// 	enemy.stats.AddArmor(amount);
			// }
			// else 
            if(target is Player)
			{
				Player player = target as Player;
				player.stats.AddArmor(amount);
			}
			AudioManager.instance.sfxPlayer.Play(sound);
		}
    }
}
