using Godot;
using System;

public partial class Stick : ItemResource
{

    public int damage = 1;
	
	public override void OnUse(PlayerMovement player, AnimationTree anim) {

        player.SpawnItem("PlatinumCoin");

        base.OnUse(player, anim);

    }

}
