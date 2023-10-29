using System.Collections;
using System.Collections.Generic;
using Fushigi;
using Fushigi.Byml;
using Fushigi.course;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

public class ImportCourseReactor : MonoBehaviour
{
    public GameObject spriteShape;
    [InfoBox("The location of a exported RomFS files for Wonder.")]
    public string ImportPath;
    [InfoBox("The location of your mod folder/where you wish for it to be exported.")]
    public string ExportPath;
    Course currentCourse = null;

    [Button]
    public void ImportLevel()
    {
        RomFS.SetRoot(ImportPath);
        currentCourse = new Course("Course001_Course");
        CourseArea area = currentCourse.GetArea("Course001_Main");
        var root = area.GetRootNode();

        BymlArrayNode bgUnitsArray = (BymlArrayNode)((BymlHashTable)root)["BgUnits"];
        foreach (BymlHashTable bgUnit in bgUnitsArray.Array)
        {
            BymlArrayNode wallsArray = (BymlArrayNode)((BymlHashTable)bgUnit)["Walls"];

            foreach (BymlHashTable walls in wallsArray.Array)
            {
                BymlHashTable externalRail = (BymlHashTable)walls["ExternalRail"];
                BymlArrayNode pointsArray = (BymlArrayNode)externalRail["Points"];
                List<UnityEngine.Vector3> pointList = new();
                foreach (BymlHashTable points in pointsArray.Array)
                {
                    var pos = (BymlArrayNode)points["Translate"];
                    float x = ((BymlNode<float>)pos[0]).Data;
                    float y = ((BymlNode<float>)pos[1]).Data;
                    float z = ((BymlNode<float>)pos[1]).Data;
                    pointList.Add(new UnityEngine.Vector3(x, y, z));
                }
                SpawnSpriteShapeControllers(pointList);
            }
        }
    }

    [Button]
    public void ExportLevel()
    {
        //@TODO: Export the level back out.
        // RomFS.SetRoot(ImportPath);
    }

    public void SpawnSpriteShapeControllers(List<UnityEngine.Vector3> points)
    {
        SpriteShapeController shapeController = Instantiate(spriteShape.gameObject).GetComponent<SpriteShapeController>();
        shapeController.transform.position = UnityEngine.Vector3.zero;
        shapeController.spline.Clear();
        Debug.Log("Size:" + shapeController.spline.GetPointCount());
        if (points[0].z != 0)
        {
            shapeController.transform.position = new UnityEngine.Vector3(0, 0, points[0].z);
        }

        for (int i = 0; i < points.Count; i++)
        {
            shapeController.spline.InsertPointAt(i, points[i]);
        }
    }

}
