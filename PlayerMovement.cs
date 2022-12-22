using Godot;
using System;

public partial class PlayerMovement : CharacterBody3D
{

	//NodePaths

	[Export]
	NodePath headPath;
	[Export]
	NodePath tooltipPath;
	[Export]
	NodePath itemLabelPath;
	[Export]
	NodePath cameraPath;
	[Export]
	NodePath interactRayPath;
	[Export]
	NodePath cameraAnimPath;
	[Export]
	NodePath subViewportPath;
	[Export]
	NodePath vmAnimPath;
	[Export]
	NodePath normalColliderPath;
	[Export]
	NodePath crouchColliderPath;
	[Export]
	NodePath standCastPath;

	CollisionShape3D normalCollider;
	CollisionShape3D crouchCollider;
	
	public int selectedSlot = 0;
	public Godot.Collections.Array<ItemResource> inventory;
	[Export]
	public int inventorySize = 3;

	[Export]
	public float walkSpeed = 6.5f;
	[Export]
	public float crouchSpeed = 3.25f;
	public float Speed = 5.0f;
	[Export]
	public float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	[Export]
	float minLookAngle = -80.0f;
	[Export]
	float maxLookAngle = 80.0f;
	[Export]
	float lookSensitivity = 10.0f;
		
	[Export]
	float movementControl = 0.05f;

	[Export]
	double maxHangTime = 0.2;
	double hangTime = 0;
	Vector2 mouseDelta;
	Label itemLabel;
	
	Vector2 input = Vector2.Zero;

	bool mouseMoving = false;

	bool jumping = false;

	int coinCount = 0;

	public RayCast3D interactCast;

	AnimationTree cameraAnimTree;
	AnimationTree vmAnimTree;
	Node3D camera;
	public Node3D collider;
	RichTextLabel tooltip;
	AnimationNodeStateMachinePlayback cameraSM;

	RayCast3D standCast;

	Node3D head;

	Node subViewport;
	
