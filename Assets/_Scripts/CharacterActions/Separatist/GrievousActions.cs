using System.Collections.Generic;

public class GrievousActions : HeroActions {
    
    protected override void CreateHeroActions() {
        ActionVisualData basicActionVD = new(actionSprites[0],
       ActionPosition.TargetHero, 1.5f, 0.1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.Center, 1.5f, 0.1f, true);
        ActionVisualData special2ActionVD = new(actionSprites[2],
            ActionPosition.TargetHero, 1.5f, 0.1f, true);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartAction, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartAction, CompleteAction, special1ActionVD);
        BaseAction special2 = new(ActionType.Special1, 3, SpecialAction2,
            StartAction, CompleteAction, special2ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1, special2 };
    }

    protected override void BasicAction() {
        Hero target = heroManager.TargetHero;

        float damage = myHero.damage;
        if (target.ActiveStatusList.Exists(status => status.IsBuff == false))
            damage *= .3f;


        if (!TryGiveDamageToTarget(target, damage))
            return;

        TryApplyDebuff<HealImmunity>(target, StatusType.HealImmunity, 100);

    }

    protected override void SpecialAction1() {
        throw new System.NotImplementedException();
    }

    private void SpecialAction2() {
        
    }
}