using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 10f;

    [Header("Animation")]
    public Animator animator;
    public string speedParam = "Speed";
    public float animDampTime = 0.1f;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Old Input System axes (fast for teaching)
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        // Move relative to world (simple)
        Vector3 move = input * moveSpeed;

        // CharacterController handles gravity if we apply a small downward force
        Vector3 gravity = Vector3.down * 9.81f;

        controller.Move((move + gravity) * Time.deltaTime);

        // Rotate to face movement direction (only if moving)
        if (input.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(input, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // Drive animation blend
        float speed01 = input.magnitude; // 0 idle, 1 walking
        animator.SetFloat(speedParam, speed01, animDampTime, Time.deltaTime);
    }
}
