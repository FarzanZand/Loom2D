using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{

    // 1. Setup this controller by passing in the desired hotkey
    // 2. Changes the text in the child to the hotkey
    // 3. Sets that input required is the selected hotkey created in constructor

    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackHole; 

    public void SetupHotkey(KeyCode _myHotkey, Transform _myEnemy, BlackholeSkillController _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotkey = _myHotkey;
        myText.text = _myHotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}