using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JangoFettActions : HeroActions {

    [SerializeField] private GameObject bulletProjectile;
    [SerializeField] private GameObject grenadeProjectile;
    [SerializeField] private Transform gunReferenceTransform;
    [SerializeField] private Transform grenadeReferenceTransform;

    private const float bulletTrailWaitTime = 0.4f;
    private const float grenadeWaitTimeBeforeMove = 2.1f;

    protected override void CreateHeroActions() {

        ActionVisualData basicActionVD = new(actionSprites[0],
        ActionPosition.InPlace, 1.16f, 0.1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.InPlace, 2.5f, 0.1f, true);
        ActionVisualData special2ActionVD = new(actionSprites[2],
            ActionPosition.TargetHero, 1.5f, 0.1f, true);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            BasicActionStart, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, EmptyMethod,
            Special1Start, CompleteAction, special1ActionVD);
        BaseAction special2 = new(ActionType.Special1, 3, SpecialAction2,
            StartAction, CompleteAction, special2ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1, special2 };
    }

    private void Start() {
        canRevive = true;
    }
    
    private void BasicActionStart(BaseAction performerAction) {
        StartAction(performerAction);
        Instantiate(bulletProjectile, gunReferenceTransform.position, Quaternion.identity)
            .GetComponent<BulletProjectile>()
            .Setup(heroManager.TargetHero.transform.position, bulletTrailWaitTime);
    }

    private void Special1Start(BaseAction performerAction) {
        StartAction(performerAction);
        Instantiate(grenadeProjectile, grenadeReferenceTransform)
            .GetComponent<GrenadeProjectile>()
            .Setup(Vector3.zero,grenadeWaitTimeBeforeMove,SpecialAction1);
    }

    protected override void BasicAction() {
        int debuffCounter = 0;
        heroManager.TargetHero.ActiveStatusList.ForEach(
            (status) => { if (!status.IsBuff) debuffCounter++; });

        float actualDamage = myHero.damage + myHero.damage * (0.15f * debuffCounter);

        TryGiveDamageToTarget(heroManager.TargetHero, actualDamage);
    }

    private void EmptyMethod() { }

    protected override void SpecialAction1() {
        Hero target = heroManager.TargetHero;

        Hero randomEnemy = ChooseRandomHero(false);
        while (randomEnemy == target) {
            randomEnemy = ChooseRandomHero(false);
        }
        TryGiveDamageToTarget(randomEnemy, myHero.damage);

        if (!TryGiveDamageToTarget(target, myHero.damage))
            return;
        TryApplyDebuff<HealImmunity>(target, StatusType.HealImmunity, 100);
        TryApplyDebuff<Stagger>(target, StatusType.Stagger, 100);
    }

    private void SpecialAction2() {

        var targets = heroManager.GetRivalList(myHero.IsDarkSide);

        foreach (var target in targets) {
            if(!TryGiveDamageToTarget(target,myHero.damage))
                continue;
            TryApplyDebuff<DamageOverTime>(target, StatusType.DamageOverTime, 100);
        }
    }
}
