using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayDisplay : MonoBehaviour
{
    UIManager uiManager;
    public Text dayText;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        dayText.text = "Day: " + uiManager.gameTime.GetCurrentDay().ToString();
    }
}
