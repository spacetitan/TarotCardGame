using Godot;

public partial class EnemyActionManager : Node
{
	public Enemy body { get; private set;}

	public EnemyActionManager(Enemy enemy)
	{
		this.body = enemy;
	}

	public override void _Ready()
	{
	}
}

public partial class EnemyAction : Node
{

}
