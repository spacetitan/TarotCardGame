using Godot;

public partial class IntentUI : HBoxContainer
{
    private TextureRect icon;
    private Label number;

    public override void _Ready()
    {
        GetSceneNodes();
    }

	private void GetSceneNodes()
	{
		this.icon = GetNode<TextureRect>("%TextureRectIcon");
		this.number = GetNode<Label>("%LabelNumber");
	}

    public void UpdateIntent(EnemyIntent intent)
    {
        if(intent == null)
        {
            Hide();
            return;
        }

        icon.Texture = intent.icon;
        icon.Visible = icon.Texture != null;
        number.Text = intent.number;
        number.Visible = intent.number.Length > 0;
        Show();
    }
}
