using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class RangerAbility : Ability
{
    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
    {
        DamageEffect damage = new DamageEffect(this.value, this.playSFX);
        damage.Execute(targets);
        this.abilityUsed = true;
        EventManager.instance.EmitSignal(EventManager.SignalName.PlayerAbilityused);
    }
}
