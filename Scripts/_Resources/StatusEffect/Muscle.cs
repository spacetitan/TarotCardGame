using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Muscle : Status
{
	[Export] private int value = 0;
	Node target;

    public override void InitializeStatus(Node target)
    {
		this.target = target;
		StatusChanged += OnStatusChanged;
		OnStatusChanged();
        base.InitializeStatus(target);
    }

	void OnStatusChanged()
	{
		Modifier modifier;
		ModifierValue value = null;

		if(target is Enemy)
		{
			Enemy enemy = (Enemy)target;
			modifier = enemy.modifierManager.GetModifier(ModifierType.DMGDEALT);
			value = modifier.GetValue("muscle");
		}
		else if(target is Player)
		{
			Player player = (Player)target;
			modifier = player.modifierManager.GetModifier(ModifierType.DMGDEALT);
			value = modifier.GetValue("muscle");
		}
		else {GD.Print("Target not applicable"); return; }

		if(value == null)
		{
			value = ModifierValue.CreateModifierValue("muscle", ModifierValueType.FLAT);
		}

		value.flatVal = stacks;
		modifier.AddNewValue(value);

		if(stacks <= 0 && modifier != null)
        {
            modifier.RemoveValue("muscle");
        }
	}

    public override void ApplyStatus(Node target)
	{	
		base.ApplyStatus(target);
	}
}


