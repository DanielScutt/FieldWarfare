using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    UIManager uIManager;
    public Text itemName;
    public Text itemAmount;
    public Text itemControls;

    // Start is called before the first frame update
    void Start()
    {
        uIManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(uIManager.item() != null)
        {
            GetComponent<Image>().enabled = true;
            itemName.text = uIManager.item().GetItemName();
            itemControls.text = uIManager.item().GetItemControls();
            if(uIManager.item().GetItemAmount() == "null")
            {
                itemAmount.text = "";
            }
            else
            {
                itemAmount.text = uIManager.item().GetItemAmount();
            }
        }
        else
        {
            GetComponent<Image>().enabled = false;
            itemName.text = "";
            itemAmount.text = "";
            itemControls.text = "";
        }
    }
}
