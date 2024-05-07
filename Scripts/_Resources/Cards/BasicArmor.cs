using Godot;
using System.Collections.Generic;

public partial class BasicArmor : CardStats
{
    public override void ApplyEffects(List<Node2D> targets)
    {
        ArmorEffect armor = new ArmorEffect(4);
        //block.sound = this.sound;
        armor.Execute(targets);
    }
}
