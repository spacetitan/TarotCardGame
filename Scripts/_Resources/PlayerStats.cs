using Godot;

[GlobalClass]
public partial class PlayerStats : CharacterStats
{
    [Export] public CardType type { get; private set; }
    [Export] public CardPile startingDeck{ get; private set; }
    [Export] public int handSize{ get; private set; } = 5;
    [Export] public int mana{ get; private set; } = 0;
    [Export] public Ability ability { get; private set; }

    public CardPile deck;
    public CardPile discard;

    public Player player { get; private set; }


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

    public void SetPlayer(Player player)
    {
        this.player = player;
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
        instance.type = this.type;
        instance.deck = (CardPile)instance.startingDeck.Duplicate();
        instance.deck.InitDeck();
        instance.discard = new CardPile();
        instance.ability = this.ability;
        return instance;
    }
}
