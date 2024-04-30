using Godot;

public partial class AudioPlayer : Node
{
    public void Play(AudioStream audio, bool single = false)
    {
        if(audio == null) {return;}

        if(single) {Stop();}

        foreach(AudioStreamPlayer player in GetChildren())
        {
            if(!player.Playing)
            {
                player.Stream = audio;
                player.Play();
                break;
            }
        }
    }

    public void Stop()
    {
        foreach(AudioStreamPlayer player in GetChildren())
        {
            player.Stop();
        }
    }
}