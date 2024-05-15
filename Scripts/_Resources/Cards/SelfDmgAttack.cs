using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class SelfDmgAttack : CardStats
{
	[Export] private int value = 0;
	[Export] private int selfDmg = 0;
    public override void ApplyEffects(List<Node2D> targets, Node2D player)
    {
        DamageEffect damage = new DamageEffect(this.value, this.playSFX);
        damage.Execute(targets);

		DamageEffect selfDamage = new DamageEffect(this.selfDmg, this.playSFX);
        selfDamage.Execute(player);
    }
}
