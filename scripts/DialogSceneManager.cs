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

    public bool InDialogue;

    EventHoverTooltip StationNode;

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


        base._Ready();
    }


    void _on_dialogue_box_dialogue_signal(string value)
    {
        string arg = "";
        if(value.Contains("|"))
            arg = value.Split("|")[1];

        switch(value)
        {
            case "advanceBackground":
                if(StationNode.backgrounds.Count > 0)
                {

                    currentbackground++;
                    currentbackground = currentbackground % StationNode.backgrounds.Count;
                    bgTexture.Texture = StationNode.backgrounds[currentbackground];
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
                StationNode.UpdateDisplay();
                break;
            case "togglePortraits":
                EmitSignal(SignalName.TogglePortraits);
                break;
            case "prepTutorial":
                int.TryParse(arg, out currentTutorial);
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

        StationNode = passStation;
        currentbackground = 0;
        if(StationNode.backgrounds.Count > 0)
        {
            bgTexture.Texture = StationNode.backgrounds[currentbackground];
            bgTexture.Show();
        }
        currentsong = 0;
        ASP.Stop();
        if(StationNode.songs.Count > 0)
        {
            ASP.Stream = StationNode.songs[currentsong];
            ASP.Play();
        }

        if(StationNode.RunFirstTimeTree)
        {
            StationNode.RunFirstTimeTree = false;
            EmitSignal(SignalName.StartDialogue, StationNode.FirstRunDialogTree);
        }
        else
            EmitSignal(SignalName.StartDialogue, StationNode.DialogTree);
    }

    public void ExitDialogue()
    {

        InDialogue = false;
        bgTexture.Hide();
        ASP.Stop();
        if(graphMusic != null)
        {
            ASP.Stream = graphMusic;
            ASP.Play();
        }
        MM.UpdateVisibleGraph();
        if(currentTutorial > -1 && currentTutorial < tutorialDialogues.Count)
            SendTutorial(currentTutorial);

    }



    public void SendTutorial(int i)
    {
        if(InDialogue)
            return;
        InDialogue = true;

        bgTexture.Hide();

        currentTutorial = -1;
        EmitSignal(SignalName.StartDialogue, tutorialDialogues[i]);
    }
    [Signal]
    public delegate void TogglePortraitsEventHandler();


    [Signal]
    public delegate void StartDialogueEventHandler(Resource DialogData);

}