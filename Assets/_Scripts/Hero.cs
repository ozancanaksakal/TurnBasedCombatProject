using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    public event Action OnDamaged;

    public event Action OnMyTurn;
    public event Action<bool> OnIamTarget;

    public event Action<BaseStatus> OnStatusAdded;
    public event Action<BaseStatus> OnStatusRemoved;
    public event Action OnDead;
    //public event Action<BaseStatus, bool> OnActiveSatusListChanged;

    [SerializeField] private HeroSO myProps;
    [SerializeField] private bool isDarkSide;

    public bool IsDarkSide => isDarkSide;
    public HeroActions HeroActions { get; private set; }
    public HeroAnimator HeroAnimator { get; private set; }
    public HeroVisual HeroVisual { get; private set; }
    public int Turnmeter => turnmeter;
    public float Health => health;
    public List<BaseStatus> ActiveStatusList => activeStatusList;

    private List<BaseStatus> activeStatusList;

    [HideInInspector] public float damage;
    [HideInInspector] public int speed;
    [HideInInspector] public int potency;
    [HideInInspector] public int tenacity;
    [HideInInspector] public int accuracy, dodgeChance;
    [HideInInspector] public int critChance;

    [SerializeField] private int turnmeter;
    private float health, protection;
    private int defense;
    //private int healthRegen;

    protected virtual void Awake() {
        InitStats();
        //turnmeter = 0;
        activeStatusList = new List<BaseStatus>();
        HeroActions = GetComponent<HeroActions>();
        HeroAnimator = GetComponent<HeroAnimator>();
        HeroVisual = GetComponent<HeroVisual>();
        //TurnSystem.Instance.AddToList(this);
    }

    private void OnDestroy() {
        TurnSystem.Instance.RemoveFromList(this);
    }

    private void Start() {
        HeroManager.Instance.OnTargetHeroSelected += CombatManager_OnTargetHeroSelected;
        TurnSystem.Instance.OnInTurnHeroChanged += TurnSystem_OnInTurnHeroChanged;
    }

    private void TurnSystem_OnInTurnHeroChanged() {
        if (this == TurnSystem.Instance.InTurnHero) {
            OnMyTurn?.Invoke();
            //Debug.Log($"{myProps.HeroName} fired event OnMyTurn");
        }
    }

    private void CombatManager_OnTargetHeroSelected() {

        bool isTarget = this == HeroManager.Instance.TargetHero;
        OnIamTarget?.Invoke(isTarget);
        //if (isTarget ) {
        //}
        //else if( !(this == TurnSystem.Instance.InTurnHero)) {
        //  return;
        //}//
    }


    public bool IsCritic() => IsActionSucceded(critChance);

    public static bool IsActionSucceded(int percentage) {
        int randomValue = UnityEngine.Random.Range(0, 101);
        return randomValue < percentage;
    }

    public void TakeDamage(float damageAmount) {
        //if (isHit) {
        //    // dodge event fired. dodge animation
        //    return;
        //}
        // Ekranda dodge yazýlabilir.
        float remainDamageAmount = damageAmount - defense;

        if (protection > remainDamageAmount) {
            protection -= remainDamageAmount;
            Debug.Log($"{gameObject.name} get damaged. Amount{remainDamageAmount}. Remain protection: {protection}");
        }
        else {
            health -= remainDamageAmount - protection;
            protection = 0;
            Debug.Log($"{gameObject.name} get damaged. Amount{remainDamageAmount}. Remain health: {health}");
        }

        OnDamaged?.Invoke();
        if (health <= 0) {

            if (HeroActions.canRevive)
                health = myProps.Health;
            else
                Die();
        }
    }

    public void ChangeHealthValue(float amount) {

        if (HeroActions.HasStatus(this, StatusType.HealImmunity) ||
            HeroActions.HasStatus(this, StatusType.Shock))
            return;
        //alternatif hasHealImmunity boolean

        health = Mathf.Clamp(health + amount, 0, myProps.Health);
    }

    private void Die() {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
        OnDead?.Invoke();
    }

    public float GetNormalizedHealth() {
        return health / myProps.Health;
    }

    public float GetNormalizedProtection() {
        return protection / myProps.Protection;
    }

    public void ChangeProtectionValue(float percentage) {
        float changeAmount = protection * percentage;
        protection = Mathf.Clamp(protection + changeAmount, 0, myProps.Protection);
    }


    public float GetNormalizedTurnmeter() { return (float)turnmeter / 100; }
    public void ChangeTurnmeter(int percentage) {
        if (percentage > 0 && HeroActions.HasStatus(this, StatusType.Shock))
            return;

        SetTurnmeter(turnmeter + percentage);
    }

    public void SetTurnmeter(int value) { turnmeter = Mathf.Clamp(value, 0, 100); }

    public void AddStatusToList(BaseStatus baseStatus) {
        activeStatusList.Add(baseStatus);
        OnStatusAdded?.Invoke(baseStatus);
        //Debug.Log($"onstatusadded fired {baseStatus.StatusType}");
        //OnActiveSatusListChanged?.Invoke(baseStatus, true);
    }

    public void RemoveFromStatusList(BaseStatus baseStatus) {
        activeStatusList.Remove(baseStatus);
        OnStatusRemoved?.Invoke(baseStatus);
        //OnActiveSatusListChanged?.Invoke(baseStatus, false);
    }

    public HeroSO GetMyProperties() { return myProps; }

    public void OnCallToAssist() => HeroActions.GetAction(ActionType.Basic).StartAction();

    private void InitStats() {
        health = myProps.Health;
        protection = myProps.Protection;
        speed = myProps.Speed;
        damage = myProps.Damage;
        critChance = myProps.CritChance;
        defense = myProps.Defense;
        //healthRegen = myProps.HealthRegeneration;
        dodgeChance = myProps.DodgeChance;
        potency = myProps.Potency;
        accuracy = myProps.Accuracy;
    }

}