using Godot;

public partial class VFXManager : Node
{
	public static VFXManager instance;

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

	public void Shake(Node2D thing, float strength, float duration = 0.2f)
    {
        if(thing == null){return;}

        Vector2 origPos = thing.Position;
        int shakeCount = 10;
        Tween tween = CreateTween();

        for(int i = 0; i < 10; i++)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            Vector2 shakeOffset = new Vector2(rng.RandfRange(-1.0f, 1.0f), rng.RandfRange(-1.0f, 1.0f));
            Vector2 target = origPos + strength * shakeOffset;

            if(i % 2 == 0)
            {
                target = origPos;
            }

            if(thing != null) 
            {
                tween.TweenProperty(thing, "position", target, duration/shakeCount);
            }
            strength *= 0.75f;
        }

        tween.Finished += () => 
        { 
            thing.Position = origPos; // causes error when player dies before finished
        };
    }
}

