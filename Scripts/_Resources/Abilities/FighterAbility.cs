using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class FighterAbility : Ability
{
    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
    {
        GuardEffect armor = new GuardEffect(this.value, this.playSFX);
        armor.Execute(targets);
        this.abilityUsed = true;
        EventManager.instance.EmitSignal(EventManager.SignalName.PlayerAbilityused);
    }
}
