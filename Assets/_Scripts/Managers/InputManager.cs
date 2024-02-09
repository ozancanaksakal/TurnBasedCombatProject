using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<Hero> OnTouchToHero;

    private PlayerInputActions playerInputActions;

    [SerializeField] private LayerMask characterLayer;
    private void Awake() {
        Instance = this;
        playerInputActions = new();
        playerInputActions.Touch.Enable();
        playerInputActions.Touch.Press.performed += Press_performed;
    }
    private void OnDisable() {
        playerInputActions.Touch.Disable();
    }

    private void Press_performed(InputAction.CallbackContext obj) {

        if (TryGetCharacterOnTouch(out Hero hero)) {
            //Debug.Log(hero.name);
            OnTouchToHero?.Invoke(hero);
        }
    }

    private Vector2 GetTouchPosition() {
        return playerInputActions.Touch.PressPosition.ReadValue<Vector2>();
    }

    private bool TryGetCharacterOnTouch(out Hero hero) {
        hero = null;
        Ray ray = Camera.main.ScreenPointToRay(GetTouchPosition());

        if (
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, characterLayer)) {
            //return hitInfo.transform.TryGetComponent<BaseCharacter>(out baseCharacter);
            hero = hitInfo.transform.GetComponent<Hero>();
            return true;
        }
        return false;
    }
   
}