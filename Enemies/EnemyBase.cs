using Godot;
using System;

public partial class EnemyBase : CharacterBody3D
{

	float remainingHP = 0;
	[Export]
	float maxHP = 5;

	[Export]
	NodePath meshPath;
	MeshInstance3D mesh;

	float damageColorAmount = 0;

	[Export]
	private Godot.Collections.Array<Godot.Collections.Array<string>> itemPools;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		remainingHP = maxHP;
		mesh = GetNode<MeshInstance3D>(meshPath);

	}

	public virtual void Hurt(float damage) {

		remainingHP -= damage;
		damageColorAmount = 0.2f;
		if (remainingHP <= 0) {

			OnDeath();

		}
		
	}

	public virtual void OnDeath() {

		foreach (Godot.Collections.Array<string> array in itemPools) {

			GD.Randomize();

			string randomValue = array[(GD.RandRange(0, (array.Count - 1)))];

			if (randomValue	!= "None") {
				PackedScene item = GD.Load<PackedScene>("res://Prefabs/Items/" + randomValue + ".tscn");
				Node spawnItem = item.Instantiate();
				if (spawnItem is Node3D) {
					Node3D itemSpawn = spawnItem as Node3D;
					GetParent().AddChild(itemSpawn);
					itemSpawn.Position = Position;
				}
			}

		}

		QueueFree();

	}

	public override void _Process(double delta) {

		damageColorAmount -= (float)delta;

		StandardMaterial3D mat = (StandardMaterial3D)mesh.Mesh.SurfaceGetMaterial(0);
		if (damageColorAmount >= 0) {
			mat.EmissionEnergyMultiplier = damageColorAmount * 0.5f;
		} else {
			mat.EmissionEnergyMultiplier = 0;
		}
		mesh.Mesh.SurfaceSetMaterial(0, mat);

	}

	
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
