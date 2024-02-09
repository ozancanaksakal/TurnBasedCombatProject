using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour {
    public static HeroManager Instance { get; private set; }

    public event Action OnTargetHeroSelected;
    public event Action<ActionType> OnWaitForChooseAlly;

    [SerializeField] private StatusDatasSO statusSprites;
    [SerializeField] private Transform teamA;
    [SerializeField] private Transform teamB;

    public List<Hero> InQueueAssistList => inQueueAssistList;
    public List<Hero> InQueueCounterList => inQueueCounterList;
    public Hero TargetHero => targetHero;
    private Hero InTurnHero => TurnSystem.Instance.InTurnHero;

    private Hero targetHero;
    private List<Hero> lightSideTeam;
    private List<Hero> darkSideTeam;

    private Dictionary<bool, Hero> lastAttackedHeroes;
    private Dictionary<bool, List<Hero>> priorityTargets;
    private List<Hero> nonSelectables;
    private List<Hero> inQueueAssistList;
    private List<Hero> inQueueCounterList;


    public bool waitForChooseAlly;

    private void Awake() {
        Instance = this;
        inQueueAssistList = new List<Hero>();
        inQueueCounterList = new List<Hero>();
        lastAttackedHeroes = new Dictionary<bool, Hero>();
        CreateTeams();
        SetHeroesLocations();
        priorityTargets = new Dictionary<bool, List<Hero>>() {
            {true, new List<Hero>() },
            {false, new List<Hero>()}
        };

        statusSprites.ConstructDictionary();
        nonSelectables = new List<Hero>();
    }

    private void Start() {
        InputManager.Instance.OnTouchToHero += InputManager_OnTouchToHero;
        TurnSystem.Instance.OnInTurnHeroChanged += () => SetTargetHero();
        HeroActions.OnAnyActionCompleted += HeroActions_OnAnyActionCompleted;
        SetDefaultTargetCharacters();
    }

    private void HeroActions_OnAnyActionCompleted() {
        lastAttackedHeroes[InTurnHero.IsDarkSide] = targetHero;
        if (inQueueAssistList.Count != 0) {
            
            inQueueAssistList[0].OnCallToAssist();
            inQueueAssistList.RemoveAt(0);
        }
        else if(inQueueCounterList.Count != 0 ){
            targetHero = InTurnHero;
            inQueueCounterList[0].OnCallToAssist();
            inQueueCounterList.RemoveAt(0);
        }
        else {
            TurnSystem.Instance.GoToNextTurn();
            //return
        }
    }

    private void InputManager_OnTouchToHero(Hero selectedHero) {
        // identify character
        if (!waitForChooseAlly) {
            List<Hero> selectableTargets = GetSelectableTargets();

            if (selectableTargets.Contains(selectedHero)) {
                targetHero = selectedHero;
                OnTargetHeroSelected?.Invoke();
            }
        }
        else {
            OnHeroChooseAssistant(selectedHero);
        }
    }

    private void OnHeroChooseAssistant(Hero assistant) {
        List<Hero> selectableTargets = GetAllyList(InTurnHero.IsDarkSide);

        if (selectableTargets.Contains(assistant) &&
            assistant != InTurnHero) {
            inQueueAssistList.Add(assistant);

            IAssistable assistable = InTurnHero.HeroActions as IAssistable;
            assistable.AsisstantHero = assistant;
            assistable.PerformAssistableAction();
        }
    }

    public void SetWaitForChooseAlly(ActionType actionType) {
        waitForChooseAlly = true;
        OnWaitForChooseAlly?.Invoke(actionType);
    }

    private List<Hero> GetSelectableTargets() {
        bool isDarkSide = InTurnHero.IsDarkSide;
        if (priorityTargets[isDarkSide].Count != 0)
            return priorityTargets[isDarkSide];

        if (nonSelectables.Count == 0) {
            return GetRivalList(isDarkSide);
        }

        List<Hero> selectables = new();
        selectables.AddRange(GetRivalList(InTurnHero.IsDarkSide));
        selectables.RemoveAll(selectable => nonSelectables.Contains(selectable));
        return selectables;
    }

    private void SetDefaultTargetCharacters() {
        bool isDarkside = InTurnHero.IsDarkSide;
        targetHero = GetRivalList(isDarkside)[0];
        OnTargetHeroSelected?.Invoke();
        lastAttackedHeroes.Add(isDarkside, targetHero);
        lastAttackedHeroes.Add(!isDarkside, GetAllyList(isDarkside)[0]);
    }

    private void SetTargetHero() {
        Hero lastAttackedHero = lastAttackedHeroes[InTurnHero.IsDarkSide];

        if (lastAttackedHero != null) { targetHero = lastAttackedHero; }
        else {
            var rivalList = GetRivalList(InTurnHero.IsDarkSide);
            int randomIndex = UnityEngine.Random.Range(0, rivalList.Count);
            targetHero = rivalList[randomIndex];
        }

        OnTargetHeroSelected?.Invoke();
        //Debug.Log("Target is " + targetHero.name);
    }

    public List<Hero> GetAllyList(bool iAmDarkSide) {
        return iAmDarkSide ? darkSideTeam : lightSideTeam;
    }

    public List<Hero> GetRivalList(bool iAmDarkSide) {
        return iAmDarkSide ? lightSideTeam : darkSideTeam;
    }

    private void CreateTeams() {
        lightSideTeam = new List<Hero>();
        darkSideTeam = new List<Hero>();
        foreach (Hero hero in TurnSystem.Instance.GetAllHeroes()) {
            if (hero.IsDarkSide)
                darkSideTeam.Add(hero);
            else lightSideTeam.Add(hero);
        }
    }

    private void SetHeroesLocations() {
        int positionIndex = 1;

        for (int i = 0; i < lightSideTeam.Count; i++) {
            Hero hero = lightSideTeam[i];
            if (hero.GetMyProperties().ID == CharID.Yoda)
                hero.transform.position = teamA.GetChild(0).position;
            else {
                lightSideTeam[i].transform.position = teamA.GetChild(positionIndex).position;
                Debug.Log(positionIndex);
                positionIndex++;
            }
        }
        positionIndex = 1;
        for (int i = 0; i < darkSideTeam.Count; i++) {
            Hero hero = darkSideTeam[i];
            if (hero.GetMyProperties().ID == CharID.ChancellorPalpatine)
                hero.transform.position = teamB.GetChild(0).position;
            else {
                darkSideTeam[i].transform.position = teamB.GetChild(positionIndex).position;
                positionIndex++;
            }
        }

        //foreach (Transform transform in teamA) {
        //    Hero hero = lightSideTeam[index];
        //    if (hero.GetMyProperties().ID == CharID.Yoda)
        //        hero.transform.position = teamA.GetChild(0).position;

        //    else
        //        lightSideTeam[index].transform.position = transform.position;

        //    index++;
        //    if (index == lightSideTeam.Count)
        //        break;
        //}
        //index = 0;
        //foreach (Transform transform in teamB) {
        //    darkSideTeam[index].transform.position = transform.position;
        //    index++;
        //    if (index == darkSideTeam.Count)
        //        break;
        //}
        //Destroy(teamA.gameObject);
        //Destroy(teamB.gameObject);
    }

    public void RemoveFromTeamList(Hero hero) {
        if (hero.IsDarkSide)
            darkSideTeam.Remove(hero);
        else lightSideTeam.Remove(hero);
    }

    public void UpdatePriorityTargetList(Hero hero, bool add) {
        if (add)
            priorityTargets[!hero.IsDarkSide].Add(hero);
        else
            priorityTargets[!hero.IsDarkSide].Remove(hero);
    }

    public void UpdateNonSelectableList(Hero hero, bool add) {
        if (add)
            nonSelectables.Add(hero);
        else
            nonSelectables.Remove(hero);
    }

    public void AddToQueue(Hero hero) { inQueueAssistList.Add(hero); }
}