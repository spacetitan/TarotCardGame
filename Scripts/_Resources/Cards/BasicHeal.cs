using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class BasicHeal : CardStats
{
    [Export] private int value = 0;
    public override void ApplyEffects(List<Node2D> targets, Node2D player)
    {
        HealEffect heal = new HealEffect(this.value, this.playSFX);
        heal.Execute(targets);
    }
}
