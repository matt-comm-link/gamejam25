using Godot;
using System;

public partial class MapManager : Control
{
	[Export]
	Godot.Collections.Array<EventHoverTooltip> tooltips = new Godot.Collections.Array<EventHoverTooltip>();
	[Export]
    public Godot.Collections.Dictionary<string, string> globals = new Godot.Collections.Dictionary<string, string>();

	[Export]
	float mouseMoveScale;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UpdateVisibleGraph();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void UpdateVisibleGraph()
	{
		foreach(EventHoverTooltip station in tooltips)
		{
			station.UpdateDisplay();
		}
	}


	public void DragMap()
	{
		if(Input.IsMouseButtonPressed(MouseButton.Right))
		{
			Vector2 mousePos = Input.GetLastMouseVelocity();
		}
	}
}
