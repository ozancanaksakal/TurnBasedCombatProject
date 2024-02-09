public class Shatterpoint : BaseStatus {
    public override StatusType StatusType => StatusType.Shatterpoint;

    public override int DefaultDuration => 1;

    public override bool IsBuff => false;


    protected  void Start() {
        HeroActions.OnAnyActionCompleted += HeroActions_OnAnyActionCompleted;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        HeroActions.OnAnyActionCompleted -= HeroActions_OnAnyActionCompleted;
    }

    private void HeroActions_OnAnyActionCompleted() {
        Destroy(this);
    }

    public override void UpdateDuration(int value) {
        duration += value;
    }
}