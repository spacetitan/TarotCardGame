using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class MageAbility : Ability
{
    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
    {
        GenerateManaEffect genMana = new GenerateManaEffect(this.value, this.playSFX);
        genMana.Execute(targets);
        this.abilityUsed = true;
        EventManager.instance.EmitSignal(EventManager.SignalName.PlayerAbilityused);
    }
}
