using Godot;

public partial class AudioManager : Node
{
	public static AudioManager instance;

	public void init()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	public AudioPlayer musicPlayer { get; private set;}
	public AudioPlayer sfxPlayer { get; private set;}

	public override void _Ready()
	{
		init();

		this.musicPlayer = GetNode<AudioPlayer>("%MusicPlayer");
		this.sfxPlayer = GetNode<AudioPlayer>("%SFXPlayer");
	}
}

