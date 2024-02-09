

public class ResilientDefense : BaseStatus
{
    public override StatusType StatusType => StatusType.ResilientDefense;

    public override int DefaultDuration => 3;

    public override bool IsBuff => true;

    //private int resilientStack = 3;

    
    private void Start() {
        HeroManager.Instance.UpdatePriorityTargetList(hero, true);
        hero.OnDamaged += TryDestroyStatus;
    }



    protected override void OnDestroy() {
        base.OnDestroy();
        HeroManager.Instance.UpdatePriorityTargetList(hero, false);
        hero.OnDamaged -= TryDestroyStatus;
    }

    public override void UpdateDuration(int value) {
        duration += value;
    }

}
