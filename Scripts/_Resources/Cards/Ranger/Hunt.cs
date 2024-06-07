using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Hunt : CardStats
{
	[Export] private int value = 2;
	public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
	{
		int numOfstatus = 0;
		Enemy enemy = targets[0] as Enemy;
		numOfstatus = enemy.statusManager.GetAllStatuses().Count;

		DamageEffect damageEffect = new DamageEffect(playerStats.dexterity + (value * numOfstatus), this.playSFX);
		damageEffect.Execute(targets);
	}
}

