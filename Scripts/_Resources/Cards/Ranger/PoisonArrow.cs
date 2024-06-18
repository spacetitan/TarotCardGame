using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PoisonArrow : CardStats
{
	[Export] private int value = 0;

	const String POISON_STATUS = "res://Resources/Status/PoisonStatus.tres";

	public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
	{
		DamageEffect damageEffect = new DamageEffect(modifiers.GetModifiedValue(playerStats.dexterity, ModifierType.DMGDEALT), this.playSFX);
        damageEffect.Execute(targets);

		Poison status = ResourceLoader.Load<Poison>(POISON_STATUS);
		Poison poison = status.Duplicate() as Poison;
		poison.SetStacks(value);

		StatusEffect statusEffect = new StatusEffect(poison);
		statusEffect.status = poison;
		statusEffect.Execute(targets);
	}
}

