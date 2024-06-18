using Godot;
using System;
using System.Collections.Generic;

public partial class Modifier : Node
{
	[Export] public ModifierType type;
	List<ModifierValue> modifierValues = new List<ModifierValue>();

	public Modifier(ModifierType type)
	{
		this.type = type;
	}

	public ModifierValue GetValue(String source)
	{
		foreach (ModifierValue value in modifierValues)
		{
			if(value.source == source)
			{
				return value;
			}
		}

		GD.Print("No values found.");
		return null;
	}

	public void AddNewValue(ModifierValue value)
	{
		ModifierValue tempValue = GetValue(value.source);

		if(tempValue == null)
		{
			this.modifierValues.Add(value);
		}
		else
		{
			tempValue.flatVal = value.flatVal;
			tempValue.percentVal = value.percentVal;
		}
	}

	public void RemoveValue(String source)
	{
		foreach (ModifierValue value in this.modifierValues)
		{
			if(value.source == source)
			{
				value.QueueFree();
			}
		}
	}

	public void ClearValues()
	{
		this.modifierValues.Clear();
	}

	public int GetModifiedValue(int baseVal)
	{
		int flatResult = baseVal;
		float percentResult = 1.0f;

		foreach (ModifierValue value in this.modifierValues)
		{
			switch(value.type)
			{
				case ModifierValueType.FLAT:
				flatResult += value.flatVal;
				break;

				case ModifierValueType.PERCENT:
				percentResult += value.percentVal;
				break;

				case ModifierValueType.NONE:
				default:
				GD.Print("No Modifier value type set.");
				break;
			}
		}

		return (int) Math.Clamp(flatResult * percentResult, 0, 1000);
	}
}
