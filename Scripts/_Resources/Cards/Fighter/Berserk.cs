using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Berserk : CardStats
{
	[Export] private int value = 0;
	const String MUSCLE_STATUS = "res://Resources/Status/MuscleStatus.tres";
	public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
	{
		Muscle status = ResourceLoader.Load<Muscle>(MUSCLE_STATUS);
		Muscle muscle = status.Duplicate() as Muscle;
		muscle.SetDuration(value);

		StatusEffect statusEffect = new StatusEffect(muscle);
		statusEffect.Execute(targets);
	}
}

