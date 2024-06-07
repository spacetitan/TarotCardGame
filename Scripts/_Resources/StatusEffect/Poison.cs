using Godot;

[GlobalClass]
public partial class Poison : Status
{
    public override void ApplyStatus(Node target)
    {
        DamageEffect damageEffect = new DamageEffect(this.stacks, this.sfx);
        damageEffect.Execute(target as Node2D);

        this.SetStacks(this.stacks-1);

        base.ApplyStatus(target);
    }
}
