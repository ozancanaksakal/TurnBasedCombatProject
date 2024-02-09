using UnityEngine;

public readonly struct ActionVisualData {

    public readonly Sprite actionSprite;
    public readonly ActionPosition performPosition;
    public readonly float animationTime;
    public readonly float closingDistance;
    public readonly bool turnToTargetHero;
    public Vector3 TargetPosition {
        get {
            if (performPosition == ActionPosition.TargetHero)
                return HeroManager.Instance.TargetHero.transform.position;
            else if (performPosition == ActionPosition.Center)
                return Vector3.zero;
            
            return Vector3.zero;
        }
    }

    public ActionVisualData(Sprite actionSprite, ActionPosition performPosition,
        float animationTime, float closingDistance, bool turnToTargetHero) {
        this.actionSprite = actionSprite;
        this.performPosition = performPosition;
        this.animationTime = animationTime;
        this.closingDistance = closingDistance;
        this.turnToTargetHero = turnToTargetHero;
    }
}
