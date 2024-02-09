public class OffenseUp : BaseStatus {
    private const float changeConstant = .2f;
    public override StatusType StatusType => StatusType.OffenseUp;

    public override int DefaultDuration => 2;

    public override bool IsBuff => true;

    private void Start() {
            hero.HeroActions.OnActionStarted += TryDestroyStatus;
            hero.damage += (int)(hero.damage * changeConstant);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.damage = hero.GetMyProperties().Damage;
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }
}
