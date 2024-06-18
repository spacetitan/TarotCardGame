using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class StrAttack : CardStats
{
    [Export] private int value = 0;

    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
    {
        DamageEffect damage = new DamageEffect(modifiers.GetModifiedValue(this.value + playerStats.strength, ModifierType.DMGDEALT), this.playSFX);
        damage.Execute(targets);
    }
}
