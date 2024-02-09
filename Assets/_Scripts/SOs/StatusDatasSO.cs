using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StatusData {
    [SerializeField] private StatusType statusType;
    [SerializeField] private Sprite sprite;

    public StatusType StatusType => statusType;
    public Sprite Sprite => sprite;
}


[CreateAssetMenu(fileName = "StatusData", menuName = "ScriptableObjects/StatusDatas", order = 3)]
public class StatusDatasSO : ScriptableObject {
    [SerializeField] private List<StatusData> statusDatas;

    public Dictionary<StatusType, Sprite> StatusSprites;
    public Color buffColor;
    public Color debuffColor;

    public void ConstructDictionary() {
        StatusSprites = new Dictionary<StatusType, Sprite>();
        for (int i = 0; i < statusDatas.Count; i++) {
            StatusData statusData = statusDatas[i];
            StatusSprites.Add(statusData.StatusType, statusData.Sprite);
        }
        //statusDatas = null;

    }

    public Sprite GetStatusSprite(StatusType statusType) {
        int index = (int)statusType;
        return statusDatas[index].Sprite;
    }

    public Sprite GetStatusSprite2(StatusType statusType) {
        foreach (var statusData in statusDatas) {
            if (statusData.StatusType == statusType)
                return statusData.Sprite;
        }
        return null;
    }
}
    public enum StatusType {
        Stun,
        DamageOverTime,
        AbilityBlock,
        Shatterpoint,
        ResilientDefense,
        Taunt,
        OffenseUp,
        OffenseDown,
        SpeedUp,
        SpeedDown,
        PotencyUp,
        PotencyDown,
        TenacityUp,
        TenacityDown,
        Foresight,
        HealImmunity,
        BuffImmunity,
    Shock,
    Stagger,
    Stealth,
    Riposte
}