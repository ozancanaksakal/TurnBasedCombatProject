using UnityEngine;

public class Taunt : BaseStatus
{
    public override StatusType StatusType => StatusType.Taunt;

    public override int DefaultDuration => 2;

    public override bool IsBuff => true;


    private void Start() {
        HeroManager.Instance.UpdatePriorityTargetList(hero, true);
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        HeroManager.Instance.UpdatePriorityTargetList(hero, false);
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }
}
