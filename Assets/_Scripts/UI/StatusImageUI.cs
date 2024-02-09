using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusImageUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private Image statusImage;
    [SerializeField] private StatusDatasSO statusDatasSO;

    private static readonly Color buffcolor = new Color(49f / 255f,
        229f / 255f, 82f / 255f);
    private static readonly Color debuffcolor = new Color(210f / 255f,
        47f / 255f, 50f / 255f);

    private BaseStatus status;

    public void Setup(BaseStatus baseStatus) {
        status = baseStatus;
    }

    private void Start() {
        status.OnDurationUpdated += Status_OnDurationUpdated;

        SetBackgroundColor();

        statusImage.sprite = statusDatasSO.StatusSprites[status.StatusType];
        durationText.text = status.DefaultDuration.ToString();
    }

    private void SetBackgroundColor() {
        if (status.IsBuff) {
            GetComponent<Image>().color = buffcolor;
        }
        else {
            GetComponent<Image>().color = debuffcolor;
        }
    }

    private void Status_OnDurationUpdated(int duration) {
        durationText.text = duration.ToString();
        if (duration <= 0) {
            status.OnDurationUpdated -= Status_OnDurationUpdated;
            Destroy(gameObject);
        }
    }
}