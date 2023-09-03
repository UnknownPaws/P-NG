using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [HideInInspector]
    private AudioManager audioManager;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    private Rigidbody2D ballRb;

    [SerializeField]
    [Range(0, 20)]
    private float initSpeed = 3f;

    private float speed;

    [SerializeField]
    [Range(0, 5)]
    private float speedIncrement = 0.5f;

    [SerializeField]
    private TrailRenderer ballTrail;

    [SerializeField]
    private ParticleSystem leftPs, rightPs;

    private bool moreSpeed;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        speed = initSpeed;
    }

    private void Update()
    {
        if (gameManager.getRoundState() == GameManager.INTERVAL && !gameManager.getPauseState() && Input.GetKeyDown(KeyCode.Space))
        {
            startRound();
        }
    }

    private void FixedUpdate()
    {
        if (moreSpeed)
        {
            increaseSpeed();
            moreSpeed = false;
            maintainSpeed(ballRb.velocity); //Dont even ask what this attempt of a fix is
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Paddle"))
        {
            moreSpeed = true;
            audioManager.play("Hit Paddle");

            if (collision.gameObject.CompareTag("Left"))
            {
                leftPs.Play();

            }
            else if (collision.gameObject.CompareTag("Right"))
            {
                rightPs.Play();
            }
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            gameManager.setRoundState(GameManager.INTERVAL);
            audioManager.play("Score Point");

            if (collision.gameObject.tag == "Left Goal")
            {
                gameManager.setRoundState(GameManager.RIGHT_WON);
            }
            else if (collision.gameObject.tag == "Right Goal")
            {
                gameManager.setRoundState(GameManager.LEFT_WON);
            }
            else { Debug.LogError("Round end triggered without winner"); }

            resetBall();
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            audioManager.play("Hit Wall");
        }

    }

    #region Non-Unity Methods

    [ContextMenu("Start Round")]
    private void startRound()
    {
        gameManager.setRoundState(GameManager.ONGOING_MATCH);
        ballTrail.time = 0.2f;

        int leftRightDir = Random.Range(0, 2) == 0 ? -1 : 1; //50/50 on the left right init direction
        Vector2 initVector = new Vector2(leftRightDir, Random.Range(-1f, 1f));

        maintainSpeed(initVector);
    }

    private void increaseSpeed() { speed += speedIncrement; }

    private void maintainSpeed(Vector2 velocity)
    {
        ballRb.velocity = velocity / velocity.magnitude * speed; //Multiply the unit vector by speed
    }

    private void resetBall()
    {
        ballTrail.time = 0;
        speed = initSpeed;
        ballRb.velocity = Vector2.zero;
        transform.position = new Vector3(0, 0, 0);
    }

    public float predictY(float paddleX)
    {
        float predictedY = ((ballRb.velocity.y * (paddleX - transform.position.x) / ballRb.velocity.x) + transform.position.y + 5) % 10.0f - 5; //honestly this is lowkey just sketchy maths that happens to make a good AI

        return predictedY;
    }

    #endregion
}
