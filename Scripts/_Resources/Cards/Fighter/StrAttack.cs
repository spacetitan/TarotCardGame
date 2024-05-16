using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class StrAttack : CardStats
{
    [Export] private int value = 0;

    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
    {
        DamageEffect damage = new DamageEffect(this.value + playerStats.strength, this.playSFX);
        damage.Execute(targets);
    }
}
