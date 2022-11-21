using Godot;
using System;

public partial class PlayerMovement : CharacterBody3D
{

	[Export]
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

	RayCast3D rayCast;

	AnimationTree cameraAnimTree;
	Node3D camera;
	Node3D collider;
	RichTextLabel tooltip;
	AnimationNodeStateMachinePlayback stateMachine;
	
	public override void _Ready() {

		tooltip = GetNode<RichTextLabel>("/root/Root/PanelContainer/RichTextLabel");
		itemLabel = GetNode<Label>("/root/Root/ItemCount");
		coinCount = 0;
		camera = GetNode<Node3D>("PlayerCamera");
		rayCast = camera.GetNode<RayCast3D>("RayCast3D");
		cameraAnimTree = camera.GetNode<Camera3D>("Camera").GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)cameraAnimTree.Get("parameters/playback");
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);

	}

	public override void _UnhandledInput(InputEvent @event) 
	{
		
		if (@event is InputEventMouseMotion mouseMotion) {
			
			mouseMoving = true;
			mouseDelta = mouseMotion.Relative * 0.001f * lookSensitivity;
			
		}
		
	}
		
	public override void _PhysicsProcess(double delta) 
	{

		if (rayCast.IsColliding()){
			((Control)tooltip.GetParent()).Visible = true;
    		collider = (Node3D)rayCast.GetCollider();

			MeshInstance3D mesh = collider.GetNode<MeshInstance3D>("Mesh");
			if (mesh.MaterialOverride is BaseMaterial3D) {

				BaseMaterial3D mat = mesh.MaterialOverride as BaseMaterial3D;
				mat.EmissionEnergyMultiplier = 1f;

			}

			tooltip.Text = "[p align=center]" + collider.Name + "[/p]" + ((string)collider.GetMeta("tooltip", "[center]This interactable has no tooltip."));

			switch((string)collider.GetMeta("interactableType", "None")) {
				
				case "Item":

					ItemResource iR = (ItemResource)collider.Get("itemResource");

					tooltip.Text = "[p align=center]" + iR.itemName + "[/p]" + ((string)collider.GetMeta("tooltip", "[center]This item has no tooltip."));

					if (Input.IsActionJustPressed("interact")) {

						if (iR.givesValue) {

							coinCount += iR.itemValue;

						}
						
						iR.itemScript.Call("OnPickup");

						collider.Free();
						collider = null;

					}
				break;
				default:
				break;

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

		itemLabel.Text = (coinCount.ToString() + " Coins");
		
		Vector3 velocity = Velocity;

		if (!mouseMoving) {
			
			mouseDelta = Vector2.Zero;
			
		}
		
		RotateY(-mouseDelta.x);
		
		camera.RotateX(-mouseDelta.y);
		
		camera.Rotation = new Vector3(Mathf.Clamp(camera.GetRotation().x, minLookAngle, maxLookAngle), camera.GetRotation().y, camera.GetRotation().z);

		// Add the gravity.
		if (!IsOnFloor() && hangTime <= 0) {
		velocity.y -= gravity * (float)delta;
		}
		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			velocity.y = JumpVelocity;
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

			if (stateMachine.GetCurrentNode() != "PlayerCameraWalk") stateMachine.Travel("PlayerCameraWalk");

		} else {
			if (stateMachine.GetCurrentNode() != "PlayerCameraIdle") stateMachine.Travel("PlayerCameraIdle");
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
