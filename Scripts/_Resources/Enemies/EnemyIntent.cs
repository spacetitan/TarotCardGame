using Godot;
using System;

[GlobalClass]
public partial class EnemyIntent : Resource
{
    [Export] public String number;
    [Export] public Texture2D icon;

    public void SetIntent(String number, Texture2D icon)
    {
        if(number != null)
        {
            this.number = number;
        }

        if(icon != null)
        {
            this.icon = icon;
        }
    }
}
