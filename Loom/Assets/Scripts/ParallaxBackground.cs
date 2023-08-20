using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    // Set this to the parent background gameObject. It and all its children will follow at designated speed
    // parallaxEffect decides speed. 1 = doesn't move. 0.9, moves slightly etc etc
    // You can toggle if it is endless or not in inspector
    // You can toggle if it should move with Y position also
    // BUG: Why can't I get the vertical endless to work? Also vertical Parallax moves position

    private GameObject cam;

    public bool shouldMoveHorizontally = true;
    public bool shouldMoveVertically = false;
    public bool isEndless = false;


    [SerializeField] private float parallaxEffect = 1;

    private float xPosition;
    private float yPosition;
    private float lengthX;
    private float lengthY;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    void Update()
    {
        if (!isEndless)
            BasicParallaxEffect();
        else
            EndlessParallaxEffect(); 
    }

    private void BasicParallaxEffect() // Is not endless and does not repeat image
    {
        if (shouldMoveHorizontally)
        {
            float distanceToMoveX = cam.transform.position.x * parallaxEffect;
            transform.position = new Vector3(xPosition + distanceToMoveX, transform.position.y);
        }

        if (shouldMoveVertically)
        {
            float distanceToMoveY = cam.transform.position.y * parallaxEffect;
            transform.position = new Vector3(transform.position.x, yPosition + distanceToMoveY);
        }
    }

    private void EndlessParallaxEffect() // Picture repeats itself, doesn't work with vertical though? 
    {
        if (shouldMoveHorizontally)
        {
            float distanceMovedX = cam.transform.position.x * (1 - parallaxEffect);
            float distanceToMoveX = cam.transform.position.x * parallaxEffect;

            transform.position = new Vector3(xPosition + distanceToMoveX, transform.position.y);

            if (distanceMovedX > xPosition + lengthX)
                xPosition = xPosition + lengthX;
            else if (distanceMovedX < xPosition - lengthX)
                xPosition = xPosition - lengthX;
        }

        if (shouldMoveVertically)
        {
            float distanceMovedY = cam.transform.position.y * (1 - parallaxEffect);
            float distanceToMoveY = cam.transform.position.y * parallaxEffect;

            transform.position = new Vector3(transform.position.x, yPosition + distanceMovedY);

            if (distanceMovedY > yPosition + lengthY)
                yPosition = yPosition + lengthY;
            else if (distanceMovedY < yPosition - lengthY)
                yPosition = yPosition - lengthY;
        }
    }
}
