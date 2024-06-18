using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class BasicGuard : CardStats
{
    [Export] private int value = 0;

    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
    {
        GuardEffect armor = new GuardEffect(this.value, this.playSFX);
        armor.Execute(targets);
    }
}
