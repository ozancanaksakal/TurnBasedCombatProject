using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject attackButtonUI;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image protectionBarImage;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private GameObject chooseAllyTextGameObject;

    private void Awake() {
        buttonContainer.gameObject.SetActive(false);
        TurnSystem.Instance.OnInTurnHeroChanged += TurnSystem_OnInTurnCharacterChanged;
        // TurnSystem'in once (execution order)  calistigi bilindigi icin event hata olusturmaz
    }

    private void Start() {
        HeroActions.OnAnyActionStarted += HideButtonContainer;
        HeroActions.OnAnyActionCompleted += () => UpdateTargetVitalsUI();
        HeroManager.Instance.OnTargetHeroSelected += () => UpdateTargetVitalsUI();
        HeroManager.Instance.OnWaitForChooseAlly += (actionType) => ShowChooseAllyText();
        //Invoke(nameof(UpdateTargetVitalsUI), .2f);
    }


    private void OnDestroy() {
        HeroActions.OnAnyActionStarted -= HideButtonContainer;
        HeroActions.OnAnyActionCompleted -= () => UpdateTargetVitalsUI();
    }

    private void HideButtonContainer() {
        buttonContainer.gameObject.SetActive(false);
        chooseAllyTextGameObject.SetActive(false);
    }
    private void ShowChooseAllyText() {
        chooseAllyTextGameObject.SetActive(true);
    }

    private void UpdateTargetVitalsUI() {
        Hero hero = HeroManager.Instance.TargetHero;

        float healthNormalized = hero.GetNormalizedHealth();
        healthBarImage.fillAmount = healthNormalized;
        protectionBarImage.fillAmount = hero.GetNormalizedProtection();
        heroNameText.text = hero.GetMyProperties().HeroName;
    }

    private void TurnSystem_OnInTurnCharacterChanged() {
        buttonContainer.gameObject.SetActive(true);

        foreach (Transform buttonTransform in buttonContainer) {
            Destroy(buttonTransform.gameObject);
        }

        var inTurnCharacter = TurnSystem.Instance.InTurnHero;

        foreach (BaseAction baseAction in inTurnCharacter.HeroActions.ActionList) {
            Instantiate(attackButtonUI, buttonContainer).
                GetComponent<ActionButtonUI>().SetBaseAction(baseAction);
        }
    }

}
