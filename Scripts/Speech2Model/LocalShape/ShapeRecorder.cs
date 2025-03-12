using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton. Reads all existing geometries in the scene.
/// </summary>
public class ShapeRecorder : MonoBehaviour
{
    [SerializeField] private Transform SearchFolder;


    #region PUBLIC METHOD
    public void GetAllShapes()
    {
        foreach(Transform child in SearchFolder)
        {
            
        }
    }
    #endregion



    #region LIFE CYCLE
    void Start()
    {
        GetAllShapes();
    }

    // Create Singleton
    public static ShapeRecorder Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}

public class Shape
{

}
