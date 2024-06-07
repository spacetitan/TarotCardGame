// meta-name: Status
// meta-description: Functionality for new status'
using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class _CLASS_ : Status
{
	[Export] private int value = 0;
	public override void ApplyStatus(Node target)
	{
		
		base.ApplyStatus(target);
	}
}

