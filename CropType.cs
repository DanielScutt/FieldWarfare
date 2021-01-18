using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CropType : MonoBehaviour
{
    [SerializeField] cropTypes thisCrop;

    public string GetCropType()
    {
        return thisCrop.ToString();
    }
}
