using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class CardStats : Resource
{
    [ExportGroup("Card Display")]
    [Export] public String cardName { get; private set;}
    [Export] public CardType cardType { get; private set;}
    [Export] public CardTag cardTag { get; private set;}
    [Export] public String cardDesc { get; private set;}
    [Export] public int cardCost { get; private set;}
    [Export] public int cardGen { get; private set;}
    [Export] public bool isExhaust { get; private set; } = false;
    
    [ExportGroup("Card Behavior")]
    [Export] public Target target { get; private set;}

    [ExportGroup("External")]
    [Export] public Texture2D cardArt { get; private set;}
    [Export] public AudioStream playSFX { get; private set;}
    public AudioStream burnSFX = ResourceLoader.Load<AudioStream>("res://Audio/SFX/CardBurn.wav");

    public bool isSingleTargeted()
    {
        return target == Target.SINGLE;
    }

    public List<Node2D> GetTargets(List<Node2D> targets)
    {
        if(targets == null)
        {
            return null;
        }

        SceneTree tree = targets[0].GetTree();

        switch(target)
        {
            case Target.SELF:
            return ToolsManager.GDArrayToList(tree.GetNodesInGroup("Player"));

            case Target.ALL:
            return ToolsManager.GDArrayToList(tree.GetNodesInGroup("Enemies"));

            case Target.EVERYONE:
            return ToolsManager.GDArrayToList(tree.GetNodesInGroup("Player") + tree.GetNodesInGroup("Enemies"));

            case Target.SINGLE:
            case Target.NONE:
            default:
            return null;
        }
    }
    public void Play(PlayerStats playerStats, List<Node2D> targets, ModifierManager modifiers)
    {
        playerStats.AddMana(-cardCost);

        if(isSingleTargeted())
        {
            ApplyEffects(targets, playerStats, modifiers);
        }
        else
        {
            ApplyEffects(GetTargets(targets), playerStats, modifiers);
        }
        EventManager.instance.EmitSignal(EventManager.SignalName.CardPlayed, this);
    }
    public void Burn(PlayerStats playerStats)
    {
        playerStats.AddMana(cardGen);
        AudioManager.instance.sfxPlayer.Play(this.burnSFX);
        EventManager.instance.EmitSignal(EventManager.SignalName.CardBurned, this);
    }
    public virtual void ApplyEffects(List<Node2D> targets, PlayerStats playerStats, ModifierManager modifiers)
    {
        return;
    }
}
