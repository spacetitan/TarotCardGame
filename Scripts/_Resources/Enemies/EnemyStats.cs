using Godot;
using System;

[GlobalClass]
public partial class EnemyStats : CharacterStats
{
    [Export] public EnemyAction[] actions { get; private set; }
    [Export] WinPrize winPrize;
    public override EnemyStats CreateInstance()
    {
        EnemyStats instance = (EnemyStats)this.Duplicate();
		instance.art = this.art;
		instance.SetMaxHealth(this.maxHealth);
		instance.ResetHealth();
		instance.ResetArmor();
        instance.strength = this.strength;
        instance.dexterity = this.dexterity;
        instance.intelligence = this.intelligence;
        return instance;
    }
}
