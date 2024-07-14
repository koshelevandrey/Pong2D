using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isAI;
    [SerializeField] private GameObject ball;

    private Rigidbody2D rigidBody;
    private Vector2 playerMove;

    private void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (isAI) {
             AIControl();
        }
        else {
            PlayerControl();
        }
    }

    private void FixedUpdate() {
        rigidBody.velocity = playerMove * movementSpeed;
    }

    private void PlayerControl() {
        playerMove = new Vector2(0, Input.GetAxisRaw("Vertical"));
    }

    private void AIControl() {
        var ballPositionY = ball.transform.position.y;
        var playerPositionY = transform.position.y;
        if (ballPositionY > playerPositionY + 0.5f) {
            playerMove = new Vector2(0, 1);
        }
        else if (ballPositionY < playerPositionY - 0.5f) {
            playerMove = new Vector2(0, -1);
        }
        else {
            playerMove = Vector2.zero;
        }
    }
}
