using Godot;
using System;

[GlobalClass]
public partial class StatsData : Resource
{
	[Signal] public delegate void StatsChangedEventHandler();
	[Export] public String name;
	[Export] public String desc;
	[Export] public Texture2D art;
	[Export] public int maxHealth { get; private set;} 
	[Export] public int health{ get; private set;}
	[Export] public int armor{ get; private set;}

	private void SetHealth(int value)
	{
		this.health = Math.Clamp(value, 0, this.maxHealth);
		EmitSignal(SignalName.StatsChanged);
	}

	public void SetMaxHealth(int value)
	{
		this.maxHealth = value;
		EmitSignal(SignalName.StatsChanged);
	}

	public void ResetHealth()
	{
		this.health = this.maxHealth;
		EmitSignal(SignalName.StatsChanged);
	}

	public void Heal(int amount)
	{
		this.health = Math.Clamp(this.health + amount, 0, this.maxHealth);
		EmitSignal(SignalName.StatsChanged);
	}

	public virtual void TakeDamage(int damage)
	{
		if(damage == 0)
		{
			return;
		}

		int initial_dmg = damage;
		damage = Math.Clamp(damage - armor, 0, damage);
		this.armor = Math.Clamp(armor - initial_dmg, 0, armor);
		this.health = Math.Clamp(this.health - damage, 0, this.health);
		EmitSignal(SignalName.StatsChanged);
	}

	public void SetArmor(int value)
	{
		this.armor = Math.Clamp(value, 0, 999);
		EmitSignal(SignalName.StatsChanged);
	}

	public void AddArmor(int armor)
	{
		if(armor == 0)
		{
			return;
		}

		this.armor += armor;
		EmitSignal(SignalName.StatsChanged);
	}

	public void ResetArmor()
	{
		this.armor = 0;
		EmitSignal(SignalName.StatsChanged);
	}

	public virtual Resource CreateInstance()
	{
		StatsData instance = (StatsData)this.Duplicate();
		instance.art = this.art;
		instance.SetMaxHealth(this.maxHealth);
		instance.ResetHealth();
		instance.ResetArmor();
		return instance;
	}
}
