using Godot;
using System;

public partial class EventRes : Resource
{
    [Export]
    string outcomeTag;
    [Export]
    string outcomeStationName;
    [Export]
    string outcomeBlurb;

    [Export]
    public Godot.Collections.Dictionary<string, string> globals = new Godot.Collections.Dictionary<string, string>();

}
