
using UnityEngine;

public class PotencyDown : BaseStatus {

    private const float changeConstant = .2f;

    public override StatusType StatusType => StatusType.PotencyDown;

    public override int DefaultDuration => defaultDuration;

    public override bool IsBuff => false;

    private int defaultDuration=2;

    protected  void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
        hero.potency -= (int)(hero.potency * changeConstant);
    }
   

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.potency = hero.GetMyProperties().Potency;
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;
    }

}