	public override void _Ready() {

		standCast = GetNode<RayCast3D>(standCastPath);

		crouchCollider = GetNode<CollisionShape3D>(crouchColliderPath);
		normalCollider = GetNode<CollisionShape3D>(normalColliderPath);
		

		subViewport = GetNode(subViewportPath);
		vmAnimTree = GetNode<AnimationTree>(vmAnimPath);
		head = (Node3D)GetNode(headPath);
		inventory = new Godot.Collections.Array<ItemResource>();
		tooltip = GetNode<RichTextLabel>(tooltipPath);
		itemLabel = GetNode<Label>(itemLabelPath);
		coinCount = 0;
		camera = GetNode<Node3D>(cameraPath);
		interactCast = GetNode<RayCast3D>(interactRayPath);
		cameraAnimTree = GetNode<AnimationTree>(cameraAnimPath);
		cameraSM = (AnimationNodeStateMachinePlayback)cameraAnimTree.Get("parameters/playback");
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);

	}

	public override void _UnhandledInput(InputEvent @event) 
	{
		
		if (@event is InputEventMouseMotion mouseMotion) {
			
			mouseMoving = true;
			mouseDelta = mouseMotion.Relative * 0.001f * lookSensitivity;
			
		}
		
	}

	public override void _Process(double delta) {


		if (Input.IsActionJustReleased("itemScrollUp")) {

			if (selectedSlot > 0) {

				selectedSlot -= 1;

			} else {

				selectedSlot = inventorySize - 1;

			}

		}

		if (Input.IsActionJustReleased("itemScrollDown")) {

			if (selectedSlot < inventorySize - 1) {

				selectedSlot += 1;

			} else {

				selectedSlot = 0;

			}

		}

		if (interactCast.IsColliding()){
			((Control)tooltip.GetParent()).Visible = true;
			if (collider != null) {

				if (collider.HasNode("Mesh")) {

				MeshInstance3D oldMesh = collider.GetNode<MeshInstance3D>("Mesh");
				if (oldMesh.MaterialOverride is BaseMaterial3D) {

				BaseMaterial3D mat = oldMesh.MaterialOverride as BaseMaterial3D;
				mat.EmissionEnergyMultiplier = 0f;

			}
			}
			}

			collider = (Node3D)interactCast.GetCollider();

			if (collider != null) {

				if (collider.HasNode("Mesh")) {

				MeshInstance3D mesh = collider.GetNode<MeshInstance3D>("Mesh");
				if (mesh.MaterialOverride is BaseMaterial3D) {

				BaseMaterial3D mat = mesh.MaterialOverride as BaseMaterial3D;
				mat.EmissionEnergyMultiplier = 0.1f;

			}

			}
			
			

			tooltip.Text = "[p align=center]" + collider.Name + "[/p]" + ((string)collider.GetMeta("tooltip", "[center]This interactable has no tooltip."));
			if (collider != null) {

				switch((string)collider.GetMeta("interactableType", "None")) {
				
				case "Item":


					bool freeItem = true;

					ItemResource iR = (ItemResource)collider.Get("itemResource");

					tooltip.Text = "[p align=center]" + iR.itemName + "[/p]" + "[p align=center]'E' To grab.[/p]" + iR.tooltip;

					if (Input.IsActionJustPressed("interact")) {

						if (iR.givesValue) {

							coinCount += iR.itemValue;

						}
						if (iR.isInventoryItem) {

							if (inventory.Count < inventorySize) {
								if (inventory.Contains(null)) {

									inventory[inventory.IndexOf(null)] = iR;

								} else {

									inventory.Add(iR);	

								}
								
							} else {
								
								if (inventory.Contains(null)) {

									inventory[inventory.IndexOf(null)] = iR;

								} else {

									if (iR.id != inventory[selectedSlot].id) {
									SpawnItem(inventory[selectedSlot].id);
									inventory[selectedSlot] = iR;
									} else {
										freeItem = false;
									}

								}
								
							}
							inventory[selectedSlot].OnPickup(this);

						}
						
						
						if (freeItem) {
							collider.Free();
							collider = null;
						}
						

					}
				break;
				case "Door":
					if (Input.IsActionJustPressed("interact")) collider.Call("Toggle");
				break;
				default:
				break;

			}

			}

			}
			
			
		} else if (collider != null) {
			((Control)tooltip.GetParent()).Visible = false;
			MeshInstance3D mesh = collider.GetNode<MeshInstance3D>("Mesh");
			if (mesh.MaterialOverride is BaseMaterial3D) {

				BaseMaterial3D mat = mesh.MaterialOverride as BaseMaterial3D;
				mat.EmissionEnergyMultiplier = 0f;

			}

		} else {
			((Control)tooltip.GetParent()).Visible = false;
		}

	}

	public void SpawnItem(string id) {

		PackedScene item = GD.Load<PackedScene>("res://Prefabs/Items/" + id + ".tscn");
		Node spawnItem = item.Instantiate();
		if (spawnItem is CharacterBody3D) {
			CharacterBody3D itemSpawn = spawnItem as CharacterBody3D;
			GetParent().AddChild(itemSpawn);
			itemSpawn.Position = Position;
			itemSpawn.Velocity = Basis.z * -6;
		}

	}

	public override void _PhysicsProcess(double delta) 
	{

		if (!Input.IsActionPressed("crouch") && !standCast.IsColliding()) {
			
			cameraAnimTree.Set("crouching", false);
			Speed = walkSpeed;
			crouchCollider.Disabled = true;
			normalCollider.Disabled = false;

		} else if (Input.IsActionPressed("crouch")) {

			cameraAnimTree.Set("crouching", true);
			Speed = crouchSpeed;
			crouchCollider.Disabled = false;
			normalCollider.Disabled = true;

		}

		if (Input.IsActionJustPressed("drop_item") && inventory.Count - 1 >= selectedSlot) {

			if (inventory[selectedSlot] != null) {

				SpawnItem(inventory[selectedSlot].id);
				inventory[selectedSlot] = null;

			}

		}

		if (Input.IsActionJustPressed("use") && inventory.Count - 1 >= selectedSlot) {

			if (inventory[selectedSlot] != null) {

				inventory[selectedSlot].OnUse(this, vmAnimTree);

			}

		} else if (Input.IsActionJustPressed("use")) {

			vmAnimTree.Set("parameters/AttackSeek/seek_position", 0);
			vmAnimTree.Set("parameters/AttackShot/active", true);

		}
		if (Input.IsActionJustPressed("alt_use") && inventory.Count - 1 >= selectedSlot) {

			if (inventory[selectedSlot] != null) {

				inventory[selectedSlot].OnAltUse(this, vmAnimTree);

			}

		}

		if (inventory.Count - 1 >= selectedSlot) {

			if (inventory[selectedSlot] != null) {

				PackedScene item = GD.Load<PackedScene>("res://Models/" + inventory[selectedSlot].id + "VM.tscn");
				Node spawnItem = item.Instantiate();
				if (spawnItem is Node3D) {
					Node3D itemSpawn = spawnItem as Node3D;
					if (subViewport.HasNode("Camera3D/Hand/ViewModel/ViewItem/Node2")) {
						if ((subViewport.GetNode("Camera3D/Hand/ViewModel/ViewItem/Node2") != itemSpawn)) {

							subViewport.GetNode("Camera3D/Hand/ViewModel/ViewItem/Node2").Free();
							subViewport.GetNode("Camera3D/Hand/ViewModel/ViewItem").AddChild(itemSpawn);

						}
						
					} else {
						subViewport.GetNode("Camera3D/Hand/ViewModel/ViewItem").AddChild(itemSpawn);
					}
				} 

			} else if (subViewport.HasNode("Camera3D/Hand/ViewModel/ViewItem/Node2")) {

			subViewport.GetNode("Camera3D/Hand/ViewModel/ViewItem/Node2").Free();

		}

		} else if (subViewport.HasNode("Camera3D/Hand/ViewModel/ViewItem/Node2")) {

			subViewport.GetNode("Camera3D/Hand/ViewModel/ViewItem/Node2").Free();

		}

		

		itemLabel.Text = (coinCount.ToString() + " Coins");
		
		Vector3 velocity = Velocity;

		if (!mouseMoving) {
			
			mouseDelta = Vector2.Zero;
			
		}
		
		RotateY(-mouseDelta.x);
		
		camera.RotateX(-mouseDelta.y);
		
		camera.Rotation = new Vector3(Mathf.Clamp(camera.GetRotation().x, minLookAngle, maxLookAngle), camera.GetRotation().y, camera.GetRotation().z);

		head.Rotation = camera.Rotation;

		// Add the gravity.
		if (!IsOnFloor() && hangTime <= 0) {
		velocity.y -= gravity * (float)delta;
		}
		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor() && !Input.IsActionPressed("crouch")) {
			velocity.y = JumpVelocity;
			vmAnimTree.Set("parameters/JumpSeek/seek_position", 0);
			vmAnimTree.Set("parameters/JumpShot/active", true);
			jumping = true;
		}
			
		if (!Input.IsActionPressed("jump")) {

			hangTime = 0;

		}
		if (jumping && velocity.y < 0) {

			
			hangTime = maxHangTime;
			jumping = false;

		}
		
		if (Input.IsActionJustPressed("exit"))
			GetTree().Quit();
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("movement_left", "movement_right", "movement_forward", "movement_back");
		if (inputDir != Vector2.Zero && IsOnFloor()) {

			cameraAnimTree.Set("moving", true);
			vmAnimTree.Set("moving", true);

		} else {
			cameraAnimTree.Set("moving", false);
			vmAnimTree.Set("moving", false);
		}
		if (inputDir != Vector2.Zero) {input = input.Lerp(inputDir, movementControl);} else {input = input.Lerp(Vector2.Zero, movementControl);}
		Vector3 direction = (Transform.basis * new Vector3(input.x, 0, input.y));
		if (direction != Vector3.Zero)
		{
			velocity.x = direction.x * Speed;
			velocity.z = direction.z * Speed;
		}
		else
		{
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
			velocity.z = Mathf.MoveToward(Velocity.z, 0, Speed);
		}

		hangTime -= delta;

		Velocity = velocity;
		MoveAndSlide();
		
		mouseMoving = false;
	}
}
