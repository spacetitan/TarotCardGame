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
    [Export] public Texture2D cardArt { get; private set;}

    // [ExportGroup("Card Audio")]
    // [Export] public AudioStream playSFX;
    // [Export] public AudioStream burnSFX = ResourceLoader.Load<AudioStream>("res://Sounds/FireSFX2.wav");

    [ExportGroup("Card Behavior")]
    [Export] public Target target { get; private set;}

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
            return ToolsManager.instance.GDArrayToList(tree.GetNodesInGroup("Player"));

            case Target.ALL:
            return ToolsManager.instance.GDArrayToList(tree.GetNodesInGroup("Enemies"));

            case Target.EVERYONE:
            return ToolsManager.instance.GDArrayToList(tree.GetNodesInGroup("Player") + tree.GetNodesInGroup("Enemies"));

            case Target.SINGLE:
            case Target.NONE:
            default:
            return null;
        }
    }
    public void Play(PlayerStats playerStats, List<Node2D> targets)
    {
        playerStats.AddMana(-cardCost);

        if(isSingleTargeted())
        {
            ApplyEffects(targets);
        }
        else
        {
            ApplyEffects(GetTargets(targets));
        }
        EventManager.instance.EmitSignal(EventManager.SignalName.CardPlayed, this);
    }
    public void Burn(PlayerStats playerStats)
    {
        playerStats.AddMana(cardGen);
        //AudioManager.instance.sfxPlayer.Play(burnSFX);
        EventManager.instance.EmitSignal(EventManager.SignalName.CardPlayed, this);
    }
    public virtual void ApplyEffects(List<Node2D> targets)
    {
        return;
    }
}
