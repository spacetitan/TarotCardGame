using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CardTargetSelector : Node2D
{
	const int ARC_POINTS = 9;

	private Sprite2D targetSprite;
	private Area2D targetArea;
	private Line2D cardArc;

	private CardUI currentCard;
	bool targeting = false;

	public override void _Ready()
	{
		GetSceneNodes();

		EventManager.instance.CardAimStarted += OnCardAimStarted;
		EventManager.instance.CardAimEnded += OnCardAimEnded;
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
		Vector2 start = currentCard.GlobalPosition;
		start.X += currentCard.Size.X/2;
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

	void OnCardAimStarted(CardUI card)
	{
		if(!card.cardStats.isSingleTargeted())
		{
			return;
		}

		this.targeting = true;
		this.targetArea.Monitoring = true;
		this.targetArea.Monitorable = true;
		this.targetSprite.Visible = true;
		currentCard = card;
	}

	void OnCardAimEnded()
	{
		this.targeting = false;
		cardArc.ClearPoints();
		this.targetArea.Position = Vector2.Zero;
		this.targetArea.Monitoring = false;
		this.targetArea.Monitorable = false;
		this.targetSprite.Visible = false;
		currentCard = null;
	}

	void OnArea2DEntered(Area2D area)
	{
		if(currentCard == null || !targeting)
		{
			return;
		}

		if(!currentCard.targets.Contains(area as Node2D))
		{
			currentCard.targets.Add(area as Node2D);
		}
	}
	void OnArea2DExited(Area2D area)
	{
		if(currentCard == null || !targeting)
		{
			return;
		}

		currentCard.targets.Remove(area as Node2D);
	}
}
