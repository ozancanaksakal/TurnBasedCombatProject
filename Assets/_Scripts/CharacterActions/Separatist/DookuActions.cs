using System.Collections.Generic;
using UnityEngine;

public class DookuActions : HeroActions {
    
    private const float effectWaitTimeToStart = .65f;
    private const float lightningShowTime = .65f;

    private LightningVisual lightningVisual;

    protected override void CreateHeroActions() {

        ActionVisualData basicActionVD = new(actionSprites[0],
        ActionPosition.TargetHero, 1.5f, 1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.InPlace, 1.5f, 0.1f, true);
        ActionVisualData special2ActionVD = new(actionSprites[2],
            ActionPosition.InPlace, 2.367f, 0.1f, false);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartAction, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartActionSpecial1, CompleteAction, special1ActionVD);
        BaseAction special2 = new(ActionType.Special2, 3, SpecialAction2,
            StartAction, CompleteAction, special2ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1, special2 };
    }

    private void StartActionSpecial1(BaseAction performerAction) {
        lightningVisual.Setup(heroManager.TargetHero,
            effectWaitTimeToStart, lightningShowTime);
        StartAction(performerAction);
    }

    private void Start() {
        lightningVisual = gameObject.GetComponentInChildren<LightningVisual>(true);
    }

    protected override void BasicAction() {
        Hero target = heroManager.TargetHero;
        if (!TryGiveDamageToTarget(target, myHero.damage))
            return;

        TryApplyDebuff<Stun>(target, StatusType.Stun, 50);

        TryApplyDebuff<AbilityBlock>(target, StatusType.AbilityBlock, 50);
    }

    protected override void SpecialAction1() {
        var mainTarget = heroManager.TargetHero;
        if (!TryGiveDamageToTarget(mainTarget, myHero.damage))
            return;

        TryApplyDebuff<Stun>(mainTarget, StatusType.Stun, 80);
        Debug.Log("Choosing another enemy...");
        Hero anotherEnemy = ChooseAnotherEnemy(mainTarget);
        Debug.Log("Random enemy is  "+anotherEnemy.name);

        TryApplyDebuff<Shock>(anotherEnemy, StatusType.Shock, 50);
        TryApplyDebuff<Stun>(anotherEnemy, StatusType.Stun, 50);
    }

    private Hero ChooseAnotherEnemy(Hero mainTarget) {
        Hero randomEnemy = ChooseRandomHero(false);
        if (randomEnemy == mainTarget)
            return ChooseAnotherEnemy(mainTarget);

        return randomEnemy;
    }

    private void SpecialAction2() {

    }
}
