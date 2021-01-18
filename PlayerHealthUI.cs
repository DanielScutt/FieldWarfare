using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    UIManager uiManager;
    Slider slider;
    public GameObject bar;
    public Text healthText;
    bool checkHealth;
    public Health playerHealth;

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
        if (uiManager.IsNightTime())
        {
            if (checkHealth)
            {
                maxAmount = playerHealth.healthAmount;
                healthText.text = playerHealth.healthAmount.ToString();
                checkHealth = false;
            }
            bar.SetActive(true);
            slider.value = (playerHealth.healthAmount / playerHealth.GetMaxHealth());
            healthText.text = playerHealth.healthAmount.ToString();
        }
        else
        {
            bar.SetActive(false);
            checkHealth = true;
        }
    }
}
