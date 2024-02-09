using System.Collections.Generic;

public class AnakinActions : HeroActions {

    protected override void CreateHeroActions() {
        ActionVisualData basicActionVD = new(actionSprites[0],
            ActionPosition.TargetHero, 1.5f, 1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.Center, 2.967f, 0.2f, true);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartAction, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartAction, CompleteAction, special1ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1 };
    }

    private void Start() {
        areaDamageConstant = .7f;
    }

    protected override void BasicAction() {

        if (!TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage))
            return;

        int applyChance = 80;

        TryApplyDebuff<HealImmunity>(heroManager.TargetHero, StatusType.HealImmunity, applyChance);
        TryApplyDebuff<BuffImmunity>(heroManager.TargetHero, StatusType.BuffImmunity, applyChance);
    }

    protected override void SpecialAction1() {
        float newDamage = myHero.damage * areaDamageConstant;

        var targets = heroManager.GetRivalList(myHero.IsDarkSide);
        int criticCounter = 0;
        foreach (Hero target in targets) {
            TryGiveDamageToTarget(target, newDamage, out bool isCritic);
            if (isCritic) criticCounter++;
        }

        float protectionUpPercentage = criticCounter * .1f;
        var allies = heroManager.GetAllyList(myHero.IsDarkSide);
        foreach (Hero ally in allies) {
            ApplyBuff<OffenseUp>(ally, StatusType.OffenseUp);
            ally.ChangeProtectionValue(protectionUpPercentage);
        }
    }

}