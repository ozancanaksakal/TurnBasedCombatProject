using UnityEngine;

public class Stun : BaseStatus {
    public override StatusType StatusType => StatusType.Stun;
    public override int DefaultDuration => 1;
    
    public override bool IsBuff => false;

    protected  void Start() {
        hero.HeroVisual.ShowStunVisual();
        hero.OnMyTurn += Hero_OnMyTurn;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.HeroVisual.HideStunVisual();
        hero.OnMyTurn -= Hero_OnMyTurn;
    }

    private void Hero_OnMyTurn() {
        TurnSystem.Instance.GoToNextTurn();
        TryDestroyStatus();
        Debug.Log($"{hero.name} is stunned. Turn changed!");
    }

}
