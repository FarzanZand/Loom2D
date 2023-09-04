using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    // 1. Creates a blackHole and puts a hotkey on enemies, hitting it adds them to a damage-list. 
    // 2. When blackhole collides with an enemy, freeze time for that enemy, and create a hotkey above
    // 3. Hotkey is instantiated from prefab above enemy, and chosenKey picks a key from the list in inspector
    // 4. Its controller is created and prepped, and passed to the new instantiated hotkey
    // 5. in BlackholeHotkeyController.cs connected to each enemy, press the hotkey adds them to targets-list. 

    [SerializeField] private GameObject HotkeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;

    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private List<Transform> targets = new List<Transform>();

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }

    private void CreateHotkey(Collider2D collision)
    {
        if (KeyCodeList.Count <= 0)
        {
            Debug.Log("Not enough hotkeys in keycode list"); // Perhaps put a cap on how many you can target?
            return;
        }

        GameObject newHotkey = Instantiate(HotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode chosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(chosenKey); // Inspector-error. For some reason, when you have toggled on to show the hotkeys, gets 1000 exceptions. When minimized, works fine

        BlackholeHotkeyController newHotkeyScript = newHotkey.GetComponent<BlackholeHotkeyController>();

        newHotkeyScript.SetupHotkey(chosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);  
}