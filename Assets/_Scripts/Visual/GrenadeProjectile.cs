using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{

    [SerializeField] private AnimationCurve arcYAnimationCurve;
    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    
    //private TrailRenderer trailRenderer;
    private Vector3 targetPosition;
    private float totalDistance;
    private float maxHeight;
    private Vector3 positionXZ;
    private float waitTime;
    private Action actionLogic;

    public void Setup(Vector3 targetPosition,float waitTimeBeforeMove, System.Action actionLogic) {
        this.targetPosition = targetPosition;
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
        maxHeight = totalDistance / 4;
        waitTime = waitTimeBeforeMove;
        this.actionLogic = actionLogic;
        
    }

    private void Update() {
        if (waitTime >= 0) {
            waitTime -= Time.deltaTime;
            return;
        }

        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        float moveSpeed = 5f;
        positionXZ += moveSpeed * Time.deltaTime * moveDir;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float normalizedDistance = 1- (distance/totalDistance);

        float posY= arcYAnimationCurve.Evaluate(normalizedDistance)*maxHeight;
        transform.position = new Vector3(positionXZ.x,posY, positionXZ.z);

        if (distance < .2f) {
            actionLogic();
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);

            Destroy(gameObject);
        }
    }

}
