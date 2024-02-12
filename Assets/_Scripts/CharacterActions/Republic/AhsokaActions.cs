using System.Collections.Generic;

public class AhsokaActions : HeroActions {

    protected override void CreateHeroActions() {

        ActionVisualData basicActionVD = new(actionSprites[0],
            ActionPosition.TargetHero, 1.5f, 1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.TargetHero, 2.433f, 1.3f, true);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartAction, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartAction, CompleteAction, special1ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1 };
    }

    protected override void BasicAction() {

        if (!TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage, out bool isCritic))
            return;

        DispelAllStatuses(true, new List<Hero> { heroManager.TargetHero });

        if (isCritic) {
            DispelAllStatuses(false, new List<Hero>() { ChooseRandomHero(true), myHero });
        }
    }

    protected override void SpecialAction1() {

        TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage);

        foreach (Hero hero in heroManager.GetAllyList(myHero.IsDarkSide)) {
            hero.ChangeProtectionValue(.2f);
            float healAmount = hero.GetMyProperties().Health * .2f;
            hero.ChangeHealthValue(healAmount);
        }
    }
}
