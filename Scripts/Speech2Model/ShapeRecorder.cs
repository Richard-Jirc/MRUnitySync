using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton. Reads all existing geometries in the scene.
/// </summary>
public class ShapeRecorder : MonoBehaviour
{
    [SerializeField] private Transform SearchFolder;


    private void GetAllShapes()
    {
        foreach(Transform child in SearchFolder)
        {
            Debug.Log(child.name);
        }
    }


    #region LIFE CYCLE
    void Start()
    {
        GetAllShapes();
    }


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
