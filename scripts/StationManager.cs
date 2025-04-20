using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StationManager : Control
{

	/*

	Dictionary<string, Station> stations = new Dictionary<string, Station>();
	List<string> stationNames = new List<string>();
	Dictionary<string, string> stationInroads = new Dictionary<string, string>();

	List<TextureRect> stationNodes = new List<TextureRect>();
	List<EventHoverTooltip> toolTips = new List<EventHoverTooltip>();

	[Export]
	public DialogSceneManager DSM;
	[Export]
	public GlobalResourceManager GRM;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UpdateGraph();
		SetupTooltips();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadStations()
	{
		
	}

	public void SetupTooltips()
	{
		for(int i = 0; i < toolTips.Count; i++)
		{
			if(stations.ContainsKey(toolTips[i].StationToken))
			{
				toolTips[i].SetEvent(stationNodes[i], stations[toolTips[i].StationToken], this);
			}
		}
	}

	public void UpdateGraph()
	{
		stationInroads.Clear();
		for(int i = 0; i < stations.Count; i++)
		{
			if(stations[stationNames[i]].name == "start")
			{
				stations[stationNames[i]].accessable = true;
				stations[stationNames[i]].visible = true;
			}
			stationInroads.Add(stations[stationNames[i]].currentOutcome, stationNames[i]);
		}

		for(int i = 0; i < stations.Count; i++)
		{
			if(stations[stationNames[i]].name == "start"  || stationInroads.ContainsKey(stations[stationNames[i]].name))
			{
				stations[stationNames[i]].accessable = true;
				stations[stationNames[i]].visible = true;

			}else if(stationInroads.ContainsKey(stations[stationNames[i]].name))
			{
				stations[stationInroads[stationNames[i]]].outcomeDiscoverable[stationNames[i]] = true;
				if(stations[stationInroads[stationNames[i]]].outcomeDiscovered[stationNames[i]])
				{
					stations[stationInroads[stationNames[i]]].outcomeDiscovered[stationNames[i]] = true;
					stations[stationNames[i]].accessable = true;
					stations[stationNames[i]].visible = true;

				}else
				{
					stations[stationNames[i]].accessable = false;
					stations[stationNames[i]].visible = false;

				}
			}
			else{
				stations[stationNames[i]].accessable = false;
				stations[stationNames[i]].visible = false;

			}
		}



	}


	public void PlayDialogue(string dia)
	{
		DSM.SendDialogue(stations[dia]);
	}
	*/
}



public class Station
{
	//We need for this station a link to the tres file for the conversation(s), a list of requirements (or several if there are multiple start conditions), and a list of outcomes.
	//We also need a dictionary of keyed values kinda like a JSON
	//But let's start
	public bool LoadedCorrectly;
	public string name = "null";
	public Dictionary<string, string> resourceKey = new Dictionary<string, string>();
	public string[] outcomeName;	//point at these outcomes for each entry
	public Dictionary<string, string> outcomeTags = new Dictionary<string, string>();	//For every outcome, a string containing tags seperated by | dividers.
	public bool accessable = false;
	public bool visible = false;
	public string currentOutcome = "-1";	//-1 = no visible exit
	public Dictionary<string, bool> outcomeDiscovered = new Dictionary<string, bool>();	//For every outcome, does the blurb display?
	public Dictionary<string, bool> outcomeDiscoverable = new Dictionary<string, bool>();		//For every outcome, can it be scrolled to?
	public Dictionary<string, string> outcomeBlurb = new Dictionary<string, string>();		//For every outcome, a blurb to display on the event node
	public int playedThrough = 0;
	public int priority = 0;

	Station(string file)
	{
		LoadedCorrectly = false;
		string[] lines = file.Split('\n');

		for(int i = 0; i < lines.Length; i++)
		{
			string lineLower = lines[i].ToLower();
			if(string.IsNullOrEmpty(lines[i]) || string.IsNullOrWhiteSpace(lines[i]))
				continue;
			if(lines[i].Contains(":"))
				switch(lineLower.Split(":")[0])
				{
					case "name":
						if(lines[i].Split(":").Length > 1)
							name = lines[i].Split(":")[1];
						else
							name = "null";
							
						break;
					case "res":
						if(outcomeName.Length < 1 )
							throw new Exception("Outcomes names need to be at the top of the file");
						Dictionary<string, string> resourceKeys = new Dictionary<string, string>();
						i++;
						while(!lines[i].ToLower().Contains("res"))
						{
							resourceKeys.Add(outcomeName[i], lines[i]);
							i++;
						}
						resourceKey = resourceKeys;
						break;
					case "priority":
						if(outcomeName.Length < 1 )
							throw new Exception("Outcomes names need to be at the top of the file");

						if(lines[i].Split(":").Length > 1)
							if(!int.TryParse(lines[i].Split(":")[1], out priority))
								priority = 0;
						else
							priority = 0;
						break;
					case "outcomeblurbs":
						if(outcomeName.Length < 1 )
							throw new Exception("Outcomes names need to be at the top of the file");
						Dictionary<string, string> outcomeBlurbs = new Dictionary<string, string>();
						i++;
						while(!lines[i].ToLower().Contains("outcome"))
						{
							outcomeBlurbs.Add(outcomeName[i], lines[i]);
							i++;
						}
						outcomeBlurb = outcomeBlurbs;
						break;
					case "outcomenames":
						List<string> outcomeNames = new List<string>();
						i++;
						while(!lines[i].ToLower().Contains("outcome"))
						{
							outcomeDiscovered.Add(lines[i], false);
							if(outcomeNames.Count == 0)
								outcomeDiscoverable.Add(lines[i], true);
							else
								outcomeDiscoverable.Add(lines[i], false);


							outcomeNames.Add(lines[i]);
							i++;
						}
						outcomeName = outcomeNames.ToArray();
						break;
					case "outcometags":
						if(outcomeName.Length < 1 )
							throw new Exception("Outcomes names need to be at the top of the file");

						Dictionary<string, string> outcomeTagses = new Dictionary<string, string>();
						i++;
						while(!lines[i].ToLower().Contains("outcome"))
						{
							outcomeTagses.Add(outcomeName[i], lines[i]);
							i++;
						}
						outcomeTags = outcomeTagses;
						break;
					default:
						break;
				}
		}
		currentOutcome = outcomeName[0];
		if(currentOutcome == "end")
			LoadedCorrectly = true;
		if(name != "null" && resourceKey.Keys.Count == outcomeName.Length && outcomeName.Length > 0)
			LoadedCorrectly = true;

		if(!LoadedCorrectly)
			return;
	}


}