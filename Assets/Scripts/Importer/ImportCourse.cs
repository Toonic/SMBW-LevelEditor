using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class ImportCourse : MonoBehaviour
{
    [InfoBox("These are the base assets, please ignore.", EInfoBoxType.Warning)]
    public TextAsset newLevelAsset;
    public GameObject actorPrefab;
    public GameObject actorsParent;
    public SpriteShapeController spriteShapeController;
    public GameObject levelParent;
    private StringReader stringReader;
    string line;

    public Course course;
    [HorizontalLine]
    [InfoBox("Drop your .yaml inside of ImportLevelAsset below, and press the buttons to start creating.")]
    public TextAsset importLevelAsset;
    private List<ActorObject> actorObjects = new List<ActorObject>();
    private List<SpriteShapeController> spriteShapeControllers = new List<SpriteShapeController>();

    [Button]
    public void NewLevel()
    {
        ClearLevel();
        ImportLevel(newLevelAsset);
        SpawnActors();
        SpawnSpriteShapeControllers();
    }

    [Button]
    public void ImportLevel()
    {
        ClearLevel();
        ImportLevel(importLevelAsset);
        SpawnActors();
        SpawnSpriteShapeControllers();
    }

    [Button]
    public void ExportLevel()
    {
        List<ActorObject> findObjects = FindObjectsOfType<ActorObject>().ToList();
        foreach (var item in findObjects)
        {
            if (!actorObjects.Contains(item)) actorObjects.Add(item);
        }

        List<SpriteShapeController> findSpriteShape = FindObjectsOfType<SpriteShapeController>().ToList();
        foreach (var item in findSpriteShape)
        {
            if (!spriteShapeControllers.Contains(item)) spriteShapeControllers.Add(item);
        }


        //Convert back the actors.
        List<Course.Actor> actors = new List<Course.Actor>();
        foreach (var item in actorObjects)
        {
            if (item != null)
                actors.Add(item.ConvertBack());
        }
        course.actors = actors;

        //Convert back the level.
        course.bgUnit.walls.Clear();
        List<Course.Walls> walls = new List<Course.Walls>();
        foreach (var item in spriteShapeControllers)
        {
            if (item != null)
            {
                Course.Walls wall = new Course.Walls();
                for (int i = 0; i < item.spline.GetPointCount(); i++)
                {
                    wall.Points.Add(item.spline.GetPosition(i));
                }
                course.bgUnit.walls.Add(wall);
            }
        }



        StreamWriter writer = new StreamWriter(Application.dataPath + "/Exports/" + newLevelAsset.name + ".yaml", false);
        Debug.Log("Saving to: " + Application.dataPath + "/Exports/" + newLevelAsset.name + ".yaml");
        writer.Write(course.ToYaml());
        writer.Flush();
        writer.Close();
        writer.Dispose();
        Debug.Log("Done saving");
    }

    [Button]
    public void ClearLevel()
    {
        var children = new GameObject[actorsParent.transform.childCount];

        int i = 0;
        foreach (Transform child in actorsParent.transform)
        {
            children[i] = child.gameObject;
            i++;
        }
        // Destroy the collected child objects
        foreach (var child in children)
        {
            DestroyImmediate(child);
        }

        children = new GameObject[levelParent.transform.childCount];

        i = 0;
        foreach (Transform child in levelParent.transform)
        {
            children[i] = child.gameObject;
            i++;
        }
        // Destroy the collected child objects
        foreach (var child in children)
        {
            DestroyImmediate(child);
        }
    }

    public void SpawnSpriteShapeControllers()
    {
        foreach (var item in course.bgUnit.walls)
        {
            SpriteShapeController shapeController = Instantiate(spriteShapeController.gameObject).GetComponent<SpriteShapeController>();
            shapeController.transform.position = Vector3.zero;
            shapeController.spline.Clear();
            Debug.Log("Size:" + spriteShapeController.spline.GetPointCount());
            if (item.Points[0].z != 0)
            {
                shapeController.transform.position = new Vector3(0, 0, item.Points[0].z);
            }
            for (int i = 0; i < item.Points.Count; i++)
            {
                shapeController.spline.InsertPointAt(i, item.Points[i]);
            }
            spriteShapeControllers.Add(shapeController);
            shapeController.transform.SetParent(levelParent.transform);
        }
    }

    public void SpawnActors()
    {
        foreach (var item in course.actors)
        {
            GameObject obj = Instantiate(actorPrefab);
            obj.GetComponent<ActorObject>().InitActor(item);
            actorObjects.Add(obj.GetComponent<ActorObject>());
            obj.transform.SetParent(actorsParent.transform);

            if (item.Gyaml.Contains("Camera")) obj.SetActive(false);
        }
    }

    public void ImportLevel(TextAsset targetTextAsset)
    {
        stringReader = new StringReader(targetTextAsset.text);
        line = stringReader.ReadLine();

        course = new Course();

        while (line != null)
        {
            line = stringReader.ReadLine();
            if (line != null && line.Contains("Actors:"))
            {
                course.actors = GetActors();
            }
            if (line != null && line.Contains("BgUnits:"))
            {
                course.bgUnit = GetBGUnit();
            }
            if (line != null && line.Contains("Links:") && !line.Contains("InLinks:"))
            {
                line = stringReader.ReadLine();
                while (line != null && !line.Contains("RootAreaHash:"))
                {
                    course.Links.Add(line);
                    line = stringReader.ReadLine();
                }
            }
            if (line != null && line.Contains("RootAreaHash:"))
            {
                course.RootAreaHash = line.Split(":")[1];
            }
            if (line != null && line.Contains("StageParam:"))
            {
                course.StageParam = line.Split(":")[1];
            }
        }
    }

    private Course.BGUnit GetBGUnit()
    {
        Course.BGUnit bgUnit = new Course.BGUnit();

        while (line != null && !line.Contains("Links:"))
        {
            line = stringReader.ReadLine();
            if (line.Contains("ModelType:"))
            {
                bgUnit.ModelType = line.Split(": ")[1];
            }
            if (line.Contains("SkinDivision:"))
            {
                bgUnit.ModelType = line.Split(": ")[1];
            }
            if (line.Contains("Walls:"))
            {
                Course.Walls currentWall = null;
                List<Course.Walls> walls = new List<Course.Walls>();
                while (line != null && !line.Contains("Links:"))
                {
                    line = stringReader.ReadLine();
                    if (line.Contains("ExternalRail:"))
                    {
                        if (currentWall != null) walls.Add(currentWall);
                        currentWall = new Course.Walls();
                    }
                    if (line.Contains("IsClosed:"))
                    {
                        currentWall.IsClosed = line.Split(": ")[1];
                    }
                    if (line.Contains("Translate:"))
                    {
                        currentWall.Points.Add(GetVector3());
                    }
                }
                if (currentWall != null && !walls.Contains(currentWall)) walls.Add(currentWall);
                bgUnit.walls = walls;
            }
        }

        return bgUnit;
    }

    private List<Course.Actor> GetActors()
    {
        Course.Actor currentActor = null;
        List<Course.Actor> actors = new List<Course.Actor>();
        while (line != null && !line.Contains("BgUnits:"))
        {
            line = stringReader.ReadLine();
            if (line.Contains("AreaHash"))
            {
                if (currentActor != null) actors.Add(currentActor);
                currentActor = new Course.Actor();
                currentActor.AreaHash = line.Split("AreaHash: ")[1];
            }
            if (line.Contains("Dynamic:"))
            {
                currentActor.Dynamic += line.Split(":", 2)[1];
                List<Course.Dynamic> dynamics = new List<Course.Dynamic>();
                line = stringReader.ReadLine();
                while (line != null && !line.Contains("Gyaml:"))
                {
                    line = line.Trim();
                    dynamics.Add(new Course.Dynamic(line.Split(":")[0], line.Split(":")[1].Trim()));
                    line = stringReader.ReadLine();
                }
                currentActor.Dynamics = dynamics;
            }
            if (line.Contains("Gyaml:"))
            {
                currentActor.Gyaml = line.Split(": ")[1];
            }
            if (line.Contains("Hash:"))
            {
                currentActor.Hash = line.Split(": ")[1];
            }
            if (line.Contains("InLinks:"))
            {
                currentActor.InLinks = line.Split(": ", 2)[1];
            }
            if (line.Contains("Layer:"))
            {
                currentActor.Layer = line.Split(": ")[1];
            }
            if (line.Contains("Name:"))
            {
                currentActor.Name = line.Split(": ")[1];
            }
            if (line.Contains("Rotate:"))
            {
                currentActor.Rotate = GetVector3();
            }
            if (line.Contains("Scale:"))
            {
                currentActor.Scale = GetVector3();
            }
            if (line.Contains("Translate:"))
            {
                currentActor.Translate = GetVector3();
            }
        }
        if (currentActor != null && !actors.Contains(currentActor)) actors.Add(currentActor);
        return actors;
    }

    private Vector3 GetVector3()
    {
        Vector3 vector3 = Vector3.zero;
        //X Pos
        line = stringReader.ReadLine();
        vector3.x = float.Parse(line.Split("- ")[1]);
        //Y Pos
        line = stringReader.ReadLine();
        vector3.y = float.Parse(line.Split("- ")[1]);
        //Z Pos
        line = stringReader.ReadLine();
        vector3.z = float.Parse(line.Split("- ")[1]);

        return vector3;
    }
}
