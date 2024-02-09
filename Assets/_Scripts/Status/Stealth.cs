public class Stealth : BaseStatus {
    public override StatusType StatusType => StatusType.Stealth;

    public override int DefaultDuration => 2;

    public override bool IsBuff => false;

    private void Start() {
        HeroManager.Instance.UpdateNonSelectableList(hero, true);
        hero.HeroVisual.ShowStealthVisual();
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        HeroManager.Instance.UpdateNonSelectableList(hero,false);
        hero.HeroVisual.HideStealthVisual();
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;

    }
}
