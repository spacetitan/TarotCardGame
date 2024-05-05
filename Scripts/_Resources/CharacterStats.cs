using Godot;

[GlobalClass]
public partial class CharacterStats : StatsData
{
	[Export] public int strength { get; set;} 
    [Export] public int dexterity { get; set;} 
    [Export] public int intelligence { get; set;}

    public override void TakeDamage(int damage)
    {
        int initialHealth = this.health;
        base.TakeDamage(damage);
        if(initialHealth > this.health)
        {
            EventManager.instance.EmitSignal(EventManager.SignalName.PlayerHit);
        }
    }

    public override CharacterStats CreateInstance()
    {
        CharacterStats instance = (CharacterStats)this.Duplicate();
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
