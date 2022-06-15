/*
Historically accurate educational video game based in 1830s Lausanne.
Copyright (C) 2021  GameLab UNIL-EPFL

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using Godot;
using System;

public class Map : Node2D
{
	private bool hidden = true;
	
	private AudioStreamPlayer ASP;
	private Sprite S;
	private Context context;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ASP = GetNode<AudioStreamPlayer>("../ButtonSound");
		S = GetNode<Sprite>("../TabMap");
		context = GetNode<Context>("/root/Context");
		
		context._UpdateHighlightedLocation(context._GetLocation());
	}

	public void _on_MapButton_pressed() {
		if(ASP.Playing == false) {
			ASP.Play();
		}
		if(hidden) {
			Show();
			S.Frame = 0;
		} else {
			Hide();
			S.Frame = 1;
		}
		hidden = !hidden;
	}

	public override void _Process(float delta) {
		if(Visible) {
			int currentHighlight = (int)context._GetLocation();
			if(Input.IsActionJustPressed("ui_left")) {
				context._UpdateHighlightedLocation((Locations)((currentHighlight - 1) % (int)Locations.N_LOCATIONS));
			}
			if(Input.IsActionJustPressed("ui_right")) {
				context._UpdateHighlightedLocation((Locations)((currentHighlight + 1) % (int)Locations.N_LOCATIONS));
			}
		}
	}

}


