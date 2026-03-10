using UnityEngine;

public class HorizontalDragController3D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Rigidbody targetRb;           // ← Drag your 3D object with Rigidbody here

    [Header("Horizontal Control (X-axis)")]
    [SerializeField, Tooltip("Strength of drag → velocity (screen-width normalized)")]
    private float sensitivity = 0.22f;

    [SerializeField, Tooltip("Maximum |horizontal| velocity from dragging")]
    private float maxHorizontalSpeed = 14f;

    [SerializeField, Tooltip("How quickly horizontal velocity returns to 0 after release")]
    private float releaseDamping = 6.5f;

    [Header("Input Feel")]
    [SerializeField, Range(5f, 30f)]
    private float deadZonePixels = 14f;                     // ignore tiny jitters/taps

    [SerializeField, Range(0.04f, 0.25f)]
    private float minDragScreenFractionToStart = 0.08f;     // require ~8% screen drag to begin

    // Runtime
    private Vector2 pointerStartPos;
    private Vector2 lastPointerPos;
    private bool isDragging;
    private bool wasPointerDownLastFrame;

    private void Awake()
    {
        if (targetRb == null)
        {
            Debug.LogWarning($"{nameof(HorizontalDragController3D)} on {name}: No target Rigidbody assigned!", this);
        }
        else
        {
            // Optional: if you want no gravity or rotation interference
            // targetRb.useGravity = false;
            // targetRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        if (targetRb == null) return;

        // ────────────────────────────────────────────────
        // 1. Gather current pointer input
        // ────────────────────────────────────────────────
        Vector2 currentPos = default;
        bool isPointerDownNow = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            currentPos = touch.position;
            isPointerDownNow = true;
        }
        else if (Input.GetMouseButton(0))
        {
            currentPos = Input.mousePosition;
            isPointerDownNow = true;
        }

        // ────────────────────────────────────────────────
        // 2. Detect drag start / end
        // ────────────────────────────────────────────────
        if (isPointerDownNow && !wasPointerDownLastFrame)
        {
            isDragging = true;
            pointerStartPos = currentPos;
            lastPointerPos = currentPos;
        }
        else if (!isPointerDownNow && wasPointerDownLastFrame)
        {
            isDragging = false;
        }

        wasPointerDownLastFrame = isPointerDownNow;

        // ────────────────────────────────────────────────
        // 3. Apply horizontal velocity while dragging
        // ────────────────────────────────────────────────
        if (isDragging)
        {
            Vector2 delta = currentPos - lastPointerPos;
            float xDeltaPixels = delta.x;

            if (Mathf.Abs(xDeltaPixels) < deadZonePixels)
            {
                lastPointerPos = currentPos;
                return;
            }

            // Normalize to screen fraction (-1 … +1 ≈ full screen drag)
            float xDeltaNormalized = xDeltaPixels / Screen.width;

            // Optional minimum drag distance filter
            float totalDragNorm = (currentPos - pointerStartPos).magnitude / Screen.width;
            if (totalDragNorm < minDragScreenFractionToStart)
            {
                xDeltaNormalized *= 0.3f; // or set to 0f to fully ignore
            }

            // Velocity change this physics step
            float velocityChangeX = xDeltaNormalized * sensitivity * 50f; // tune multiplier

            Vector3 currentVel = targetRb.linearVelocity;
            float newX = currentVel.x + velocityChangeX;

            // Clamp top speed
            newX = Mathf.Clamp(newX, -maxHorizontalSpeed, maxHorizontalSpeed);

            targetRb.linearVelocity = new Vector3(newX, currentVel.y, currentVel.z);

            lastPointerPos = currentPos;
        }
        // ────────────────────────────────────────────────
        // 4. Dampen horizontal velocity when not dragging
        // ────────────────────────────────────────────────
        else if (releaseDamping > 0.01f)
        {
            Vector3 vel = targetRb.linearVelocity;
            float dampedX = Mathf.MoveTowards(vel.x, 0f, releaseDamping * Time.fixedDeltaTime * 60f);
            targetRb.linearVelocity = new Vector3(dampedX, vel.y, vel.z);
        }
    }

    // Optional: visualize current X-velocity in scene view (yellow line)
    private void OnDrawGizmosSelected()
    {
        if (targetRb == null) return;

        Gizmos.color = new Color(1f, 0.7f, 0.1f, 0.75f);
        Vector3 pos = targetRb.transform.position;
        Gizmos.DrawLine(pos, pos + Vector3.right * targetRb.linearVelocity.x * 0.4f);
    }
}