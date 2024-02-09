
public class Foresight : BaseStatus
{
    public override StatusType StatusType => StatusType.Foresight;

    public override int DefaultDuration => 2;

    public override bool IsBuff => true;

    private void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }

}
