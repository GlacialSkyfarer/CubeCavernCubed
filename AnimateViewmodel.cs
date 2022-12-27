using Godot;
using System;

public partial class AnimateViewmodel : AnimationTree
{

	float blendAmount = 0;
	public bool moving = false;

	public float itemCooldown = 0;
	public float maxItemCooldown = 0;
	float itemBlend = 0;

	public override void _Process(double delta)
	{
		if (!moving) {

			blendAmount = Mathf.Lerp(blendAmount, 0, 0.2f);
			Set("parameters/IdleBlendWalk/blend_amount", blendAmount);
		
		} else {
			blendAmount = Mathf.Lerp(blendAmount, 1, 0.2f);
			Set("parameters/IdleBlendWalk/blend_amount", blendAmount);
		}

		if (itemCooldown <= 0) {

			itemBlend = Mathf.Lerp(itemBlend, 0, 0.3f);

			Set("parameters/ChargingBlend/blend_amount", itemBlend);
		
		} else {
			itemBlend = Mathf.Lerp(itemBlend, itemCooldown / maxItemCooldown, 0.3f);
			Set("parameters/ChargingBlend/blend_amount", itemBlend);
		}

		base._Process(delta);
	}

}

