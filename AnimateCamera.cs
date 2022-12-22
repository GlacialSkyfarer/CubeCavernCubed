using Godot;
using System;

public partial class AnimateCamera : AnimationTree
{
	
	bool crouching = false;
	bool moving = false;

	float walkAmount = 0;
	float crouchAmount = 0;
	
	public override void _Process(double delta)
	{

		if (moving) {

			walkAmount = Mathf.Lerp(walkAmount, 1, 0.2f);

			Set("parameters/BlendIdleWalk/blend_amount", walkAmount);

		} else {

			walkAmount = Mathf.Lerp(walkAmount, 0, 0.2f);

			Set("parameters/BlendIdleWalk/blend_amount", walkAmount);

		}

		if (crouching) {

			crouchAmount = Mathf.Lerp(crouchAmount, 1, 0.2f);

			Set("parameters/AddCrouch/add_amount", crouchAmount);

		} else {

			crouchAmount = Mathf.Lerp(crouchAmount, 0, 0.2f);

			Set("parameters/AddCrouch/add_amount", crouchAmount);

		}
 
	}
}
