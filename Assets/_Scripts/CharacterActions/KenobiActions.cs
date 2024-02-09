
public class KenobiActions : HeroActions
{
    protected override void Awake() {
        base.Awake();
        areaDamageConstant = .7f;
    }

    protected override void BasicAction() {

        TryGiveDamageToTarget(heroManager.TargetHero, myHero.damage);
    }

    protected override void CreateHeroActions() {
        throw new System.NotImplementedException();
    }

    protected override void SpecialAction1() {
        float newDamage = myHero.damage * areaDamageConstant;
        GiveAreaDamage(newDamage);
    }
}
