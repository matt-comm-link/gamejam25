using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DialogSceneManager : Control
{
    [Export]
    TextureRect bgTexture;
    [Export]
    Control DialogueBox;
    [Export]
    AudioStreamPlayer2D ASP;
    [Export]
    AudioStream graphMusic;
    [Export]
    MapManager MM;
    [Export]
    ScalingBaqckground SB;
    [Export]
    ScrollContainer SC;
    [Export]
    Control db;
    [Export]
    LevelManager lm;
    [Export]
    MarginContainer mc;

    public bool CanEndLevel = false;

    public bool InDialogue;

    Texture2D blank = GD.Load<Texture2D>("res://artstuffs/LineTextures/blank.png");

    EventHoverTooltip StationNode;
    [Export]
    Godot.Collections.Array<Resource> tutorialDialogues = new Godot.Collections.Array<Resource>();

    //Station eventStation;
    int currentbackground = 0;
    int currentsong = 0;
    int currentTutorial = -1;

    public override void _Ready()
    {
        if(graphMusic != null)
        {
            ASP.Stream = graphMusic;
            ASP.Play();
        }
        Vector2 dbsize = GetViewportRect().Size;
        dbsize.Y /= 2;
        db.CustomMinimumSize = dbsize;
        SC.CustomMinimumSize = GetViewportRect().Size;
        lm = GetNode("/root/LevelManager") as LevelManager;
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if(Input.IsMouseButtonPressed(MouseButton.Left))
        {
            if(CanEndLevel && !InDialogue)
            {
                if(lm != null)
                    lm.LoadNextLevel();
                else
                    GD.Print("reached the end of available levels");
            }
        }
    }

    void _on_dialogue_box_dialogue_signal(string value)
    {
        GD.Print("recieving signal " + value);
        string arg = "";
        if(value.Contains("|"))
        {
            arg = value.Split("|")[1];
            value = value.Split("|")[0];
        }

        switch(value)
        {
            case "advanceBackground":
                if(StationNode.backgrounds.Count > 0)
                {

                    currentbackground++;
                    currentbackground = currentbackground % StationNode.backgrounds.Count;
                    bgTexture.Texture = StationNode.backgrounds[currentbackground];
                    SB.ResizeBG();
                }
                break;
            case "advanceSong":
                if(StationNode.songs.Count > 0)
                {
                    currentsong++;
                    currentsong = currentsong % StationNode.songs.Count();
                    ASP.Stop();
                    ASP.Stream = StationNode.songs[currentsong];
                    ASP.Play();
                }
                break;
            case "mute":
                ASP.Stop();
                break;

            case "play":
                ASP.Play(0);                
                break;

            case "setOutcome":
                StationNode.currentOutcome = arg;
                if(!StationNode.outcomesDiscovered.Contains(arg))
                    StationNode.outcomesDiscovered.Add(arg);
                StationNode.UpdateDisplay();
                GD.Print("station node exit is now " + StationNode.currentOutcome);
                break;
            case "togglePortraits":
                EmitSignal(SignalName.TogglePortraits);
                break;
            case "prepTutorial":
                GD.Print("trying to set current tutorial to " + arg);
                int.TryParse(arg, out currentTutorial);
                break;
            case "setGlobal":
                if(arg.Contains("=")){
                    string argKey = arg.Split("=")[0];
                    string argValue = arg.Split("=")[1];

                    if(MM.globals.ContainsKey(argKey))
                    {
                        MM.globals[argKey] = argValue;
                    }else
                    {
                        MM.globals.Add(argKey, argValue);
                    }
                }
                break;

            default:
                break;
        }

        
    }


    public void SendDialogue(EventHoverTooltip passStation)
    {
        if(InDialogue)
            return;
        InDialogue = true;

        SC.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
        SC.VerticalScrollMode = ScrollContainer.ScrollMode.Disabled;


        StationNode = passStation;
        currentbackground = 0;
        if(StationNode.backgrounds.Count > 0)
        {
            bgTexture.Texture = StationNode.backgrounds[currentbackground];
            bgTexture.Show();
            SB.ResizeBG();

        }else
        {
            bgTexture.Texture = blank;
            bgTexture.Show();
            SB.ResizeBG();
        }
        bgTexture.MouseFilter = MouseFilterEnum.Pass;
        currentsong = 0;
        ASP.Stop();
        if(StationNode.songs.Count > 0)
        {
            ASP.Stream = StationNode.songs[currentsong];
            ASP.Play();
        }

        SC.Hide();
        if(StationNode.RunFirstTimeTree)
        {
            foreach(KeyValuePair<string, string> variable in MM.globals)
            {
                StationNode.FirstRunDialogTree.Set(variable.Key, variable.Value);
            }
            db.Set("globalVariables", MM.globals);
            StationNode.RunFirstTimeTree = false;
            EmitSignal(SignalName.StartDialogue, StationNode.FirstRunDialogTree);
        }
        else
        {
            db.Set("globalVariables", MM.globals);
            EmitSignal(SignalName.StartDialogue, StationNode.DialogTree);
        }
    }

    public void ExitDialogue()
    {
        MM.UpdateVisibleGraph();
        if(StationNode != null && StationNode.currentOutcome == "end")
        {
            GD.Print("It is now possible to end the level");
            CanEndLevel = true;
        }

        InDialogue = false;
        bgTexture.Hide();
        ASP.Stop();
        if(graphMusic != null)
        {
            ASP.Stream = graphMusic;
            ASP.Play();
        }
        bgTexture.Texture = blank;
        bgTexture.Hide();
        SB.ResizeBG();
        mc.Position = new Vector2(-100.8f, 461.781f);

        SC.Show();
        if(currentTutorial > -1 && currentTutorial < tutorialDialogues.Count)
            SendTutorial(currentTutorial);
        SC.HorizontalScrollMode = ScrollContainer.ScrollMode.Auto;
        SC.VerticalScrollMode = ScrollContainer.ScrollMode.Auto;

    }



    public void SendTutorial(int i)
    {
        if(InDialogue)
            return;
        InDialogue = true;
        //SC.Hide();

        bgTexture.Texture = blank;
        bgTexture.Show();
        SB.ResizeBG();    
        db.Set("globalVariables", MM.globals);

        SC.HorizontalScrollMode = ScrollContainer.ScrollMode.Auto;
        SC.VerticalScrollMode = ScrollContainer.ScrollMode.Auto;
        db.SetPosition(new Vector2(0, 0));
        currentTutorial = -1;
        bgTexture.MouseFilter = MouseFilterEnum.Ignore;
        mc.Position = new Vector2(-100.8f, 461.781f);

        GD.Print("Playing tutorial dialogue " + i);
        EmitSignal(SignalName.StartDialogue, tutorialDialogues[i]);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseButton)
        {
            if(mouseButton.ButtonIndex == 0 && CanEndLevel && !InDialogue)
            {
                if(lm != null)
                    lm.LoadNextLevel();
                else
                    GD.Print("reached the end of available levels");
            }
        }
    }


    [Signal]
    public delegate void TogglePortraitsEventHandler();


    [Signal]
    public delegate void StartDialogueEventHandler(Resource DialogData);

}