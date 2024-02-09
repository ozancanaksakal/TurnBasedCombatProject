public class Riposte : BaseStatus {
    public override StatusType StatusType => StatusType.Riposte;

    public override int DefaultDuration => 2;

    public override bool IsBuff => true;

    private void Start() {
        HeroActions.OnAnyActionStarted += HeroActions_OnAnyActionStarted;
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }

    private void HeroActions_OnAnyActionStarted() {
        HeroManager.Instance.AddToQueue(hero);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        HeroActions.OnAnyActionStarted -= HeroActions_OnAnyActionStarted;
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }
}