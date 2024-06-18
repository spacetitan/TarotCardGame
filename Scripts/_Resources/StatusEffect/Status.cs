using Godot;
using System;

[GlobalClass]
public partial class Status : Resource
{
    [Signal] public delegate void StatusAppliedEventHandler(Status status);
	[Signal] public delegate void StatusChangedEventHandler();

    [ExportGroup("Status Data")]
    [Export] public StringName id { get; private set; }
    [Export] public StatusType type { get; private set; }
    [Export] public StatusStackType stackType { get; private set; }
    [Export] public bool canExpire { get; private set; }
    [Export] public int duration { get; private set; }
    [Export] public int stacks { get; private set; }

    [ExportGroup("Status External")]
    [Export] public Texture2D statusIcon { get; private set; }
    [Export] public String desc { get; private set; }
    [Export] public AudioStream sfx { get; private set; }

    public virtual void InitializeStatus(Node target) { return; }
    public virtual void ApplyStatus(Node target) 
    {
        GD.Print("Apply: " + id);
        EmitSignal(SignalName.StatusApplied, this);
    }
    public void SetDuration(int newVal)
    {
        this.duration = newVal;
        EmitSignal(SignalName.StatusChanged);
    }
    public void AddDuration(int newVal)
    {
        this.duration += newVal;
        EmitSignal(SignalName.StatusChanged);
    }
    public void SetStacks(int newVal)
    {
        this.stacks = newVal;
        EmitSignal(SignalName.StatusChanged);
    }
    public void AddStacks(int newVal)
    {
        this.stacks += newVal;
        EmitSignal(SignalName.StatusChanged);
    }
}
