using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;


public partial class GlobalResourceManager : Node
{
	[Export]
	public Texture2D stationGraphic;

	string PackedContentPath = "res://res/assets/stations/";
	string DialogueContentPath = "res://res/assets/dialogues/";
	string PackedContentListing = "res://res/assets/stations/Listing.txt";
	string DialoguesListing = "res://res/assets/dialogues//DListing.txt";

	public List<string> Stations = new List<string>();
	public Dictionary<string, Resource> Dialogues = new Dictionary<string, Resource>();

	Resource Characters;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Characters = ResourceLoader.Load("res://res/assets/Characters.tres");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	bool SetupDiaList()
	{
		Stations.Clear();
		if(Godot.FileAccess.FileExists(DialoguesListing))
			return false;

		string[] files = Godot.FileAccess.GetFileAsString(DialoguesListing).Split("\n");
		if(files.Length == 0)
			return false;
		for(int i = 0; i < files.Length; i++)
		{
			Resource file = RetrieveTres(files[i]);
			if(file != null)
				Dialogues.Add(files[i], file);
		}
		
		if(Stations.Count < 1)
			return false;
		return true;
	}


	bool SetupStationList()
	{
		Stations.Clear();
		if(Godot.FileAccess.FileExists(PackedContentListing))
			return false;

		string[] files = Godot.FileAccess.GetFileAsString(PackedContentListing).Split("\n");
		if(files.Length == 0)
			return false;
		for(int i = 0; i < files.Length; i++)
		{
			string file = RetrieveFile(files[i]);
			if(file.Length > 0)
				Stations.Add(file);
		}
		
		if(Stations.Count < 1)
			return false;
		return true;
	}


	
	string RetrieveFile(string stationName)
	{
		GD.Print("retrieving file " + stationName);
		//Now we have greater control over loading these from places other than user stuff.
		string returnedFile = "";


		GD.Print("Checking path " + PackedContentPath + stationName + ".txt");
		if(Godot.FileAccess.FileExists(PackedContentPath + stationName + ".txt"))
				returnedFile = Godot.FileAccess.GetFileAsString(PackedContentPath + stationName + ".txt").Replace("\r", "");

		if(returnedFile == null)
			returnedFile = "";
		return returnedFile;
	}


	Resource RetrieveTres(string resName)
	{
		GD.Print("retrieving file " + resName);
		//Now we have greater control over loading these from places other than user stuff.
		Resource returnedFile = null;


		GD.Print("Checking path " + DialogueContentPath + resName + ".tres");
		if(Godot.FileAccess.FileExists(DialogueContentPath + resName + ".tres"))
				returnedFile = ResourceLoader.Load(DialogueContentPath + resName + ".tres");

		return returnedFile;
	}
}
