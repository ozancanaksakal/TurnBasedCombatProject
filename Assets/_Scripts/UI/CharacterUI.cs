using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour {

    [SerializeField] private Image healhtBarImage;
    [SerializeField] private Image protectionBarImage;
    [SerializeField] private Image turnmeterBar;
    [SerializeField] private Transform barContainer;
    [SerializeField] private Transform statusContainer;
    [SerializeField] private GameObject statusImagePrefab;
    private Hero myHero;

    private void Awake() {
        myHero = transform.parent.GetComponent<Hero>();

        LookAtCamera();

        foreach (Transform transform in statusContainer) {
            Destroy(transform.gameObject);
        }
    }

    private void Start() {
        myHero.OnDamaged += UpdateVitalBars;
        myHero.OnStatusAdded += MyHero_OnStatusAdded;
        myHero.HeroActions.OnActionStarted += HideBars;
        myHero.HeroActions.OnActionCompleted += () => StartCoroutine(OnActionCompleted());
        //HeroActions.OnAnyActionCompleted += HeroActions_OnAnyActionCompleted;
        TurnSystem.Instance.OnInTurnHeroChanged += UpdateTurnmeterBar;
        UpdateVitalBars();
        UpdateTurnmeterBar();
        UpdateStatusBarOnStart();
    }

    private IEnumerator OnActionCompleted() {
        float waitTime = .2f;
        yield return new WaitForSeconds(waitTime);
        barContainer.gameObject.SetActive(true);
        LookAtCamera();
    }

    private void MyHero_OnStatusAdded(BaseStatus baseStatus) {
        Instantiate(statusImagePrefab, statusContainer)
            .GetComponent<StatusImageUI>().Setup(baseStatus);
    }
    private void UpdateVitalBars() {
        healhtBarImage.fillAmount = myHero.GetNormalizedHealth();
        protectionBarImage.fillAmount = myHero.GetNormalizedProtection();
    }

    private void HideBars() {
        barContainer.gameObject.SetActive(false);
    }

    private void UpdateTurnmeterBar() { turnmeterBar.fillAmount = myHero.GetNormalizedTurnmeter(); }

    private void LookAtCamera() {
        Vector3 dirToCamera = (Camera.main.transform.position - transform.position).normalized;
        transform.LookAt(transform.position + dirToCamera * -1);
    }
    private void UpdateStatusBarOnStart() {
        foreach (var status in myHero.ActiveStatusList) {
            MyHero_OnStatusAdded(status);
        }
    }
}