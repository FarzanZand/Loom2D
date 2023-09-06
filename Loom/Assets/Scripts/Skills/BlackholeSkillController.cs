using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    // 1. Creates a blackHole and puts a hotkey on enemies, hitting it adds them to a damage-list. 
    // 2. When blackhole collides with an enemy, freeze time for that enemy, and create a hotkey above
    // 3. Hotkey is instantiated from prefab, and chosenKey picks a key from the list in inspector. That key is removed from list after
    // 4. Its controller is created and prepped, and passed to the new instantiated hotkey
    // 5. in BlackholeHotkeyController.cs connected to each enemy, press the hotkey adds them to targets-list. 
    // 6. When you then press R, you will destroy hotkeys on screen so no new ones are created while skill is closing
    // 7. Attack will initiate with cloneAttackReleased, which will instantiate a copy of the CloneSkill on each target randomly
    // 8. After all attacks are done, skill will shrink and get destroyed when it hits 0 with canShrink

    [SerializeField] private GameObject HotkeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    public bool canGrow;
    public bool canShrink;

    private bool canCreateHotkeys = true;
    private bool cloneAttackReleased;
    public int amountOfAttacks = 4;
    public float cloneAttackCooldown = 0.3f;
    public float cloneAttackTimer; 

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyHotkeys();
            cloneAttackReleased = true;
            canCreateHotkeys = false; 
        }

        if (cloneAttackTimer < 0 && cloneAttackReleased) 
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 1.5f;
            else
                xOffset = -1.5f;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0)); // Use the already created cloneskill on location
            amountOfAttacks--;

            if(amountOfAttacks <= 0)
            {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }


        if (canGrow && !canShrink) // Shrink the skill and destroy it when 0
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

        if (transform.localScale.x < 0)
            Destroy(gameObject);
    }

    private void DestroyHotkeys() // Destroy all hotkeys remaining after skill ends
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Freeze time on enemies in collision, and create hotkey above them
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }

    private void CreateHotkey(Collider2D collision) //
    {
        if (KeyCodeList.Count <= 0)
        {
            Debug.Log("Not enough hotkeys in keycode list"); // Perhaps put a cap on how many you can target?
            return;
        }

        if (!canCreateHotkeys) // Becomes false after ult ends with R, so no new ones are created while ult is closing
            return;

        GameObject newHotkey = Instantiate(HotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotkey);

        KeyCode chosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(chosenKey); // Inspector-error. For some reason, when you have toggled on to show the hotkeys, gets 1000 exceptions. When minimized, works fine

        BlackholeHotkeyController newHotkeyScript = newHotkey.GetComponent<BlackholeHotkeyController>();

        newHotkeyScript.SetupHotkey(chosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);  
}