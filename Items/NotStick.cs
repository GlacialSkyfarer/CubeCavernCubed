using Godot;
using System;

public partial class NotStick : ItemResource
{

    public int damage = 1;
	
	public override void OnUse(PlayerMovement player, AnimationTree anim) {

        if (player.interactCast.IsColliding()) {

            player.collider = null;

            Node3D collider = (Node3D)player.interactCast.GetCollider();
            collider.Free();
            

        }

        base.OnUse(player, anim);

    }

}
