using Godot;
using System;

public partial class ScalingBaqckground : TextureRect
{
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ResizeBG();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ResizeBG()
	{		
		Rect2 viewportRect = GetViewportRect();
		Vector2 viewportWH = viewportRect.Size;

		float targetWidth = viewportWH.X * 1.02f;
		float ratio = targetWidth / (float)Texture.GetWidth();
		float targetHeight = Texture.GetHeight() * ratio;

		CustomMinimumSize = new Vector2(targetWidth, targetHeight);
		Size = CustomMinimumSize;
}

}
