using System.Collections.Generic;

public class MaulActions : HeroActions {

    private int deadEnemyCounter;

    protected override void CreateHeroActions() {
        ActionVisualData basicActionVD = new(actionSprites[0],
            ActionPosition.TargetHero, 1.5f, 0.7f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.Center, 1.5f, 0.3f, true);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartAction, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartAction, CompleteAction, special1ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1 };
    }

    private void Start() {
        areaDamageConstant = 0.7f;
        var enemies = heroManager.GetRivalList(myHero.IsDarkSide);
        foreach (Hero enemy in enemies) {
            enemy.OnDead += Enemy_OnDead;
        }
    }

    private void Enemy_OnDead() {
        deadEnemyCounter++;
        switch (deadEnemyCounter) {
            case 1:
                myHero.critChance += 25;
                break;
            case 2: 
                myHero.dodgeChance += 25;
                break;
        }
    }

    protected override void BasicAction() {
        if (!TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage))
            return;

        if (heroManager.TargetHero.Health <= 0) {
            myHero.SetTurnmeter(100);
            ApplyBuff<OffenseUp>(myHero, StatusType.OffenseUp);
        }
    }

    protected override void SpecialAction1() {
        GiveAreaDamage(myHero.damage);
    }
}
