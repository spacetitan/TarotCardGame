using Godot;
using System;

[GlobalClass]
public partial class WinPrize : Resource
{
    [Export] public int xp = 0;
    [Export] public int money = 0;
    [Export] public Item[] items;

    public void AddPrize(WinPrize prize)
    {
        if (prize == null) { return; }

        this.xp += prize.xp;
        this.money += prize.money;

        if(prize.items == null || prize.items.Length < 1) { return; }

        int count = 0;
        Item[] tempArray = new Item[this.items.Length + prize.items.Length];

        foreach (Item value in this.items)
        {
            Item item = value;
            tempArray[count] = item;
            count++;
        }

        foreach (Item value in prize.items)
        {
            Item item = value;
            tempArray[count] = item;
            count++;
        }
    }
}
