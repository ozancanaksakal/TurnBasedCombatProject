
public class BuffImmunity : BaseStatus {
    public override StatusType StatusType => StatusType.BuffImmunity;

    public override int DefaultDuration => 2;

    public override bool IsBuff => false;

    private void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }
}