using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BasicAttack : CardStats
{
    [Export] private int value = 0;

    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
    {
        DamageEffect damage = new DamageEffect(modifiers.GetModifiedValue(this.value, ModifierType.DMGDEALT), this.playSFX);
        damage.Execute(targets);
    }
}
