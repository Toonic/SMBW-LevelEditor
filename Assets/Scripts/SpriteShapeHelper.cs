using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class SpriteShapeHelper : MonoBehaviour
{
    public SpriteShapeController controller;

    void Start()
    {
        transform.position = Vector3.zero;
    }

    void Update()
    {
        for (int i = 0; i < controller.spline.GetPointCount(); i++)
        {
            controller.spline.SetPosition(i, RoundVector3ToInt(controller.spline.GetPosition(i)));
        }
    }

    Vector3Int RoundVector3ToInt(Vector3 vector3)
    {
        return new Vector3Int(
            Mathf.RoundToInt(vector3.x),
            Mathf.RoundToInt(vector3.y),
            Mathf.RoundToInt(vector3.z)
        );
    }

}
