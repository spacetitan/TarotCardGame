using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class MarkEnemy : CardStats
{
	[Export] private int value = 0;
	const String MARK_STATUS = "res://Resources/Status/MarkStatus.tres";
	public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
	{
		Mark status = ResourceLoader.Load<Mark>(MARK_STATUS);
		Mark mark = status.Duplicate() as Mark;
		mark.SetStacks(value);

		StatusEffect statusEffect = new StatusEffect(mark);
		statusEffect.Execute(targets);
	}
}

