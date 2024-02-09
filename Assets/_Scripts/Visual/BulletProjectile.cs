using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {

    //private TrailRenderer trailRenderer;
    private Vector3 targetPos;
    private Vector3 moveDir;
    private float waitTime;


    public void Setup(Vector3 targetPos, float animationWaitTime ) {
        this.targetPos = targetPos;
        this.targetPos.y = transform.position.y;

        moveDir = (targetPos - transform.position).normalized;
        waitTime = animationWaitTime;
    }

    //private void Awake() {
    //    trailRenderer = GetComponent<TrailRenderer>();
    //}

    void Update() {
        //Vector3 moveDir = (targetPos - transform.position).normalized;
        if (waitTime >= 0) {
            waitTime -= Time.deltaTime;
            return;
        }

        float moveSpeed = 100f;
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPos);

        transform.position += moveSpeed * Time.deltaTime * moveDir;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPos);

        if (distanceBeforeMoving < distanceAfterMoving) {
            transform.position = targetPos;
            //trailRenderer.transform.parent = null;
            //Instantiate(bulletHitVFX, targetPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}