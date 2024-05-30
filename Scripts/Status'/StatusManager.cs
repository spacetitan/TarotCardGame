using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StatusManager : GridContainer
{
	[Signal] public delegate void StatusesAppliedEventHandler(StatusType type);

	const string STATUS_PATH = "";
	const float STATUS_APPLY_INTERVAL = 0.25f;
	public Node2D owner { get; private set;}

	public override void _Ready() {}

	public void SetOwner(Node2D owner)
	{
		this.owner = owner;
	}

	private void SpawnStatusUI(Node2D target, Status status)
	{
		if(status.stackType == StatusStackType.NONE)
		{
 			GD.Print("No stack type set for status");
			return;
		}

		StatusStackType stackType = status.stackType;

		if(!HasStatus(status.id))
		{
			var cardScene = GD.Load<PackedScene>(STATUS_PATH);
			var newStatus = cardScene.Instantiate();
			AddChild(newStatus);

			StatusUI statusUI = newStatus as StatusUI;
			statusUI.Reparent(this);
			statusUI.SetUI(status);
			statusUI.status.StatusApplied += OnStatusApplied;
			statusUI.status.InitializeStatus(target);
			return;
		}

		if(!status.canExpire && status.stackType == StatusStackType.NONE) { return; }

		if(status.canExpire && status.stackType == StatusStackType.DURATION)
		{
			GetStatus(status.id).AddDuration(status.duration);
		}

		if(status.canExpire && status.stackType == StatusStackType.INTENSITY)
		{
			GetStatus(status.id).AddStacks(status.duration);
		}
	}

	public void ApplyStatusByType(StatusType type)
	{
		if(type == StatusType.EVENT) { return; }

		List<Status> statuses = GetAllStatuses().FindAll(x => x.type == type);

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
		}
	}

	private bool HasStatus(StringName id)
	{
		foreach(StatusUI statusUI in GetChildren())
		{
			if(statusUI.status.id == id)
			{
				return true;
			}
		}

		return false;
	}

	private Status GetStatus(StringName id)
	{
		foreach(StatusUI statusUI in GetChildren())
		{
			if(statusUI.status.id == id)
			{
				return statusUI.status;
			}
		}

		GD.Print("No Status Present");
		return null;
	}

	private List<Status> GetAllStatuses()
	{
		List<Status> statuses = new List<Status>();

		foreach(StatusUI statusUI in GetChildren())
		{
			statuses.Add(statusUI.status);
		}

		return statuses;
	}
}
