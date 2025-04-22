using Godot;
using System;

public partial class LevelManager : Control
{
	[Export]
	Godot.Collections.Array<PackedScene> levels = new Godot.Collections.Array<PackedScene>();

	Node currentLevel;
	[Export]
	public int levelIndex = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node newLevel = levels[levelIndex].Instantiate();
		currentLevel = newLevel;
		AddChild(currentLevel);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int levelIndexLast = levelIndex;
		if(Input.IsActionPressed("LevelCheat"))
		{

			if(Input.IsKeyPressed(Key.Key0))
			{
				levelIndex = 0;
			}else if (Input.IsKeyPressed(Key.Key1))
			{
				levelIndex = 1;

			}else if (Input.IsKeyPressed(Key.Key2))
			{
				levelIndex = 2;

			}else if (Input.IsKeyPressed(Key.Key3))
			{
				levelIndex = 3;

			}else if (Input.IsKeyPressed(Key.Key4))
			{
				levelIndex = 4;

			}
		}

		if(levelIndex != levelIndexLast)
		{
			levelIndex = levelIndex % levels.Count;
			currentLevel.QueueFree();
			Node newLevel = levels[levelIndex].Instantiate();
			currentLevel = newLevel;
			AddChild(currentLevel);
		}
	}
	public void LoadNextLevel()
	{
		levelIndex++;
		levelIndex = levelIndex % levels.Count;
		currentLevel.QueueFree();
		Node newLevel = levels[levelIndex].Instantiate();
		currentLevel = newLevel;
		AddChild(currentLevel);
	}
}
