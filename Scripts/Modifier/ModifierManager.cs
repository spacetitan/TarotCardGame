using Godot;
using System;
using System.Collections.Generic;

public partial class ModifierManager : Node
{
	List<Modifier> modifiers = new List<Modifier>();

	public ModifierManager()
	{
		this.modifiers.Add(new Modifier(ModifierType.DMGDEALT));
		this.modifiers.Add(new Modifier(ModifierType.DMGTAKEN));
		this.modifiers.Add(new Modifier(ModifierType.CARDCOST));
	}

	public bool HasModifier(ModifierType modifierType)
	{
		foreach (Modifier mod in modifiers)
		{
			if(mod.type == modifierType)
			{
				return true;
			}
		}

		return false;
	}

	public Modifier GetModifier(ModifierType modifierType)
	{
		foreach (Modifier modifier in modifiers)
		{
			if(modifier.type == modifierType)
			{
				return modifier;
			}
		}

		return null;
	}

	public int GetModifiedValue(int baseVal, ModifierType modifierType)
	{
		Modifier modifier = GetModifier(modifierType);

		if(modifier == null)
		{
			return baseVal;
		}

		return modifier.GetModifiedValue(baseVal);
	}
}

