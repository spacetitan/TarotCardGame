using Godot;
using System;

[GlobalClass]
public partial class WinPrize : Resource
{
    [Export] int xp = 0;
    [Export] int money = 0;
    [Export] Item[] items;
}
