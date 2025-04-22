using Godot;
using System;
using System.Collections.Generic;
using System.IO;

/*
[Tool]
public partial class TTCBuildTool : EditorPlugin
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override bool _Build()
    {
		if(!Engine.IsEditorHint())
			return true;
		MakeFileListings();
		return true;
	}


	void MakeFileListings()	//this should never be called on the end user's machine, and is in fact impossible to do so.
	{
		List<string> FilesStation = new List<string>();
		List<string> FilesDialogue = new List<string>();
		string DirStations = ProjectSettings.GlobalizePath("res://res/assets/stations/");
		string DirDialogues = ProjectSettings.GlobalizePath("res://res/assets/dialogues/");

		var dir = new DirectoryInfo(DirStations);
    	foreach (FileInfo file in dir.GetFiles())
    	{

			if(file.Name == "Listing.txt")
			{

			}else
			{
				FilesStation.Add(file.Name.Split(".")[0]);
			}
    	}
		dir = new DirectoryInfo(DirDialogues);
		foreach (FileInfo file in dir.GetFiles())
    	{

			if(file.Name == "Listing.txt")
			{

			}else
			{
				FilesDialogue.Add(file.Name.Split(".")[0]);
			}
    	}
		GD.Print("listed " + FilesStation.Count + " stations and " + FilesDialogue + " dialogue resources");
		File.WriteAllLines(ProjectSettings.GlobalizePath(DirStations) +"Listing.txt", FilesStation);
		File.WriteAllLines(ProjectSettings.GlobalizePath(DirDialogues) +"DListing.txt", FilesDialogue);

	}
}
*/