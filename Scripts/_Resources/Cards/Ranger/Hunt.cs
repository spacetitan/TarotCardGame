using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Hunt : CardStats
{
	[Export] private int value = 2;
	public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
	{
		int numOfstatus = 0;
		Enemy enemy = targets[0] as Enemy;
		numOfstatus = enemy.statusManager.statuses.Count;

		DamageEffect damageEffect = new DamageEffect(modifiers.GetModifiedValue(playerStats.dexterity + (value * numOfstatus), ModifierType.DMGDEALT), this.playSFX);
		damageEffect.Execute(targets);
	}
}

