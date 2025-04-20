using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;

public partial class StationRes : Resource
{
    [Export]
    string name;
    [Export]
    Godot.Collections.Array<EventRes> outcomes = new Godot.Collections.Array<EventRes>();
    [Export]
    Resource dialogue;
    
}
