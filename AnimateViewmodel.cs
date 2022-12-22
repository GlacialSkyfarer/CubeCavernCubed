using Godot;
using System;

public partial class AnimateViewmodel : AnimationTree
{

	float blendAmount = 0;
	public bool moving = false;

	public override void _Process(double delta)
	{
		if (!moving) {

			blendAmount = Mathf.Lerp(blendAmount, 0, 0.2f);
			Set("parameters/IdleBlendWalk/blend_amount", blendAmount);
		
		} else {
			blendAmount = Mathf.Lerp(blendAmount, 1, 0.2f);
			Set("parameters/IdleBlendWalk/blend_amount", blendAmount);
		}

		base._Process(delta);
	}

}

