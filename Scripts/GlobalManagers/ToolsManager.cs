using Godot;
using System.Collections.Generic;

public partial class ToolsManager : Node
{
	public static ToolsManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public override void _Ready()
	{
		init();
	}

	public List<Node2D> GDArrayToList(Godot.Collections.Array<Node> arrayNodes)
	{
		List<Node2D> nodes = new List<Node2D>();

		foreach(Node node in arrayNodes)
		{
			nodes.Add(node as Node2D);
		}

		return nodes;
	}

	public Color GetCharColor(CardType type)
	{
		Color color;

		switch(type)
		{
			case CardType.FIGHTER:
			color = new Color(.6f,0,0);
			break;

			case CardType.RANGER:
			color = new Color(0,.6f,0);
			break;

			case CardType.MAGE:
			color = new Color(0,0,.6f);
			break;

			default:
			color = new Color(.6f,.6f,.6f);
			break;
		}

		return color;
	}
}

