// meta-name: Card Stats
// meta-description: Functionality and stats for cards
using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class _CLASS_ : CardStats
{
    [Export] private int value = 0;
    public virtual void ApplyEffects(List<Node2D> targets, PlayerStats playerStats)
    {
        
    }
}
