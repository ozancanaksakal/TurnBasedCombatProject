using UnityEngine;

public class VaderActions : HeroActions {


    protected override void Awake() {
        base.Awake();
        areaDamageConstant = .7f;
    }

    protected override void BasicAction() {
        if (TryGetStatus(heroManager.TargetHero, StatusType.Foresight, out BaseStatus status)) {
            Destroy(status);
            return;
        }
        // target cannot dodge this attack
        bool isCritic = myHero.IsCritic();
        float actualDamage = isCritic ? 1.5f * myHero.damage: myHero.damage;
        heroManager.TargetHero.TakeDamage(actualDamage);

        int applyChance = 90; //katsayý
        TryApplyDebuff<AbilityBlock>(heroManager.TargetHero, StatusType.DamageOverTime, applyChance);
    }

    protected override void CreateHeroActions() {
        throw new System.NotImplementedException();
    }

    protected override void SpecialAction1() {
        float newDamage = myHero.damage * areaDamageConstant;
        GiveAreaDamage(newDamage);

        var targets = heroManager.GetRivalList(myHero.IsDarkSide);

        foreach (Hero target in targets) {
            int applyChance = 80;
            TryApplyDebuff<DamageOverTime>(target, StatusType.DamageOverTime, applyChance);
        }
    }
    
    //protected override void SpecialAction2() {
    //}

    //protected override void SpecialAction3() {
    //}
}