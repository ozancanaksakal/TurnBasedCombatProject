using System.Collections.Generic;
using UnityEngine;

public class WinduActions : HeroActions, IAssistable {
    public Hero AsisstantHero { get; set; }
    private bool HasResilientDefense = false;

    protected override void CreateHeroActions() {

        ActionVisualData basicActionVD = new(actionSprites[0],
        ActionPosition.TargetHero, 1.5f, 1f, true);
        ActionVisualData special1ActionVD = new(actionSprites[1],
            ActionPosition.TargetHero, 1.5f, 1f, true);
        ActionVisualData special2ActionVD = new(actionSprites[2],
            ActionPosition.TargetHero, 1.5f, 1f, true);

        BaseAction basicAction = new(ActionType.Basic, 0, BasicAction,
            StartAction, CompleteAction, basicActionVD);
        BaseAction special1 = new(ActionType.Special1, 3, SpecialAction1,
            StartAction, CompleteAction, special1ActionVD);
        BaseAction special2 = new(ActionType.Special1, 3, SpecialAction2,
            AssistableActionStart, CompleteAction, special2ActionVD);

        actionList = new List<BaseAction>() { basicAction, special1, special2 };

    }

    private void Start() {
        areaDamageConstant = .7f;
        myHero.OnMyTurn += MyHero_OnMyTurn;

        foreach (Hero hero in heroManager.GetAllyList(myHero.IsDarkSide)) {
            hero.OnDamaged += Heroes_OnDamaged;
        }

        myHero.OnDamaged += MyHero_OnDamaged;
    }

    private void MyHero_OnDamaged() {

        if (HasStatus(myHero, StatusType.ResilientDefense) ) {
            PerformCounterAttack();
        }
    }

    private void OnDestroy() {
        foreach (Hero hero in heroManager.GetAllyList(myHero.IsDarkSide)) {
            hero.OnDamaged -= Heroes_OnDamaged;
        }
    }

    private void MyHero_OnMyTurn() {
        HasResilientDefense = false;
        SetShatterpointHero();
    }

    private void Heroes_OnDamaged() {
        if (HasResilientDefense)
            return;

        ApplyBuff<ResilientDefense>(myHero, StatusType.ResilientDefense);
        HasResilientDefense = true;
    }

    protected override void BasicAction() {
        Hero target = heroManager.TargetHero;

        if (TryGetStatus(target, StatusType.Foresight, out BaseStatus status)) {
            Destroy(status);
            return;
        }

        if (!IsHit(target)) return;

        bool isCritic = myHero.IsCritic();
        float actualDamage = isCritic ? 1.5f * myHero.damage : myHero.damage;
        heroManager.TargetHero.TakeDamage(actualDamage);

        float healthNormalized = myHero.GetNormalizedHealth();
        if (healthNormalized > .5f)
            myHero.ChangeTurnmeter(15);
        else
            myHero.ChangeHealthValue(actualDamage);

        int applyChance = 80;
        TryApplyDebuff<AbilityBlock>(target, StatusType.AbilityBlock, applyChance);
    }

    protected override void SpecialAction1() {
        if (!TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage)) { return; }

        Hero target = heroManager.TargetHero;

        DispelAllStatuses(true, new List<Hero> { target });

        if (HasStatus(target, StatusType.Shatterpoint)) {
            target.ChangeTurnmeter(-50);
            myHero.ChangeTurnmeter(50);
            int applyChance = 80;
            TryApplyDebuff<Stun>(target, StatusType.Stun, applyChance);
        }
    }

    private void SpecialAction2() {
        if (!TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage)) { return; }

        int myTurnmeter = myHero.Turnmeter;
        myHero.SetTurnmeter(AsisstantHero.Turnmeter);
        AsisstantHero.SetTurnmeter(myTurnmeter);
        if (TryGetStatus(myHero, StatusType.Shatterpoint, out BaseStatus shatterpointStatus))
            shatterpointStatus.UpdateDuration(2);

        myHero.ChangeProtectionValue(.30f);
        AsisstantHero.ChangeProtectionValue(.30f);
    }

    private void SetShatterpointHero() {
        Hero target = ChooseRandomHero(myHero.IsDarkSide);
        ApplyStatus<Shatterpoint>(target);
    }

    public void AssistableActionStart(BaseAction baseAction) {
        Debug.Log("assistable action started");
        currentlyPerformingAction = baseAction;
        heroManager.SetWaitForChooseAlly(baseAction.actionType);
    }

    public void PerformAssistableAction() {
        StartAction(currentlyPerformingAction);
        heroManager.waitForChooseAlly = false;
    }
}
