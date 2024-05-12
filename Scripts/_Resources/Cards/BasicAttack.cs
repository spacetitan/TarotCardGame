using Godot;
using System;
using System.Collections.Generic;

public partial class BasicAttack : CardStats
{
    public override void ApplyEffects(List<Node2D> targets)
    {
        DamageEffect damage = new DamageEffect(6);
        //damage.sound = this.sound;
        damage.Execute(targets);
    }
}
