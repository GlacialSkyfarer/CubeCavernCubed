using Godot;
using System;

public partial class Stick : ItemResource
{

    [Export]
    public int damage = 2;
	
	public override void OnUse(PlayerMovement player, AnimationTree anim) {

        player.Call("Attack", damage);

        base.OnUse(player, anim);

    }

}
