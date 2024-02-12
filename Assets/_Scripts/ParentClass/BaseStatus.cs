using System;
using UnityEngine;

public abstract class BaseStatus : MonoBehaviour {

    public event Action<int> OnDurationUpdated;

    public abstract StatusType StatusType { get; }
    public abstract int DefaultDuration { get; }
    public abstract bool IsBuff { get; }

    protected Hero hero;
    protected int duration;

    protected virtual void Awake() {
        hero = GetComponent<Hero>();
        hero.AddStatusToList(this);
        duration = DefaultDuration;
        Debug.Log($"{StatusType} is added to {hero.name}");
    }

    public virtual void CreateInstanceOnHero(Hero hero) {
        Debug.Log("Method Not implemented");
    }

    protected virtual void OnDestroy() {
        Debug.Log($"Status {StatusType} is removed from {hero.name}");
        hero.RemoveFromStatusList(this);
    }

    protected void TryDestroyStatus() {
        duration--;
        OnDurationUpdated?.Invoke(duration);

        if (duration <= 0) Destroy(this);
    }

    public virtual void UpdateDuration(int value) {
        duration = Mathf.Max(duration, value);
    }
}
