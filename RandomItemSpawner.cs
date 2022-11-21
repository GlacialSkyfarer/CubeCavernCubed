using Godot;
using System;

public partial class RandomItemSpawner : Node3D
{

	[Export]
	private string[] itemPool;
	bool hasSpawned = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Process(double delta)
	{

		hasSpawned = false;

		string randomValue = itemPool[GD.Randi() % itemPool.Length];

		PackedScene item = GD.Load<PackedScene>("res://Prefabs/Items/" + randomValue + ".tscn");
		Node spawnItem = item.Instantiate();
		if (spawnItem is Node3D) {
			Node3D itemSpawn = spawnItem as Node3D;
			GetParent().AddChild(itemSpawn);
			itemSpawn.Position = Position;
			hasSpawned = true;
		}
		
		Free();
		
	}

}
