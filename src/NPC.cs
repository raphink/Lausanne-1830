using Godot;
using System;

public class NPC : KinematicBody2D {
	
	private DialogueController DC;
	
	private string[] AutoDialogues;
	private int AutoDialogueIdx = 0;
	
	private string[] DemandDialogues;
	private int DemandDialogueIdx = 0;
	
	//Used to display text
	private TextBox TB;
	
	[Export]
	public string AutoDialogueID;
	[Export]
	public string DemandDialogueID;
	[Export]
	public bool HasAutoDialogue = true;
	[Export]
	public bool HasDemandDialogue = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Show();
		//Fetch the scene's Dialogue controller and the TextBox
		DC = Owner.GetNode<DialogueController>("DialogueController");
		TB = GetNode<TextBox>("TextBox");
		
		//Sanity Check
		if(HasDemandDialogue || HasAutoDialogue) {
			if(DC == null) {
				throw new Exception("Every scene must have its own dialogue controller!!");
			} 
			
			if(HasAutoDialogue) {
				if(AutoDialogueID == null) {
					throw new Exception("NPC doesn't have a dialogueID!");
				}
				//Load in the NPC's dialogue
				AutoDialogues = DC._QueryDialogue(AutoDialogueID);
			}
			
			if(HasDemandDialogue) {
				if(DemandDialogueID == null) {
					throw new Exception("NPC doesn't have a dialogueID!");
				}
				DemandDialogues = DC._QueryDialogue(DemandDialogueID);
			}
			
		}
	}

	private void _on_ListenBox_area_entered(Area2D tb) {
		if(tb.Owner is Player) {
			Player p = (Player)tb.Owner;
			//Subscribe to the player
			p._Subscribe(this);
			
			//Show auto dialogue if needed
			if(HasAutoDialogue) {
				//Fetch the right dialogue
				string d = AutoDialogues[AutoDialogueIdx];
				AutoDialogueIdx = (AutoDialogueIdx + 1) % AutoDialogues.Length;
				
				//Show it in the box
				TB._ShowText(d);
			}
		} 
	}
	
	private void _on_ListenBox_area_exited(Area2D tb) {
		if(tb.Owner is Player) {
			Player p = (Player)tb.Owner;
			//Unsubscribe to the player
			p._Unsubscribe(this);
			
			//Reset dialogue counter
			DemandDialogueIdx = 0;
			
			//Hide the text box
			TB._HideText();
		}
	}
	
	public void _Notify() {
		if(HasDemandDialogue) {
			//Check if there is any dialogue left
			if(DemandDialogueIdx < DemandDialogues.Length) {
				//Fetch the right dialogue
				string d = DemandDialogues[DemandDialogueIdx++];
				
				//Show it in the box
				TB._ShowText(d);
			} else {
				TB._HideText();
			}
		}
	}
}



