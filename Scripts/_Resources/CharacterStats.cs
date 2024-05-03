using Godot;

[GlobalClass]
public partial class CharacterStats : StatsData
{
	public int strength { get; set;} 
    public int dexterity { get; set;} 
    public int intelligence { get; set;}

    public override void TakeDamage(int damage)
    {
        int initialHealth = health;
        base.TakeDamage(damage);
        if(initialHealth > health)
        {
            EventManager.instance.EmitSignal(EventManager.SignalName.PlayerHit);
        }
    }
}
