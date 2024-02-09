using System.Collections.Generic;
using UnityEngine;

public class YodaActions : HeroActions {

    protected override void CreateHeroActions() {

        ActionVisualData basicActionVD = new(actionSprites[0],
            ActionPosition.TargetHero, 1.5f, 1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.Center, 1.5f, 0.2f, true);
        ActionVisualData special2ActionVD = new(actionSprites[2],
            ActionPosition.TargetHero, 1.5f, 1f, true);
        ActionVisualData special3ActionVD = new(actionSprites[3],
            ActionPosition.InPlace, 3.267f, 0.05f, false);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartAction, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartAction, CompleteAction, special1ActionVD);
        BaseAction special2 = new(ActionType.Special2, 3, SpecialAction2,
            StartAction, CompleteAction, special2ActionVD);
        BaseAction special3 = new(ActionType.Special3, 3, SpecialAction3,
            StartAction, CompleteAction, special3ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1, special2, special3 };
    }

    private void Start() {
        areaDamageConstant = .7f;
    }

    protected override void BasicAction() {
        if (!TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage)) { return; }

        int applyChance = 80;
        TryApplyDebuff<PotencyDown>(heroManager.TargetHero, StatusType.PotencyDown, applyChance);

        float enemyHealthNormalized = heroManager.TargetHero.GetNormalizedHealth();
        if (enemyHealthNormalized >= 0.5f) {
            myHero.ChangeTurnmeter(40);
            ApplyBuff<Foresight>(myHero, StatusType.Foresight);
        }
        else {
            ApplyBuff<OffenseUp>(myHero, StatusType.OffenseUp);

        }
    }

    protected override void SpecialAction1() {
        int newDamage = Mathf.RoundToInt(myHero.damage * areaDamageConstant);
        GiveAreaDamage(newDamage);

        var targets = heroManager.GetRivalList(myHero.IsDarkSide);
        foreach (Hero target in targets) {
            var buffList = target.ActiveStatusList
                .FindAll(status => status.IsBuff == true);

            TryAddBuffsToMe(buffList);
        }
        TurnSystem.Instance.GiveBonusTurn(myHero);
        // turn sisteme kendini haber ver tur seçildiðinde bana gelsin
    }

    private void SpecialAction2() {
        Hero target = heroManager.TargetHero;
        TryGiveDamageToTarget(target, myHero.damage);

        int percentage = -70;
        target.ChangeTurnmeter(percentage);

        float healthNormalized = target.GetNormalizedHealth();
        int applyChance = 80;
        if (healthNormalized < 1)
            TryApplyDebuff<Stun>(target, StatusType.Stun, applyChance);
    }

    private void SpecialAction3() {

        foreach (Hero ally in heroManager.GetAllyList(myHero.IsDarkSide)) {
            ally.ChangeProtectionValue(.3f);
            ApplyBuff<TenacityUp>(ally, StatusType.TenacityUp);
            ApplyBuff<Foresight>(ally, StatusType.Foresight);
        }
    }

    private void TryAddBuffsToMe(List<BaseStatus> buffList) {

        foreach (BaseStatus buff in buffList) {
            if (buff.StatusType == StatusType.ResilientDefense ||
                buff.StatusType == StatusType.Taunt)
                continue;

            if (HasStatus(myHero, buff.StatusType))
                continue;

            buff.CreateInstanceOnHero(myHero);
        }
    }

    //private void AddStatus(StatusType statusType) {
    //    switch (statusType) {
    //        //case StatusType.OffenseUp:
    //        //    if (!HasStatus(myHero, statusType))
    //        //        ApplyStatus<OffenseUp>(myHero, statusSprites.offenseUp);
    //        //    break;
    //        //case StatusType.SpeedUp:
    //        //    break;
    //        case StatusType.PotencyUp:
    //            if (!HasStatus(myHero, statusType))
    //                //ApplyStatus<PotencyUp>(myHero, statusSprites.potency);
    //            break;
    //    }
    //}
}
