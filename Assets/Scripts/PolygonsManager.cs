using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] polygons;
    private int currentLevel;

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        currentLevel -= 1;
        if(currentLevel >= polygons.Length)
        {
            displayPolygon(polygons.Length - 1);
        }
        else
        {
            displayPolygon(currentLevel);
        }
    }
    private void displayPolygon(int poly)
    {
        for (int i = 0; i < polygons.Length; i++)
        {
            polygons[i].SetActive(false);
        }
        polygons[poly].SetActive(true);
    }
}
