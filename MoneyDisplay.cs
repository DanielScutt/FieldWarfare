using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    UIManager uiManager;
    public Text moneyText;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "£" + uiManager.GetMoney().ToString();    
    }
}
