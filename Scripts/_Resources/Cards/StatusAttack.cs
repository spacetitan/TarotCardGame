using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class StatusAttack : CardStats
{
	[Export] private int value = 0;
	[Export] private int duration = 0;

    const String EXPOSED_STATUS = "res://Resources/Status/ExposeStatus.tres";

    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
    {
        DamageEffect damage = new DamageEffect(this.value, this.playSFX);
        damage.Execute(targets);

        Expose status = ResourceLoader.Load<Expose>(EXPOSED_STATUS);
        Expose expose = status.Duplicate() as Expose;
        expose.SetDuration(this.duration);

        StatusEffect statusEffect = new StatusEffect(this.duration);
        statusEffect.status = expose;
        statusEffect.Execute(targets);
    }
}
