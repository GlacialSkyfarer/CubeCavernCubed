using Godot;
using System;
using System.Linq;

public partial class CrucibleScript : StaticBody3D
{

	[Export]
	Godot.Collections.Array<RecipeResource> recipes;
	[Export]
	NodePath areaPath;
	Area3D area;

	Godot.Collections.Array<string> currentItems = new Godot.Collections.Array<string>();

	public static bool CheckStringArraysWithoutOrder(Godot.Collections.Array<string> arr1, Godot.Collections.Array<string> arr2) {

		string[] arr1Conv = arr1.ToArray<string>();
		string[] arr2Conv = arr2.ToArray<string>();

		if (arr1Conv.Length != arr2Conv.Length) return false;
		Array.Sort(arr1Conv);
		Array.Sort(arr2Conv);
		return true;

	}

	public override void _Ready() {

		area = (Area3D)GetNode(areaPath);

	}

	public void Craft() {

		currentItems.Clear();

		foreach (PhysicsBody3D col in area.GetOverlappingBodies()) {

			if ((ItemResource)col.Get("itemResource") != null) {

				currentItems.Add(((ItemResource)col.Get("itemResource")).id);

			}

		}

		foreach (RecipeResource r in recipes) {

			if (CheckStringArraysWithoutOrder(currentItems, r.ingredients)) {

				PackedScene item = GD.Load<PackedScene>("res://Prefabs/Items/" + r.result + ".tscn");
				Node spawnItem = item.Instantiate();
				if (spawnItem is Node3D) {
					Node3D itemSpawn = spawnItem as Node3D;
					GetParent().AddChild(itemSpawn);
					itemSpawn.Position = Position + Vector3.Up * 0.5f;
				}
				foreach (PhysicsBody3D col in area.GetOverlappingBodies()) {

					col.QueueFree();

				}

			}

		}

	}

}
