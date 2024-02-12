using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI remainCooldownText;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private GameObject abilityBlockImagePrefab;

    private Button button;
    private BaseAction baseAction;

    private void Awake() {
        cooldownImage.gameObject.SetActive(false);
        button = GetComponent<Button>();
    }
    public void SetBaseAction(BaseAction baseAction) {
        this.baseAction = baseAction;
    }

    private void Start() {
        button.onClick.AddListener(() => baseAction.StartAction());
        HeroManager.Instance.OnWaitForChooseAlly += HeroManager_OnWaitForChooseAlly;
        SetButtonVisual();
    }
    private void OnDestroy() {
        HeroManager.Instance.OnWaitForChooseAlly -= HeroManager_OnWaitForChooseAlly;
    }

    private void HeroManager_OnWaitForChooseAlly(ActionType actionType) {
        if (baseAction.actionType == actionType) {
            Instantiate(abilityBlockImagePrefab, transform);
            Debug.Log("choose an ally");
        }
        else {
            cooldownImage.gameObject.SetActive(true);
            button.interactable = false;
        }
    }

    private void SetButtonVisual() {
        button.image.sprite = baseAction.visualData.actionSprite;

        if (baseAction.isBlocked) {
            button.interactable = false;
            Instantiate(abilityBlockImagePrefab, transform);
        }

        else if (baseAction.IsActive()) {
            remainCooldownText.gameObject.SetActive(false);
            cooldownImage.gameObject.SetActive(false);
        }

        else {
            // if action in cooldown
            button.interactable = false;
            remainCooldownText.text = baseAction.GetCurrentCooldown().ToString();
            cooldownImage.fillAmount = (float)baseAction.GetCurrentCooldown() / baseAction.cooldown;
        }
    }

}