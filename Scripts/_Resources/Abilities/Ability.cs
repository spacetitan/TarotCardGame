using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Ability : Resource
{
    [Export] public int value { get; private set; }
    [Export] public Target targetType { get; private set; }
    [Export] public AudioStream playSFX { get; private set; }
    public bool abilityUsed = false;

    public virtual void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
    {
        EventManager.instance.EmitSignal(EventManager.SignalName.PlayerAbilityused);
        return;
    }

    public List<Node2D> GetTargets(List<Node2D> targets)
    {
        if(targets == null)
        {
            return null;
        }

        SceneTree tree = targets[0].GetTree();

        switch(targetType)
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
}
