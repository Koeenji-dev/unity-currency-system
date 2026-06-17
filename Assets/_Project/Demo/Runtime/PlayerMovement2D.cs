using UnityEngine;
using UnityEngine.InputSystem;

namespace KoeenjiDev.CurrencySystem.Demo
{
    public sealed class PlayerMovement2D : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D body;

        [SerializeField, Min(0f)]
        private float movementSpeed = 5f;

        private Vector2 movementInput;

        private void Awake()
        {
            if (body == null)
            {
                Debug.LogError(
                    $"{nameof(PlayerMovement2D)} requires a {nameof(Rigidbody2D)} reference. " +
                    "Assign it in the Inspector.",
                    this);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            if (body == null)
            {
                return;
            }

            body.linearVelocity = movementInput * movementSpeed;
        }
    }
}
