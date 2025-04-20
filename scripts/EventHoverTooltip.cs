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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(string.IsNullOrEmpty(eventName) || string.IsNullOrWhiteSpace(eventName))
			eventName = Name;
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
