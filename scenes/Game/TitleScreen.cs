using Godot;
using System;

public partial class TitleScreen : Control
{

	LevelManager lm;

	int tickTimer = 120;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
		lm = GetNode("/root/LevelManager") as LevelManager;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(tickTimer > 0)
			tickTimer--;

		if(Input.IsMouseButtonPressed(MouseButton.Left))
        {
            if(lm != null && tickTimer <= 0)
                lm.LoadNextLevel();
            else
                GD.Print("reached the end of available levels");

        }
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseButton)
        {
            if(mouseButton.ButtonIndex == 0)
            {
                if(lm != null)
                    lm.LoadNextLevel();
                else
                    GD.Print("reached the end of available levels");
            }
        }
    }
}
