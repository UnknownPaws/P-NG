using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private BallScript ballScript;

    [SerializeField] [Range(0.0f, 10.0f)]
    private float yBound = 4.15f;

    [SerializeField] [Range(0.0f, 10.0f)]
    private float speed = 3.5f;

    [SerializeField]
    private bool leftPaddle = true;

    [SerializeField]
    private int aiDiff;

    #region Constants
    public const int HUMAN = 0;
    public const int EASY = 1;
    public const int MEDIUM = 2;
    public const int HARD = 3;
    #endregion


    // Update is called once per frame
    void Update()
    {
        if (!gameManager.getPauseState())
        {
            float movement = 0f;

            if (aiDiff == HUMAN)
            {
                movement = leftPaddle ? Input.GetAxisRaw("Vertical WASD") : Input.GetAxisRaw("Vertical Arrows");
            }

            else
            {
                float moveY = Mathf.Clamp(ballScript.predictY(transform.position.x), -yBound, yBound);

                movement = transform.position.y - 0.2f < moveY && transform.position.y + 0.2f > moveY ? 0 :aiDiff * Mathf.Sign(moveY - transform.position.y); //Stops it from haveing a seizure close to goal
            }

            float paddleY = Mathf.Clamp(
                    transform.position.y + movement * speed * Time.deltaTime,
                    -yBound, yBound);

            transform.position = new Vector3(transform.position.x, paddleY, 0);
        }
    }


    public void setAiDiff(int aiDiff)
    {
        this.aiDiff = aiDiff;
    }
}
