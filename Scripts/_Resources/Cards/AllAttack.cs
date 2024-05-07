using Godot;
using System.Collections.Generic;

public partial class AllAttack : CardStats
{
   public override void ApplyEffects(List<Node2D> targets)
	{
		DamageEffect damage = new DamageEffect();
        damage.amount = 4;
        //damage.sound = this.sound;
        damage.Execute(targets);
	}
}
