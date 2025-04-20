using Godot;
using System;
using System.Collections.Generic;

public partial class EventHoverTooltip : PanelContainer
{
	[Export]string eventName;
	[Export]RichTextLabel EventName;
	[Export]RichTextLabel EventBlurb;
	[Export]TextureRect StationNode;
	[Export]Button previous;
	[Export]Button next;
	[Export]Button play;
	[Export]MarginContainer margin;

	[Export]EventHoverTooltip RootNode;

	[Export]
	Godot.Collections.Dictionary<string, string> blurbs = new Godot.Collections.Dictionary<string, string>();
	[Export]
	public string currentOutcome;

	[Export]
	public Godot.Collections.Array<Texture2D> backgrounds = new Godot.Collections.Array<Texture2D>();
	[Export]
	public Godot.Collections.Array<AudioStream> songs = new Godot.Collections.Array<AudioStream>();

	[Export]
	Texture2D LineMat;

	public List<string> outcomesDiscovered = new List<string>();

	[Export]
	public Resource FirstRunDialogTree;

	[Export]
	public Resource DialogTree;

	[Export]
	public DialogSceneManager DSM;
	[Export]
	public MapManager MM;

	[Export]
	public bool RunFirstTimeTree;

	[Export]
	bool Marl = true;
	[Export]
	bool Sean = true;
	[Export]
	bool Larissa = true;

	PackedScene linePrefab = GD.Load<PackedScene>("res://artstuffs/LineTextures/line_2dReel.tscn");
	Line2D Line;
	Texture2D MSL = GD.Load<Texture2D>("res://artstuffs/LineTextures/MarlSeanLari.png");
	Texture2D MS = GD.Load<Texture2D>("res://artstuffs/LineTextures/MarlSean.png");
	Texture2D ML = GD.Load<Texture2D>("res://artstuffs/LineTextures/MarlLari.png");
	Texture2D SL = GD.Load<Texture2D>("res://artstuffs/LineTextures/SeanLari.png");
	Texture2D M = GD.Load<Texture2D>("res://artstuffs/LineTextures/Marl.png");
	Texture2D S = GD.Load<Texture2D>("res://artstuffs/LineTextures/Sean.png");
	Texture2D L = GD.Load<Texture2D>("res://artstuffs/LineTextures/Lari.png");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(string.IsNullOrEmpty(eventName) || string.IsNullOrWhiteSpace(eventName))
			eventName = Name;

		if(RootNode != null)
		{
			Node lFirst = linePrefab.Instantiate();
			Line = (Line2D)lFirst;

			Vector2 midpoint = new Vector2((Position.X + RootNode.Position.X)/2, Position.Y);
			
			Line.Points = new Vector2[]{Position, midpoint, RootNode.Position};

			if(Marl && Sean && Larissa)
				Line.Texture = MSL;
			else if(Marl && Sean)
				Line.Texture = MS;
			else if(Marl && Larissa)
				Line.Texture = ML;
			else if(Sean && Larissa)
				Line.Texture = SL;
			else if (Marl)
				Line.Texture = M;
			else if (Sean)
				Line.Texture = S;
			else if (Larissa)
				Line.Texture = L;

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}




	public void UpdateDisplay()
	{

		if(RootNode != null && !RootNode.currentOutcome.Contains(eventName))
		{	
			Hide();
			return;
		}
		else
		{
			Show();
			if(StationNode != null)
				StationNode.Position = this.Position;
		}


		if(outcomesDiscovered.Count > 1){
			int currentIndex = outcomesDiscovered.LastIndexOf(currentOutcome);
			previous.Show();
			next.Show();
			if(currentIndex > 0)
				previous.Text = "<";
			else
				previous.Text = "X";
			if(currentIndex < outcomesDiscovered.Count - 1)
				next.Text = ">";
			else
				next.Text = "X";
		}else
		{
			previous.Hide();
			next.Hide();
		}

		if(blurbs.ContainsKey(currentOutcome))
			EventBlurb.Text = blurbs[currentOutcome];
		else
			EventBlurb.Text = "Unknown effect on the timeline...";

		
	
	}

	public void AdvanceOutcome()
	{
		int currentIndex = outcomesDiscovered.LastIndexOf(currentOutcome);
		if(currentIndex < outcomesDiscovered.Count - 1)
			currentOutcome = outcomesDiscovered[currentIndex + 1];
		MM.UpdateVisibleGraph();
	}

	public void RegressOutcome()
	{
		int currentIndex = outcomesDiscovered.LastIndexOf(currentOutcome);
		if(currentIndex > 0)
			currentOutcome = outcomesDiscovered[currentIndex - 1];
		UpdateDisplay();
		MM.UpdateVisibleGraph();

	}


	public void PlayDialogue()
	{
		DSM.SendDialogue(this);
		UpdateDisplay();

	}
}
