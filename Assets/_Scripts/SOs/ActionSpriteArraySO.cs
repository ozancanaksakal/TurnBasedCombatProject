using UnityEngine;

[CreateAssetMenu(fileName = "ActionSprites", menuName = "ScriptableObjects/ActionSprites", order = 2)]
public class ActionSpriteArraySO : ScriptableObject {
    [field: SerializeField] public Sprite[] actionSpriteArray { get; private set; }

}


public enum ActionType {
    Basic,
    Special1,
    Special2,
    Special3,
}

public enum ActionPosition {
    InPlace,
    Center,
    TargetHero,
}
