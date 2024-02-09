using System.Collections;
using UnityEngine;

public class LightningVisual : MonoBehaviour {
    //[SerializeField] private GameObject particleEffectGameObject;

    [SerializeField] private Transform targetPointTransform;
    [SerializeField] private Transform referenceHand;

    private ParticleSystem[] particleEffects;
    Hero target;
    //private float waitTimeToStart = .45f;
    //private float lightningShowTime = .55f;

    private void Awake() {
        particleEffects = new ParticleSystem[]{
            GetComponent<ParticleSystem>(),
            GetComponentInChildren<ParticleSystem>()
        };
        StopEffects();
    }

    public void Setup(Hero target, float waitTime, float showTime) {
        this.target = target;
        StartCoroutine(ShowLightningVisual(waitTime, showTime));
    }

    private IEnumerator ShowLightningVisual(float waitTime, float showTime) {
        yield return new WaitForSeconds(waitTime);
        //HandlePosition();
        PlayEffects();
        yield return new WaitForSeconds(showTime);
        StopEffects();

    }

    private void PlayEffects() {
        transform.position = referenceHand.transform.position;

        targetPointTransform.position = new Vector3(target.transform.position.x,
        referenceHand.transform.position.y,
            target.transform.position.z);

        foreach (var effect in particleEffects) {
            effect.Play();
        }
    }

    private void StopEffects() {

        foreach (var effect in particleEffects) {
            effect.Stop();
        }
    }
 
}
