using Godot;
using System;

public partial class ItemResource : Resource
{

    [Export]
    public string itemName;
    [Export]
    public string id;
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
    [Export]
    public float useTime;
    [Export]
    public float range;
    [Export(PropertyHint.MultilineText)]
    public string tooltip;

    public virtual async void OnUse(PlayerMovement player, AnimationTree anim) {

        anim.Set("parameters/AttackSeek/seek_position", 0);
        anim.Set("parameters/AttackShot/active", true);

	}

	public virtual void OnPickup(PlayerMovement player) {



	}

	public virtual void OnAltUse(PlayerMovement player, AnimationTree anim) {

		

	}

}
