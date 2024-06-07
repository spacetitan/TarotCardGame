using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Mark : Status
{
	[Export] private int value = 2;
	public override void ApplyStatus(Node target)
	{
		DamageEffect damageEffect = new DamageEffect(value, null);
        damageEffect.Execute(target as Node2D);

		base.ApplyStatus(target);
	}
}


