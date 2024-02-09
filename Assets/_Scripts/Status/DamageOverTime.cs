
public class DamageOverTime : BaseStatus
{

    public override StatusType StatusType => StatusType.DamageOverTime;

    public override int DefaultDuration => 3;

    public override bool IsBuff => false;

    private int damage = 20;


    protected  void Start() {
        hero.OnMyTurn += Hero_OnMyTurn;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        hero.OnMyTurn -= Hero_OnMyTurn;
    }

    private void Hero_OnMyTurn() {
        hero.TakeDamage(damage);
        TryDestroyStatus();
    }

    public override void UpdateDuration(int value=3) {
        duration += value;
    }

}
