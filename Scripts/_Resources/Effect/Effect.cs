using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Effect : RefCounted
{
    [Export] public AudioStream sound;
    public virtual void Execute(List<Node2D> target){}
}
