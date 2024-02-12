using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroActions : MonoBehaviour {

    public event Action OnActionStarted;
    public event Action OnActionCompleted;
    public static event Action OnAnyActionStarted;
    public static event Action OnAnyActionCompleted;

    //[SerializeField] private ActionSpriteArraySO actionSpriteArray;
    [SerializeField] protected Sprite[] actionSprites;

    public List<BaseAction> ActionList => actionList;
    public BaseAction CurrentlyPerformingAction => currentlyPerformingAction;
    public BaseAction CounterAction => counterAction;
    protected HeroManager heroManager => HeroManager.Instance;
    
    protected List<BaseAction> actionList;
    protected Hero myHero;

    protected BaseAction currentlyPerformingAction;
    protected BaseAction counterAction;
    protected float areaDamageConstant;

    [HideInInspector] public bool canRevive = false;

    protected virtual void Awake() {
        myHero = GetComponent<Hero>();
        CreateHeroActions();
    }

    protected abstract void CreateHeroActions();

    protected void StartAction(BaseAction performerAction) {
        currentlyPerformingAction = performerAction;
        OnActionStarted?.Invoke();
        OnAnyActionStarted?.Invoke();
    }

    public void CompleteAction() {
        ReduceCooldowns(currentlyPerformingAction);
        OnActionCompleted?.Invoke();
        OnAnyActionCompleted?.Invoke();
        currentlyPerformingAction = null;
    }

    public BaseAction GetAction(ActionType action) {
        int index = (int)action;
        return actionList[index];
    }

    protected void ReduceCooldowns(BaseAction senderAction) {
        //foreach (BaseAction baseAction in actionList) {
        //    if (baseAction == senderAction)
        //        continue;
        //    baseAction.ReduceCooldown();
        //}
        actionList.ForEach((action) => {
            if (action != senderAction)
                action.ReduceCooldown();
        });
    }

    protected T ApplyStatus<T>(Hero target) where T : BaseStatus {
        T newStatus = target.gameObject.AddComponent<T>();
        return newStatus;
    }

    protected void ApplyBuff<T>(Hero hero, StatusType statusType) where T : BaseStatus {

        //if (TryGetStatus(hero, StatusType.BuffImmunity, out BaseStatus buffImmunity)) {
        //    Destroy(buffImmunity);
        //    return;
        //}
        if (HasStatus(hero, StatusType.BuffImmunity) ||
            HasStatus(hero, StatusType.Shock))
            return;

        if (TryGetStatus(hero, statusType, out BaseStatus status)) {
            status.UpdateDuration(status.DefaultDuration);
        }
        else { ApplyStatus<T>(hero); }
    }

    protected void TryApplyDebuff<T>(Hero target, StatusType statusType, int applyChance) where T : BaseStatus {

        if (!CanApplyStatus(target, applyChance)) { return; }

        if (TryGetStatus(target, statusType, out BaseStatus status)) {
            status.UpdateDuration(status.DefaultDuration);
        }
        else { ApplyStatus<T>(target); }

        //return true;
    }

    //protected bool TryApplyStatus<T>(Hero target, StatusType statusType)
    //    where T : BaseStatus {
    //    //if (!CanApplyStatus(target)) { return false; }
    //    if (TryGetStatus(target, statusType, out BaseStatus status)) {
    //        status.UpdateDuration(status.DefaultDuration);
    //    }
    //    else {
    //        ApplyStatus<T>(target);
    //    }
    //    return true;
    //}

    public static bool HasStatus(Hero hero, StatusType statusType) {
        return hero.ActiveStatusList
            .Exists(status => status.StatusType == statusType);
    }

    public bool TryGetStatus(Hero hero, StatusType statusType, out BaseStatus status) {

        status = hero.ActiveStatusList
            .Find(status => status.StatusType == statusType);
        return status != null;
        //alternatif dictionary oluþturmak
    }

    protected abstract void BasicAction();
    protected abstract void SpecialAction1();

    // Burdan itibaren farklý herolarda kullanýlabilecek aksiyonlar yazýlýyor

    protected bool TryGiveDamageToTarget(Hero target, float damageAmount) {

        if (TryGetStatus(target, StatusType.Foresight, out BaseStatus status)) {
            Destroy(status);
            return false;
        }

        bool isHit = IsHit(target);

        if (isHit) {
            bool isCritic = myHero.IsCritic();
            float actualDamage = isCritic ? 1.5f * damageAmount : damageAmount;
            target.TakeDamage(actualDamage);
        }
        return isHit;
    }

    protected bool TryGiveDamageToTarget(Hero target, float damageAmount, out bool ifHitisCritic) {
        ifHitisCritic = false;

        if (TryGetStatus(target, StatusType.Foresight, out BaseStatus status)) {
            Destroy(status);
            return false;
        }

        bool isHit = IsHit(target);

        if (isHit) {
            ifHitisCritic = myHero.IsCritic();
            float actualDamage = ifHitisCritic ? 1.5f * damageAmount : damageAmount;
            target.TakeDamage(actualDamage);
        }

        return isHit;
    }

    protected bool IsHit(Hero target) {
        int hitChance = UnityEngine.Random.Range(0, 101) + myHero.accuracy;
        int dodgeChance = UnityEngine.Random.Range(0, 101) + target.dodgeChance;
        return hitChance > dodgeChance;
    }

    protected bool CanApplyStatus(Hero target, int defaultApplyChance) {
        int applyChance = UnityEngine.Random.Range(0, 101) + myHero.potency;
        int resistChance = UnityEngine.Random.Range(0, 101) + target.tenacity;
        return (applyChance + defaultApplyChance) > resistChance;
    }

    protected void GiveAreaDamage(float damageAmount) {
        List<Hero> targetList = heroManager.GetRivalList(myHero.IsDarkSide);
        float newDamage = damageAmount * areaDamageConstant;
        foreach (Hero hero in targetList) {
            TryGiveDamageToTarget(hero, newDamage);
        }
    }

    protected void DispelAllStatuses(bool isStatusesBuff, List<Hero> targets) {
        foreach (Hero hero in targets) {
            foreach (BaseStatus status in hero.ActiveStatusList) {
                if (status.IsBuff == isStatusesBuff) {
                    Destroy(status);
                }
            }
        }
    }

    protected Hero ChooseRandomHero(bool isAlly) {
        bool mySide = myHero.IsDarkSide;
        List<Hero> heroList = isAlly ? heroManager.GetAllyList(mySide)
            : heroManager.GetRivalList(mySide);

        int randomIndex = UnityEngine.Random.Range(0, heroList.Count);
        return heroList[randomIndex];
    }

    protected void PerformCounterAttack() {
        heroManager.InQueueCounterList.Add(myHero);
        Debug.Log(myHero.name + " will counter");
    }
}