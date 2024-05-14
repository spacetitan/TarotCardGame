using Godot;
using System;

[GlobalClass]
public partial class EnemyStats : CharacterStats
{
    [Export] public EnemyAction[] actions { get; private set; }
    [Export] public WinPrize winPrize { get; private set; }
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

        instance.actions = new EnemyAction[this.actions.Length];
        int count = 0;

        foreach (EnemyAction action in this.actions)
        {
            instance.actions[count] = action;
            count++;
        }

        instance.winPrize = this.winPrize;

        return instance;
    }
}
