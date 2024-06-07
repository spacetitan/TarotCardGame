using System;
using Godot;

public partial class Enemy : Area2D
{
	const float ARROW_OFFSET = 70;
	Material whiteSprite;

	[Export] public EnemyStats stats { get; private set; }
	private Sprite2D enemySprite;
	private Sprite2D arrowSprite;
	private StatsUI statsUI;
	private IntentUI intentUI;

	private EnemyActionManager actionManager;
	private EnemyAction currentAction;

	private Material WHITE_SPRITE_MATERIAL = ResourceLoader.Load<Material>("res://Materials/DamageMaterial.tres");
	private Material GREEN_SPRITE_MATERIAL = ResourceLoader.Load<Material>("res://Materials/HealMaterial.tres");

	const String STATUS_MANAGER_SCENE = "res://Scenes/GamePlay/StatusManager.tscn";
	public StatusManager statusManager { get; private set; } //instantiate in scene

	public override void _Ready()
	{
		GetSceneNodes();
		SpawnManagers();

		if(this.stats != null)
		{
			SetEnemyStats(this.stats);
		}

		ConnectEventSignals();
	}

	private void GetSceneNodes()
	{
		this.enemySprite = GetNode<Sprite2D>("%Sprite2DEnemy");
		this.arrowSprite = GetNode<Sprite2D>("%Sprite2DArrow");
		this.statsUI = GetNode<StatsUI>("%StatsUI");
		this.intentUI = GetNode<IntentUI>("%IntentUI");
	}

	private void SpawnManagers()
	{
		PackedScene scene = GD.Load<PackedScene>(STATUS_MANAGER_SCENE);
		Node newScene = scene.Instantiate();
		this.AddChild(newScene);
		this.statusManager = newScene as StatusManager;
		this.statusManager.SetOwner(this);
	}

	private void ConnectEventSignals()
	{

	}

	private void DisconnectEventSignals()
	{
		this.stats.StatsChanged -= UpdateEnemy;
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

		this.enemySprite.Material = WHITE_SPRITE_MATERIAL;

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(()=>{VFXManager.instance.Shake(this, 16, .15f);}));
		tween.TweenCallback(Callable.From(()=>{stats.TakeDamage(damage);}));
		tween.TweenInterval(0.17f);
		tween.Finished += ()=>
		{
			this.enemySprite.Material = null;

			if(stats.health <=0)
			{
				DestroyEnemy();
			}
		};
	}

	public void Heal(int value)
	{
		this.enemySprite.Material = GREEN_SPRITE_MATERIAL;

		Tween tween = CreateTween();
		tween.TweenCallback(Callable.From(()=>{VFXManager.instance.Shake(this, 16, .15f);}));
		tween.TweenCallback(Callable.From(()=>{stats.Heal(value);}));
		tween.TweenInterval(0.17f); // this is the interval time
		tween.Finished += ()=>
		{
			this.enemySprite.Material = null;

			if(stats.health <=0)
			{
				DestroyEnemy();
			}
		};
	}

	private void DestroyEnemy()
	{
		EventManager.instance.EmitSignal(EventManager.SignalName.EnemyDied, this);
		DisconnectEventSignals();
		QueueFree();
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
