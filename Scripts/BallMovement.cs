using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour {
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float speedIncrease = 0.25f;
    [SerializeField] private Text playerScore;
    [SerializeField] private Text aiScore;

    private int hitCounter;
    private Rigidbody2D rigidBody;
    
    private void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        Invoke(nameof(StartBall), 2f);
    }

    private void FixedUpdate() {
        rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, initialSpeed + hitCounter * speedIncrease);
    }

    private void StartBall() {
        // Create vector for moving the ball to player at start of round
        rigidBody.velocity = new Vector2(-1, 0) * (initialSpeed + hitCounter * speedIncrease);
    }

    // Resets ball at start of the game or round
    private void ResetBall() {
        rigidBody.velocity = Vector2.zero;
        // Move ball to center on round end
        transform.position = Vector2.zero;
        hitCounter = 0;
        Invoke(nameof(StartBall), 2f);
    }

    // Sets ball velocity vector on bouncing off player
    private void PlayerBounce(Transform playerTransform) {
        hitCounter++;

        Vector2 ballPosition = transform.position;
        Vector2 playerPosition = playerTransform.position;

        float xDirection = ballPosition.x > 0 ? -1 : 1;
        
        float yDirection = (ballPosition.y - playerPosition.y) /
                           playerTransform.GetComponent<Collider2D>().bounds.size.y;
        if (yDirection == 0) {
            yDirection = 0.25f;
        }

        rigidBody.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + hitCounter * speedIncrease); 
    }

    // Handles collision with player
    private void OnCollisionEnter2D(Collision2D collision) {
        string collisionGameObjectName = collision.gameObject.name;
        if (collisionGameObjectName is "Player" or "AI") {
            PlayerBounce(collision.transform);
        }
    }

    // Handles editing score on triggering scoring zone
    private void OnTriggerEnter2D(Collider2D collider) {
        float ballPositionX = transform.position.x;
        if (ballPositionX == 0) return;
        
        if (ballPositionX > 0) {
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
        } else  {
            aiScore.text = (int.Parse(aiScore.text) + 1).ToString();
        }
        
        ResetBall();
    }
}
