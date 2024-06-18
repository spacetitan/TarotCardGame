using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StatusManager : GridContainer
{
	[Signal] public delegate void StatusesAppliedEventHandler(StatusType type);

	public List<Status> statuses { get; private set; } = new List<Status>();

	const string STATUS_PATH = "res://Scenes/UI/StatusUI.tscn";
	const float STATUS_APPLY_INTERVAL = 0.25f;
	public Node2D owner { get; private set;}

	public override void _Ready() {}

	public void SetOwner(Node2D owner)
	{
		this.owner = owner;
	}

	public void AddStatus(Node2D target, Status status)
	{
		if(status.stackType == StatusStackType.NONE)
		{
 			GD.Print("No stack type set for status");
			return;
		}

		if(!HasStatus(status.id))
		{
			PackedScene newScene = GD.Load<PackedScene>(STATUS_PATH);
			Node newStatus = newScene.Instantiate();
			this.AddChild(newStatus);

			StatusUI statusUI = newStatus as StatusUI;
			statusUI.Reparent(this);
			statusUI.SetStatus(status);
			statusUI.status.StatusApplied += OnStatusApplied;
			statusUI.status.InitializeStatus(target);

			this.statuses.Add(status);
			return;
		}

		if(!status.canExpire && status.stackType == StatusStackType.NONE) { return; }

		if(status.canExpire && status.stackType == StatusStackType.DURATION)
		{
			GetStatus(status.id).AddDuration(status.duration);
		}

		if(status.canExpire && status.stackType == StatusStackType.INTENSITY)
		{
			GetStatus(status.id).AddStacks(status.stacks);
		}
	}

	public void ApplyStatusByType(StatusType type)
	{
		if(type == StatusType.EVENT) { return; }

		List<Status> statuses = this.statuses.FindAll(x => x.type == type);

		if(statuses == null || !statuses.Any())
		{
			EmitSignal(SignalName.StatusesApplied, (int)type);
			return;
		}

		Tween tween = CreateTween();
		foreach (Status status in statuses)
		{
			tween.TweenCallback(Callable.From(() => { status.ApplyStatus(owner); }));
			tween.TweenInterval(STATUS_APPLY_INTERVAL);
		}

		tween.Finished += () => {EmitSignal(SignalName.StatusesApplied, (int)type); };
	}

	private void OnStatusApplied(Status status)
	{
		if(status.canExpire)
		{
			status.SetDuration(status.duration-1);

			if(status.stackType == StatusStackType.INTENSITY && status.stacks < 1)
			{
				this.statuses.Remove(status);
			}
			else if(status.stackType == StatusStackType.DURATION && status.duration < 1)
			{
				this.statuses.Remove(status);
			}
		}
	}

	private bool HasStatus(StringName id)
	{
		foreach(Status status in this.statuses)
		{
			if(status.id == id)
			{
				return true;
			}
		}

		return false;
	}

	private Status GetStatus(StringName id)
	{
		foreach(Status status in this.statuses)
		{
			if(status.id == id)
			{
				return status;
			}
		}

		GD.Print("No Status Present");
		return null;
	}
}
