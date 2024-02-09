using UnityEngine;

public class HeroVisual : MonoBehaviour {
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject transparentBody;

    public void ShowStunVisual() { transparentBody.SetActive(true); }
    public void HideStunVisual() { transparentBody.SetActive(false); }

    public void ShowStealthVisual() {
        body.SetActive(false); ;
        transparentBody.SetActive(true);
    }
    public void HideStealthVisual() {
        body.SetActive(true);
        transparentBody.SetActive(false);
    }
}
