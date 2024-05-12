using Godot;
using System;

[GlobalClass]
public partial class EnemyAction : Resource
{
    [Export] public EnemyIntent intent;
	[Export] public EnemyActionType type;
	[Export(PropertyHint.Range, "0,10")] public int chanceWeight = 0;
	[Export] public AudioStream sound;
	public float accumulatedweight = 0;

	public Enemy enemy;
	public Node2D target;

	public virtual bool IsPerformable()
	{
		return true;
	}

	public virtual void PerformAction(){}
}
