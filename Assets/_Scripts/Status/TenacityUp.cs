public class TenacityUp : BaseStatus {

    private const float changeConstant = .2f;
    public override StatusType StatusType => StatusType.TenacityUp;

    public override int DefaultDuration => 2;

    public override bool IsBuff => true;

    private void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
        hero.tenacity += (int)(hero.tenacity * changeConstant);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.tenacity = hero.GetMyProperties().Tenacity;
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }
}
