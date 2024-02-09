
public class HealImmunity : BaseStatus
{
    public override StatusType StatusType => StatusType.HealImmunity;

    public override int DefaultDuration => 2;

    public override bool IsBuff => false;

    private void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }
}
