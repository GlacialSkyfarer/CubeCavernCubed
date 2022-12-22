using Godot;
using System;

public partial class Door : CharacterBody3D
{

	bool open = false;

	[Export(PropertyHint.Layers2dPhysics)]
	public uint openLayers;
	[Export(PropertyHint.Layers2dPhysics)]
	public uint closedLayers;
	
	[Export]
	NodePath animTree;
	AnimationTree animationTree;
	AnimationNodeStateMachinePlayback blendTree;

	public override void _Ready() {

		animationTree = GetNode<AnimationTree>(animTree);
		blendTree = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		blendTree.Start("DoorClosed");

	}

	public void Toggle() {

		open = !open;

		CollisionLayer = open ? closedLayers : openLayers;
		CollisionMask = open ? closedLayers : openLayers;

		if (open) {

			blendTree.Travel("DoorOpen");

		} else {

			blendTree.Travel("DoorClosed");

		}

	}

}
