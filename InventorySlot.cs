using Godot;
using System;

public partial class InventorySlot : PanelContainer
{
	
	[Export]
	int itemIndex = 0;

	[Export]
	Node3D player;

	TextureRect tex;

    public override void _Process(double delta)
    {

		

		if (player == null) {

			player = GetNode<Node3D>("/root/Root/Player");

		}

		if (((int)player.Get("selectedSlot")) == itemIndex) {

			StyleBoxFlat style = (StyleBoxFlat)Get("theme_override_styles/panel");

			style.BgColor = (new Color(0.5f, 0.5f, 0.5f, 0.25f));
			
			Set("theme_override_styles/panel", style);

		} else {

			StyleBoxFlat style = (StyleBoxFlat)Get("theme_override_styles/panel");

			style.BgColor = (new Color(0f, 0f, 0f, 0.25f));
			
			Set("theme_override_styles/panel", style);

		}

		Godot.Collections.Array<ItemResource> array = (Godot.Collections.Array<ItemResource>)player.Get("inventory");

		if (tex == null) {

			tex = GetNode<TextureRect>("TextureRect");

		}

		if (array.Count >= itemIndex + 1) {

			if (array[itemIndex] != null) {

				tex.Visible = true;
           		tex.Texture = GD.Load<Texture2D>("mugs/" + array[itemIndex].itemName + ".png");

			} else {
				tex.Visible = false;
			}
			
		} else {
			tex.Visible = false;
		}	

    }

}
