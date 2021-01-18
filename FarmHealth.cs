using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmHealth : MonoBehaviour
{
    UIManager uiManager;
    Slider slider;
    public GameObject bar;
    public Text healthText;
    bool checkCrop;

    float maxAmount;

    // Start is called before the first frame update
    void Start()
    {
        slider = bar.GetComponent<Slider>();
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiManager.IsNightTime())
        {
            if(checkCrop)
            {
                maxAmount = uiManager.GetCropCount();
                healthText.text = uiManager.GetCropCount() + " / " + maxAmount;
                checkCrop = false;
            }
            bar.SetActive(true);
            slider.value = (uiManager.GetCropCount() / maxAmount);
            healthText.text = uiManager.GetCropCount() + " / " + maxAmount;
        }
        else
        {
            bar.SetActive(false);
            checkCrop = true;
        }
    }
}
