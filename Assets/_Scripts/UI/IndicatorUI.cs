using UnityEngine;

public class IndicatorUI : MonoBehaviour {
    [SerializeField] Hero myHero;
    [SerializeField] private Material[] indicatorMaterialList;

    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        Hide();
    }

    private void Start() {
        myHero.OnMyTurn += MyHero_OnMyTurn;
        myHero.OnIamTarget += MyHero_OnIamTarget;
        HeroActions.OnAnyActionStarted += () => Hide();
    }

    private void OnDestroy() {
        myHero.OnMyTurn -= MyHero_OnMyTurn;
        myHero.OnIamTarget -= MyHero_OnIamTarget;
    }

    private void Hide() => meshRenderer.enabled = false;

    private void MyHero_OnIamTarget(bool amITarget) {
        if (amITarget) {
            meshRenderer.enabled = true;
            meshRenderer.material = indicatorMaterialList[1];
        }
        else {
            bool isHeroTurn = myHero == TurnSystem.Instance.InTurnHero;
            meshRenderer.enabled = isHeroTurn;
        }
    }

    private void MyHero_OnMyTurn() {
        meshRenderer.enabled = true;
        meshRenderer.material = indicatorMaterialList[0];
    }
}
