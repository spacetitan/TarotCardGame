using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AbilityTargetSelector : Node2D
{
	const int ARC_POINTS = 9;

	private Sprite2D targetSprite;
	private Area2D targetArea;
	private Line2D cardArc;

	private Player player;
	private Ability ability;
	bool targeting = false;
	public List<Node2D> targets { get; private set; } = new List<Node2D>();

	public override void _Ready()
	{
		GetSceneNodes();

		EventManager.instance.PlayerAbilityAimStarted += OnAbilityAimStarted;
		EventManager.instance.PlayerAbilityAimEnded += OnAbilityAimEnded;
	}

	private void GetSceneNodes()
	{
		this.targetSprite = GetNode<Sprite2D>("%Sprite2DTargeter");
		this.targetArea = GetNode<Area2D>("%Area2DTargetArea");
		this.cardArc = GetNode<Line2D>("%Line2DTargetArc");
	}

	public override void _Process(double delta)
	{
		if(!targeting)
		{
			return;
		}

		this.targetSprite.Position = GetLocalMousePosition();
		this.targetArea.Position = GetLocalMousePosition();
		cardArc.Points = GetPoints();
	}

	Vector2[] GetPoints()
	{
		List<Vector2> points = new List<Vector2>();
		Vector2 start = player.GlobalPosition;
		Vector2 target = GetGlobalMousePosition();
		Vector2 distance = target - start;

		for(int i = 0; i < ARC_POINTS; i++)
		{
			float t = 1.0f/ARC_POINTS * i;
			float x = start.X + distance.X / ARC_POINTS * i;
			float y = start.Y + EaseOutCubic(t) * distance.Y;
			points = points.Append(new Vector2(x,y)).ToList();
		}

		points.Append(target);

		return points.ToArray();
	}

	float EaseOutCubic(float number)
	{
		return 1.0f - MathF.Pow(1.0f - number, 3.0f);
	}

	void OnAbilityAimStarted(Player player)
	{
		if(player.stats.ability.targetType != Target.SINGLE)
		{
			return;
		}

		this.targeting = true;
		this.targetArea.Monitoring = true;
		this.targetArea.Monitorable = true;
		this.targetSprite.Visible = true;
		this.player = player;
		this.ability = player.stats.ability;
	}

	void OnAbilityAimEnded()
	{
		this.targeting = false;
		cardArc.ClearPoints();
		this.targetArea.Position = Vector2.Zero;
		this.targetArea.Monitoring = false;
		this.targetArea.Monitorable = false;
		this.targetSprite.Visible = false;
		this.ability = null;
	}

	void OnGuiInput(InputEvent inputEvent)
	{
		if(this.ability == null || !targeting)
		{
			return;
		}

		if(inputEvent.IsActionPressed("LeftMouse"))
		{
			if(this.targets == null || this.targets.Count < 1) 
			{
				OnAbilityAimEnded();
				return; 
			} 

			this.ability.ApplyEffects(this.targets, this.player.stats);
			OnAbilityAimEnded();	
		}
		else if(inputEvent.IsActionPressed("RightMouse"))
		{
			OnAbilityAimEnded();
		}
	}

	void OnArea2DEntered(Area2D area)
	{
		if(this.ability == null || !targeting)
		{
			return;
		}

		if(!targets.Contains(area as Node2D))
		{
			this.targets.Add(area as Node2D);
		}
	}
	void OnArea2DExited(Area2D area)
	{
		if(this.ability == null || !targeting)
		{
			return;
		}

		this.targets.Remove(area as Node2D);
	}
}
