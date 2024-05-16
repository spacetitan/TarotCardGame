using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class StrAllAttack : CardStats
{
    [Export] private int value = 0;
    public override void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
	{
		DamageEffect damage = new DamageEffect(playerStats.strength + this.value, this.playSFX);
        damage.Execute(targets);
	}
}
