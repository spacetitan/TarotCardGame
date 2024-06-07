using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PoisonArrow : CardStats
{
	[Export] private int value = 0;

	const String POISON_STATUS = "res://Resources/Status/PoisonStatus.tres";

	public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
	{
		DamageEffect damageEffect = new DamageEffect(playerStats.dexterity, this.playSFX);
        damageEffect.Execute(targets);

		Poison status = ResourceLoader.Load<Poison>(POISON_STATUS);
		Poison poison = status.Duplicate() as Poison;
		poison.SetStacks(value);

		StatusEffect statusEffect = new StatusEffect(this.value);
		statusEffect.status = poison;
		statusEffect.Execute(targets);
	}
}

