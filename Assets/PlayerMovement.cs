using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Vector2 targetPosition;
    private bool isMovingToPoint = false;
    private Rigidbody2D rb;
    private GridManager gridManager; // Reference to the GridManager

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gridManager = FindObjectOfType<GridManager>(); // Find and assign the GridManager instance
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene.");
        }
        targetPosition = transform.position; // Initialize target position with the current position to avoid null references
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            SetTargetPosition();
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0 || movement.y != 0)
        {
            isMovingToPoint = false; // Interrupt mouse movement if keyboard is used
        }
    }

    void FixedUpdate()
    {
        if (isMovingToPoint)
        {
            MoveToPoint();
        }
        else
        {
            MoveWithKeyboard();
        }
    }

    void SetTargetPosition()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Ensure the target position is within the grid bounds
        targetPosition = ClampToGridBounds(targetPosition);
        isMovingToPoint = true;
    }

    Vector2 ClampToGridBounds(Vector2 position)
    {
        if (gridManager == null) return position;
        
        // Clamp the position to the grid size, considering the grid starts at (0,0)
        position.x = Mathf.Clamp(position.x, 0, gridManager.gridSize - 1);
        position.y = Mathf.Clamp(position.y, 0, gridManager.gridSize - 1);
        return position;
    }

    void MoveWithKeyboard()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        // Clamp newPosition to ensure it doesn't go outside the grid bounds
        newPosition = ClampToGridBounds(newPosition);
        rb.MovePosition(newPosition);
    }

    void MoveToPoint()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (newPosition == targetPosition)
        {
            isMovingToPoint = false;
        }
    }
}
