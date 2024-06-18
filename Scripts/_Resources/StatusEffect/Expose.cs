using Godot;
using System;

[GlobalClass]
public partial class Expose : Status
{
    const float MODIFIER_VALUE = 0.5f;
	private Node target;
    public override void InitializeStatus(Node target)
    {
		this.target = target;

        Modifier modifier;
		ModifierValue value = null;

		if(target is Enemy)
		{
			Enemy enemy = (Enemy)target;
			modifier = enemy.modifierManager.GetModifier(ModifierType.DMGTAKEN);
			value = modifier.GetValue("exposed");
		}
		else if(target is Player)
		{
			Player player = (Player)target;
			modifier = player.modifierManager.GetModifier(ModifierType.DMGTAKEN);
			value = modifier.GetValue("exposed");
		}
		else { GD.Print("Target not applicable"); return; }

		if(value == null)
		{
			value = ModifierValue.CreateModifierValue("exposed", ModifierValueType.PERCENT);
            value.percentVal = MODIFIER_VALUE;
            modifier.AddNewValue(value);
		}

        StatusChanged -= OnStatusChanged;
        StatusChanged += OnStatusChanged;
    }

    void OnStatusChanged()
	{
        Modifier modifier;
		ModifierValue value = null;

        if(this.target is Enemy)
		{
			Enemy enemy = (Enemy)target;
			modifier = enemy.modifierManager.GetModifier(ModifierType.DMGTAKEN);
			value = modifier.GetValue("exposed");
		}
		else if(this.target is Player)
		{
			Player player = (Player)target;
			modifier = player.modifierManager.GetModifier(ModifierType.DMGTAKEN);
			value = modifier.GetValue("exposed");
		}
		else { GD.Print("Target not applicable"); return; }

		if(duration <= 0 && modifier != null)
        {
            modifier.RemoveValue("exposed");
        }
	}

    public override void ApplyStatus(Node target)
    {
        base.ApplyStatus(target);
    }
}
