using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwordSkill : Skill
{
    // 1. Skill throws sword towards your mouse position, starts with PlayerAimSwordState.cs, while holding skill, dots are drawn
    // 2. GenerateDots created the necessary dots and puts them in an array
    // 3. DotsPosition is called via Update(), mimics sword-physics values and draws on places dots on path
    // 4. CreateSword() is called via animation trigger when releasing button. AimSword-clip -> ThrowSword-Clip
    // 5. CreateSword() instantiates a sword, and sets up its SwordSkillController.cs which gives force
    // 6. AimDirection checks mouse-world-position vs player position and gets direction compared to them.
    // 7. FinalDir of AimDirection adds force to the vector 2 and launches sword towards finalDir. 

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    private Vector2 finalDir;  // Direction sword faces on impact 

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1)) // Takes every dot from dots-array and gives a position on flight path
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword() // Called Via animation trigger at end of SwordThrowAnim
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        newSwordScript.SetupSword(finalDir, swordGravity);

        DotsActive(false); 
    }

    public Vector2 AimDirection() // Gets direction aimed by taking mouse position - player position
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        // Example-math: If Mouse X,Y = 10, 10 and Player X,Y is 5,5 Direction is 5,5 from player which is 45 degrees

        return direction; 
    }

    public void DotsActive(bool _isActive) // Activates while aiming, deactivates when released and CreateSword() runs
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots() // Generates the amount of dots needed and places them in a list
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        // Position takes player position, aim direction, and sets distance between dots. Dots copy sword physical values and then goes same path
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t); 

        return position;
    }
}
