using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnSystem : MonoBehaviour {
    public static TurnSystem Instance { get; private set; }

    public event Action OnInTurnHeroChanged;

    private List<Hero> allHeroes;

    private Hero inTurnHero;
    public Hero InTurnHero => inTurnHero;

    private int maxSpeed = 20;

    private Hero bonusTurnHero;

    private void Awake() {
        Instance = this;
        allHeroes = new List<Hero>();
        allHeroes.AddRange(FindObjectsOfType<Hero>());
    }

    private void Start() {
        //inTurnHero = allHeroes[0];
        FindInTurnHero();
        OnInTurnHeroChanged?.Invoke();
    }

    public void GoToNextTurn() {
        inTurnHero.SetTurnmeter(0);
        FindInTurnHero();
        //inTurnHero = FindInTurnHero();
        //OnInTurnHeroChanged?.Invoke();
    }

    private void FindInTurnHero() {
        
        if ( bonusTurnHero !=null) {
            inTurnHero = bonusTurnHero;
            OnInTurnHeroChanged?.Invoke();
            bonusTurnHero = null;
            return;
        }

        allHeroes.Sort((Hero a, Hero b) => b.Turnmeter - a.Turnmeter);

        Hero maxTurnmeterHero = allHeroes[0];

        if (maxTurnmeterHero.Turnmeter >= 100) {
            inTurnHero = maxTurnmeterHero;
            OnInTurnHeroChanged?.Invoke();
            return;
        }
        float turnmeterDifference = 100f - maxTurnmeterHero.Turnmeter;

        float ratio = turnmeterDifference / maxSpeed;

        int updateTimes = Mathf.CeilToInt(ratio);
        //Debug.Log($"Maxturnmeter {maxTurnmeterHero.Turnmeter} & "+updateTimes);
        UpdateTurnMeters(updateTimes);
        FindInTurnHero();
    }

    public void GiveBonusTurn(Hero hero) {
        bonusTurnHero = hero;
        hero.SetTurnmeter(100);
    }

    private void UpdateTurnMeters(int times) {
        foreach (Hero hero in allHeroes)
            hero.SetTurnmeter(hero.Turnmeter + times * hero.speed);
    }

    public void AddToList(Hero hero) {
        allHeroes.Add(hero);
    }

    public void RemoveFromList(Hero hero) {
        allHeroes.Remove(hero);
        HeroManager.Instance.RemoveFromTeamList(hero);
    }

    public List<Hero> GetAllHeroes() { return allHeroes; }
}