using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class AllAttack : CardStats
{
   [Export] private int value = 0;
   public override void ApplyEffects(List<Node2D> targets, Node2D player)
	{
		DamageEffect damage = new DamageEffect(this.value, this.playSFX);
      damage.Execute(targets);
	}
}
