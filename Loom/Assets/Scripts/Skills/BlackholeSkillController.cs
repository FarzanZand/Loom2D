using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    // 1. Creates a blackHole and puts a hotkey on enemies, hitting it adds them to a damage-list. BlackHoleState called via keycode R in PlayerGroundedState
    // 2. When blackhole collides with an enemy, freeze time for that enemy, and create a hotkey above. Unfreezes when out of blackhole
    // 3. Hotkey is instantiated from prefab, and chosenKey picks a key from the list in inspector. That key is removed from list after
    // 4. Its BlackholeHotkeyController is created and prepped, and passed to the new instantiated hotkey
    // 5. in BlackholeHotkeyController.cs connected to each enemy, press the hotkey adds them to targets-list. 
    // 6. When you then press R skill damage/closing phase is activated, you will destroy hotkeys on screen so no new ones are created while skill is closing
    // 7. Attack will initiate which runs ReleaseCloneAttack(), which then enables CloneAttackLogic() by setting cloneAttackReleased bool to true
    // 8. After all attacks are done, or blackhole timer ends via Update(), close skill with FinishBlackholeAbility()

    [SerializeField] private GameObject HotkeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    private float blackholeTimer;

    private bool canGrow = true; 
    private bool canShrink;
    private bool canCreateHotkeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisappear = true;  

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer; 

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; } 

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0)
        {

            blackholeTimer = Mathf.Infinity; // Make sure check works only once

            if(targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink) // Shrink the skill and destroy it when 0
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

        if (transform.localScale.x < 0)
            Destroy(gameObject);
    }

    private void ReleaseCloneAttack() // Can't I just move this to CloneAttackLogic() ?
    {
        if (targets.Count <= 0)
            return; 

        DestroyHotkeys();
        cloneAttackReleased = true;
        canCreateHotkeys = false;

        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.MakeTransparent(true); 
        }

    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
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

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", .5f); // Give a little delay before ability closes
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotkeys();
        playerCanExitState = true; 
        canShrink = true;
        cloneAttackReleased = false;
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

    private void OnTriggerExit2D(Collider2D collision) // Unfreeze enemies when out of sphere
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent <Enemy>().FreezeTime(false);
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