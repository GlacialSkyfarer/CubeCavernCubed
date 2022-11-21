using Godot;
using System;

public partial class ItemResource : Resource
{

    [Export]
    public Script itemScript;
    [Export]
    public string itemName;
    [Export]
    public bool isInventoryItem = false;
    [Export]
    public int itemValue;
    [Export]
    public bool givesValue = false;
    [Export]
    public int ammoValue;
    [Export]
    public bool givesAmmo = false;
    [Export]
    public int healthValue;
    [Export]
    public bool givesHealth;
    [Export]
    public bool isWeapon;

}
