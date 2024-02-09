
public class Stagger : BaseStatus {
    public override StatusType StatusType => StatusType.Stagger;

    public override int DefaultDuration => 2;

    public override bool IsBuff => false;

    private void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
        hero.OnDamaged += Hero_OnDamaged;
    }

    private void Hero_OnDamaged() {
        hero.SetTurnmeter(0);
        Destroy(this);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
        hero.OnDamaged -= Hero_OnDamaged;
    }

}
