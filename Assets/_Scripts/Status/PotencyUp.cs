using UnityEngine;

public class PotencyUp : BaseStatus {

    private const float changeConstant = .2f;

    public override StatusType StatusType => StatusType.PotencyUp;

    public override int DefaultDuration { get { return defaultDuration; } }

    public override bool IsBuff => false;

    private int defaultDuration=2;

    private  void Start() {
        hero.HeroActions.OnActionStarted += TryDestroyStatus;
        hero.potency += (int)(hero.potency * changeConstant);
    }


    protected override void OnDestroy() {
        base.OnDestroy();
        hero.potency = hero.GetMyProperties().Potency;
        hero.HeroActions.OnActionStarted -= TryDestroyStatus;

    }

    public override void CreateInstanceOnHero(Hero hero) {
        hero.gameObject.AddComponent<PotencyUp>();
    }
}
