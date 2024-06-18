using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Mark : Status
{
	const int MODIFIER_VALUE = 2;
	Node target;
    public override void InitializeStatus(Node target)
    {
		this.target = target;

        Modifier modifier;
		ModifierValue value = null;

		if(target is Enemy)
		{
			Enemy enemy = (Enemy)target;
			modifier = enemy.modifierManager.GetModifier(ModifierType.DMGTAKEN);
			value = modifier.GetValue("marked");
		}
		else if(target is Player)
		{
			Player player = (Player)target;
			modifier = player.modifierManager.GetModifier(ModifierType.DMGTAKEN);
			value = modifier.GetValue("marked");
		}
		else { GD.Print("Target not applicable"); return; }

		if(value == null)
		{
			value = ModifierValue.CreateModifierValue("marked", ModifierValueType.FLAT);
            value.flatVal = MODIFIER_VALUE;
            modifier.AddNewValue(value);
		}

        StatusChanged -= OnStatusChanged;
        StatusChanged += OnStatusChanged;
    }

    void OnStatusChanged()
	{
        Modifier modifier;

        if(target is Enemy)
		{
			Enemy enemy = (Enemy)target;
			modifier = enemy.modifierManager.GetModifier(ModifierType.DMGTAKEN);
		}
		else if(target is Player)
		{
			Player player = (Player)target;
			modifier = player.modifierManager.GetModifier(ModifierType.DMGTAKEN);
		}
		else { GD.Print("Target not applicable"); return; }

		if(stacks <= 0 && modifier != null)
        {
            modifier.RemoveValue("marked");
        }
	}

	public override void ApplyStatus(Node target)
	{	
		this.SetStacks(this.stacks-1);
		base.ApplyStatus(target);
	}
}


