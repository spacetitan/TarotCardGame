using Godot;

public partial class Enemy : Area2D
{
	const float ARROW_OFFSET = 200;
	Material whiteSprite;

	[Export] public EnemyStats stats { get; private set; }
	[Export] public EnemyAction[] actions { get; private set; }
	private Sprite2D enemySprite;
	private Sprite2D arrowSprite;
	private StatsUI statsUI;
	private IntentUI intentUI;

	private EnemyActionManager actionManager;
	private EnemyAction currentAction;

	public override void _Ready()
	{
		GetSceneNodes();

		if(this.stats != null)
		{
			SetEnemyStats(this.stats);
		}
	}

	private void GetSceneNodes()
	{
		this.enemySprite = GetNode<Sprite2D>("%Sprite2DEnemy");
		this.arrowSprite = GetNode<Sprite2D>("%Sprite2DArrow");
		this.statsUI = GetNode<StatsUI>("%StatsUI");
		this.intentUI = GetNode<IntentUI>("%IntentUI");
	}

	public void SetEnemyStats(EnemyStats enemyStats)
	{
		this.stats = enemyStats.CreateInstance();
		this.stats.StatsChanged += UpdateEnemy;

		InititalizeUI();
		InititalizeAI();
		UpdateEnemy();
	}

	public void InititalizeUI()
	{
		this.enemySprite.Texture = this.stats.art;
		this.arrowSprite.Position = Vector2.Up * (this.enemySprite.GetRect().Size.X/2 + ARROW_OFFSET);
	}

	private void InititalizeAI()
	{
		if(this.actionManager != null)
		{
			this.actionManager.QueueFree();
		}

		this.actionManager = new EnemyActionManager(this);
	}

	public void UpdateEnemy()
	{
		this.statsUI.UpdateStats(this.stats);

		if(this.actionManager == null)
		{
			return;
		}

		if(currentAction == null)
		{
			SetCurrentAction(this.actionManager.GetAction());
			return;
		}

		EnemyAction newConditionalAction = this.actionManager.GetFirstConditionalAction();
		if(newConditionalAction != null && this.currentAction != newConditionalAction)
		{
			SetCurrentAction(newConditionalAction);
		}
	}

	public void SetCurrentAction(EnemyAction value)
	{
		this.currentAction = value;
		if(this.currentAction != null)
		{
			this.intentUI.UpdateIntent(currentAction.intent);
		}
	}

	public void TakeTurn()
	{
		this.stats.ResetArmor();

		if(this.currentAction == null)
		{
			GD.Print("Current Action is null");
			return;
		}

		this.currentAction.PerformAction();
	}

	public void TakeDamage(int damage)
	{
		if(stats.health <=0)
		{
			return;
		}

		//sprite2D.Material = WHITE_SPRITE_MATERIAL;

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(()=>{VFXManager.instance.Shake(this, 16, .15f);}));
		tween.TweenCallback(Callable.From(()=>{stats.TakeDamage(damage);}));
		tween.TweenInterval(0.17f);
		tween.Finished += ()=>
		{
			this.enemySprite.Material = null;

			if(stats.health <=0)
			{
				//QueueFree();
			}
		};
	}

	void OnAreaEntered(Area2D area2D)
	{
		this.arrowSprite.Show();
	}

	void OnAreaExited(Area2D area2D)
	{
		this.arrowSprite.Hide();
	}
}
