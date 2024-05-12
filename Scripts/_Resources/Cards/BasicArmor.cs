using Godot;
using System.Collections.Generic;

public partial class BasicArmor : CardStats
{
    public override void ApplyEffects(List<Node2D> targets)
    {
        GuardEffect armor = new GuardEffect(4);
        //block.sound = this.sound;
        armor.Execute(targets);
    }
}
