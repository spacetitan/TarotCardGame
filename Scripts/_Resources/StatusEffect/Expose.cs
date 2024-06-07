using Godot;
using System;

[GlobalClass]
public partial class Expose : Status
{
    public override void ApplyStatus(Node target)
    {
        GD.Print("Apply");

        base.ApplyStatus(target);
    }
}
