using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    [SerializeField] List<GameObject> fieldList;
    [SerializeField] List<GameObject> grownCropList;
    public List<GameObject> cropList;

    bool isNight = false;

    public void AddField(GameObject crop)
    {
        fieldList.Add(crop);
    }

    public GameObject GetRandomField()
    {
        foreach (GameObject field in fieldList)
        {
            Debug.Log(field.GetComponent<SimpleField>().HasCrops());

            if (field.GetComponent<SimpleField>().HasCrops())
            {
                return field;
            }
        }

        return null;
    }

    public void NightTime()
    {
        if(!isNight)
        {
            foreach (GameObject field in fieldList)
            {
                if (field != null)
                {
                    foreach (GameObject crop in field.GetComponent<SimpleField>().cropsInField)
                    {
                        cropList.Add(crop);
                    }
                }
            }

            isNight = true;
        }
    }

    public void NextDay()
    {
        isNight = false;
        foreach (GameObject field in fieldList)
        {
            if (field != null)
            {
                Debug.Log("dd");
                field.GetComponent<SimpleField>().Grow();
                cropList.Clear();
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown("i"))
        {
            NextDay();
        }
    }

    public GameObject getCrop()
    {
        Debug.Log(Random.Range(0, cropList.Count));
        return cropList[Random.Range(0, cropList.Count)];
    }

    public int GetCropCount()
    {
        return cropList.Count;
    }

    public void RemoveCrop(GameObject crop)
    {
        cropList.Remove(crop);
    }
}
