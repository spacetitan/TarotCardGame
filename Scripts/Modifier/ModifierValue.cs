using Godot;
using System;

public partial class ModifierValue : Node
{
	[Export] public ModifierValueType type { get; private set; } 
	[Export] public float percentVal;
	[Export] public int flatVal;
	[Export] public String source  { get; private set; }

	public static ModifierValue CreateModifierValue(String modifierSource, ModifierValueType modifierValueType)
	{
		ModifierValue newModifier = new ModifierValue();
		newModifier.source = modifierSource;
		newModifier.type = modifierValueType;
		return newModifier;
	}
}
