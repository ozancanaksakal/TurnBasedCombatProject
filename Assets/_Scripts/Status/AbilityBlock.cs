
public class AbilityBlock : BaseStatus {
    public override StatusType StatusType => StatusType.AbilityBlock;

    public override int DefaultDuration => 1;

    public override bool IsBuff => false;

    protected void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
        SetActionIsBlocked(true);
    }

    protected override void OnDestroy() {

        SetActionIsBlocked(false);
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
        base.OnDestroy();
    }

    private void SetActionIsBlocked(bool isBlock) {
        var actionList = hero.HeroActions.ActionList;

        foreach (BaseAction action in actionList) {
            if (action.actionType != ActionType.Basic)
                action.isBlocked = isBlock;
        }
    }

}
