using System.Collections;
using UnityEngine;

public class HeroAnimator : MonoBehaviour {
    private enum State { Idle, Preparing, Performing, AfterPerform }

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject stunVisualGameObject;

    private State state;
    private Vector3 initialPosition;
    private Vector3 initialForwardVector;
    private float speed = 6f;
    private Hero myHero;

    private int isMovingHash;
    private int takeDamageHash;
    //private int basicActionHash; private int special1Hash; private int special2Hash; private int special3Hash;
    //private Dictionary<ActionType, int> actionHashDict;
    private int[] actionHashArray;

    private BaseAction performerAction;

    private void Awake() {
        isMovingHash = Animator.StringToHash("IsMoving");
        takeDamageHash = Animator.StringToHash("TakeDamage");

        //actionHashDict = CreateActionHashDict();
        CreateActionHashArray();

        state = State.Idle;
        initialForwardVector = transform.forward;
    }

    private void CreateActionHashArray() {
        actionHashArray = new int[] {
        Animator.StringToHash("BasicAction"),
        Animator.StringToHash("Special1"),
        Animator.StringToHash("Special2"),
        Animator.StringToHash("Special3")
        };
    }

    private void Start() {
        initialPosition = transform.position;
        myHero = GetComponent<Hero>();
        myHero.HeroActions.OnActionStarted += HeroActions_OnActionStarted;
        myHero.OnDamaged += MyHero_OnDamaged;
    }

    private void MyHero_OnDamaged() {
        animator.SetTrigger(takeDamageHash);
    }

    private void HeroActions_OnActionStarted() {
        performerAction = myHero.HeroActions.CurrentlyPerformingAction;
        SetState(State.Preparing);
        //Debug.Log(performerAction);
    }

    private void SetState(State newState) {
        state = newState;
        //Debug.Log("Animator state is " + state);
        if (performerAction.visualData.performPosition == ActionPosition.InPlace)
            HandleAnimationInPlace();
        else
            HandleAnimationTargetPosition();
    }

    private void NextState() {
        if (state == State.Idle) SetState(State.Preparing);
        else if (state == State.Preparing) SetState(State.Performing);
        else if (state == State.Performing) SetState(State.AfterPerform);
        else if (state == State.AfterPerform) SetState(State.Idle);
    }

    private void HandleAnimationInPlace() {
        switch (state) {
            case State.Idle:
                animator.SetBool(isMovingHash, false);
                performerAction.CompleteAction();
                break;
            case State.Preparing:
                if (performerAction.visualData.turnToTargetHero) {
                    Vector3 direction = HeroManager.Instance.TargetHero.transform.position - transform.position;
                    StartCoroutine(LookAtTo(direction));
                }
                else
                    SetState(State.Performing);
                break;
            case State.Performing:
                animator.SetTrigger(GetCurrentActionHash());
                StartCoroutine(WaitActionAnimation());
                break;
            case State.AfterPerform:
                if (performerAction.visualData.turnToTargetHero)
                    StartCoroutine(LookAtTo(initialForwardVector));
                else
                    SetState(State.Idle);
                break;
        }
    }

    private void HandleAnimationTargetPosition() {
        switch (state) {
            case State.Idle: {
                    StartCoroutine(LookAtTo(initialForwardVector,false));
                    animator.SetBool(isMovingHash, false);
                    performerAction.CompleteAction();
                    break;
                }
            case State.Preparing: {
                    animator.SetBool(isMovingHash, true);
                    Vector3 movePosition = performerAction.visualData.TargetPosition;
                    float closingDistance = performerAction.visualData.closingDistance;
                    StartCoroutine(MoveTo(movePosition, closingDistance));
                    break;
                }
            case State.Performing: {
                    animator.SetTrigger(GetCurrentActionHash());
                    StartCoroutine(WaitActionAnimation());
                    break;
                }
            case State.AfterPerform: {
                    animator.SetBool(isMovingHash, true);
                    StartCoroutine(MoveTo(initialPosition, 0.5f));
                    break;
                }
        }
    }

    private int GetCurrentActionHash() {
        int index = (int)performerAction.actionType;
        return actionHashArray[index];
    }

    private IEnumerator WaitActionAnimation() {
        yield return new WaitForSeconds(performerAction.visualData.animationTime / 2);
        performerAction.Perform();
        yield return new WaitForSeconds(performerAction.visualData.animationTime / 2);
        SetState(State.AfterPerform);
    }

    private IEnumerator MoveTo(Vector3 targetPosition, float closingDistance) {

        Vector3 direction = (targetPosition - transform.position).normalized;
        StartCoroutine(LookAtTo(direction,false));

        while (Vector3.Distance(transform.position, targetPosition) > closingDistance) {
            transform.position += speed * Time.deltaTime * direction;
            yield return null;
        }

        NextState();
    }

    private IEnumerator LookAtTo(Vector3 direction, bool switchState = true) {
        float rotateSpeed = 10f;
        while (Vector3.Angle(transform.forward, direction) > 1) {

            transform.forward = Vector3.Slerp(transform.forward, direction, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.forward = direction;
        if (switchState)
            NextState();
    }

    
}
