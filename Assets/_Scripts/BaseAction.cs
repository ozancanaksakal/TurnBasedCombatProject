using System;

public class BaseAction {

    public readonly ActionType actionType;
    public readonly int cooldown;
    public readonly ActionVisualData visualData;

    public bool isBlocked;
    private int currentCooldown;

    private readonly Action<BaseAction> onActionStarted;
    private readonly Action onActionCompleted;
    public readonly Action Perform;

    public BaseAction(ActionType actionType, int cooldown, Action performAction,
        Action<BaseAction> onActionStarted, Action completeAction,ActionVisualData visualData) {
        this.actionType = actionType;
        this.cooldown = cooldown;
        Perform = performAction;
        this.onActionStarted = onActionStarted;
        this.onActionCompleted = completeAction;
        this.visualData = visualData;
    }

    public bool IsActive() { return currentCooldown == 0; }

    public void ReduceCooldown() {
        if (currentCooldown > 0)
            currentCooldown--;
    }

    public int GetCurrentCooldown() { return currentCooldown; }

    private void SetCooldown() { currentCooldown = cooldown; }

    public void StartAction() {
        SetCooldown();
        onActionStarted(this);
    }

    public void CompleteAction() => onActionCompleted();
}
