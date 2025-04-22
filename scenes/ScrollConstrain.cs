using Godot;
using System;
using System.Drawing;

public partial class ScrollConstrain : ScrollContainer
{
	[Export]
	public int sizeX;
	[Export]
	public int sizeY;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CustomMinimumSize = GetViewportRect().Size;
		Size = CustomMinimumSize;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
