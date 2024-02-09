public class Shock : BaseStatus {
    public override StatusType StatusType => StatusType.Shock;

    public override int DefaultDuration => 1;

    public override bool IsBuff => false;

    private void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }
    /* cant gain health
     * buff
     * bonus turn meter */
}