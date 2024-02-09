using System.Collections.Generic;

public class PalpatineActions : HeroActions {

    private LightningVisual[] lightningVisualArray;
    private (float Basic, float Special1) effectWaitTimes = (1.25f, 1.1f);
    private (float Basic, float Special1) effectShowTimes = (0.75f, 2.2f);

    protected override void CreateHeroActions() {
        ActionVisualData basicActionVD = new(actionSprites[0],
        ActionPosition.InPlace, 2.7f, 1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.InPlace, 4.3f, 0.1f, true);
        ActionVisualData special2ActionVD = new(actionSprites[2],
            ActionPosition.InPlace, 2.367f, 0.1f, false);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartActionLightningVisual, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartActionLightningVisual, CompleteAction, special1ActionVD);
        BaseAction special2 = new(ActionType.Special2, 3, SpecialAction2,
            StartAction, CompleteAction, special2ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1, special2 };
    }

    private void Start() {
        lightningVisualArray = GetComponentsInChildren<LightningVisual>(true);
    }

    private void StartActionLightningVisual(BaseAction performerAction) {
        List<Hero> targetList = performerAction.actionType == ActionType.Basic ?
            new List<Hero> { heroManager.TargetHero } :
            heroManager.GetRivalList(myHero.IsDarkSide);
        EffectSetup(targetList, performerAction.actionType);
        StartAction(performerAction);
    }

    private void EffectSetup(List<Hero> targets, ActionType actionType) {

        float waitTime = actionType == ActionType.Basic ?
                effectWaitTimes.Basic : effectWaitTimes.Special1;

        float showTime = actionType == ActionType.Basic ?
                effectShowTimes.Basic : effectShowTimes.Special1;

        for (int i = 0; i < targets.Count; i++) {
            lightningVisualArray[i].Setup(targets[i], waitTime, showTime);
        }
    }

    protected override void BasicAction() {
        Hero target = heroManager.TargetHero;

        if (!TryGiveDamageToTarget(target, myHero.damage))
            return;

        TryApplyDebuff<Shock>(target, StatusType.Shock, 100);

        int shockCounter = 0;
        foreach (Hero enemy in heroManager.GetRivalList(myHero.IsDarkSide)) {
            if (HasStatus(enemy, StatusType.Shock)) shockCounter++;
        }
        myHero.ChangeTurnmeter(15 * shockCounter);
    }

    protected override void SpecialAction1() {

        foreach (var ally in heroManager.GetAllyList(myHero.IsDarkSide)) {
            ApplyBuff<OffenseUp>(ally, StatusType.OffenseUp);
        }

        foreach (var enemy in heroManager.GetRivalList(myHero.IsDarkSide)) {

            if (!TryGiveDamageToTarget(enemy, myHero.damage))
                continue;

            int stunChance;
            if (TryGetStatus(enemy, StatusType.Shock, out BaseStatus shockStatus)) {
                Destroy(shockStatus);
                stunChance = 100;
            }
            else stunChance = 70;

            TryApplyDebuff<Stun>(enemy, StatusType.Stun, stunChance);
        }

    }

    private void SpecialAction2() {

    }
}