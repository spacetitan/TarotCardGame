using Godot;
using System;
using System.Collections.Generic;

public partial class ChooseCharacterUI : CanvasLayer
{
	[Export] PlayerStats[] classes;
	//private List<PlayerStats> classList = new List<PlayerStats>();
	const string CARD_UI_PATH = "res://Scenes/UI/CharacterPicker.tscn";

	private HBoxContainer pickerParent;

	public override void _Ready()
	{
		GetSceneNodes();

		DelayedSpawn();
	}

	private void GetSceneNodes()
	{
		this.pickerParent = GetNode<HBoxContainer>("%HBoxContainerPicker");
	}

	private void DelayedSpawn()
	{
		Tween tween = CreateTween();
		tween.TweenInterval(0.15f);
		tween.Finished += ()=>
		{
			SpawnCards();
		};
	}

	public void SpawnCards()
	{
		foreach(PlayerStats stats in this.classes)
		{
			var scene = GD.Load<PackedScene>(CARD_UI_PATH);
			var newCard = scene.Instantiate();
			this.pickerParent.AddChild(newCard);

			CharacterPickerUI characterPicker = newCard as CharacterPickerUI;
			characterPicker.SetUI(stats);
		}
	}
}
