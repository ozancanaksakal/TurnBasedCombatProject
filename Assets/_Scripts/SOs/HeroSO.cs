using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/HeroSO", order = 1)]
public class HeroSO : ScriptableObject {
    [field: SerializeField] public CharID ID { get; private set; }
    [field: SerializeField] public string HeroName { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int Protection { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int CritChance { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public int HealthRegeneration { get; private set; }
    [field: SerializeField] public int DodgeChance { get; private set; }
    [field: SerializeField] public int Potency { get; private set; }
    [field: SerializeField] public int Accuracy { get; private set; }
    [field: SerializeField] public int Tenacity { get; private set; }
}

public enum CharID {
    AnakinSkywalker,
    AhsokaTano,
    MaceWindu,
    Yoda,
    ObiWanKenobi,
    ChancellorPalpatine,
    CountDooku,
    DarthMaul,
    JangoFett,
    DarthVader,
    GeneralGrievous
}

