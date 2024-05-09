using Godot;

[GlobalClass]
public partial class PlayerStats : CharacterStats
{
    [Export] public CardPile startingDeck{ get; private set; }
    [Export] public int handSize{ get; private set; } = 5;
    [Export] public int mana{ get; private set; } = 0;

    public CardPile deck;
    public CardPile discard;

    private void SetMana(int value)
    {
        this.mana = value;
        EmitSignal(SignalName.StatsChanged);
    }

    public void AddMana(int value)
    {
        this.mana += value;
        EmitSignal(SignalName.StatsChanged);
    }

    public void ResetMana()
    {
        this.mana = 0;
        EmitSignal(SignalName.StatsChanged);
    }

    public bool CanPlayCard(CardUI card)
    {
        return this.mana >= card.cardStats.cardCost;
    }

    public override PlayerStats CreateInstance()
    {
        PlayerStats instance = (PlayerStats)this.Duplicate();
        instance.art = this.art;
		instance.SetMaxHealth(this.maxHealth);
		instance.ResetHealth();
		instance.ResetArmor();

        instance.strength = this.strength;
        instance.dexterity = this.dexterity;
        instance.intelligence = this.intelligence;

        instance.ResetMana();
        instance.deck = (CardPile)instance.startingDeck.Duplicate();
        instance.deck.InitDeck();
        instance.discard = new CardPile();
        return instance;
    }
}
