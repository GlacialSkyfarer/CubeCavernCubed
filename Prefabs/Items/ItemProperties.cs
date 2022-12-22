using Godot;
using System;

public partial class ItemProperties : CharacterBody3D
{
	
	[Export]
	public Resource itemResource;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 vel = Velocity;
		vel.y -= gravity * (float)delta;
		vel.x = Mathf.Lerp(vel.x, 0, 0.08f);
		vel.z = Mathf.Lerp(vel.z, 0, 0.08f);
		Velocity = vel;
		MoveAndSlide();
	}

}
