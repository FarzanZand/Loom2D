using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    // Parentclass for the tooltips
    // When mouse is on top halv, show tooltip below. Otherwise, reverse. 
    // Add CanvasGroup component to and disable interactable and blocks raycasts to stop the flickering. 

    [SerializeField] private float xLimit = 960;        // Half of 1920, 1080
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;

        if (mousePosition.x > xLimit)           // Mouse is on the right side of screen
            newXoffset = -xOffset;
        else
            newXoffset = xOffset;

        if (mousePosition.y > yLimit)           // Mouse is on the top half of screen
            newYoffset = -yOffset;
        else
            newYoffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXoffset, mousePosition.y + newYoffset);
    }

    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
            _text.fontSize = _text.fontSize * .8f;
    }
}
