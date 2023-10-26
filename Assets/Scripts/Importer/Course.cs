using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Course
{
    public List<Actor> actors;
    public BGUnit bgUnit;
    public List<string> Links = new List<string>();
    public string RootAreaHash;
    public string StageParam;

    public string ToYaml()
    {
        string yaml = "";
        yaml += "Version: 1" + "\n";
        yaml += "IsBigEndian: False" + "\n";
        yaml += "SupportPaths: False" + "\n";
        yaml += "HasReferenceNodes: False" + "\n";
        yaml += "root:" + "\n";
        yaml += "  Actors:" + "\n";
        foreach (var item in actors)
        {
            yaml += item.ToString();
        }
        yaml += "  BgUnits:" + "\n";
        yaml += bgUnit.ToString();
        yaml += "  Links:" + "\n";
        foreach (var item in Links)
        {
            yaml += "" + item + "\n";
        }
        yaml += "  RootAreaHash:" + RootAreaHash + "\n";
        yaml += "  StageParam:" + StageParam + "\n";

        return yaml;
    }

    [System.Serializable]
    public class Actor
    {
        public string AreaHash;
        public string Dynamic;
        public List<Dynamic> Dynamics;
        public string Gyaml;
        public string Hash;
        public string InLinks;
        public string Layer;
        public string Name;
        public Vector3 Rotate;
        public Vector3 Scale;
        public Vector3 Translate;

        public override string ToString()
        {
            string yaml = "";
            yaml += "    - AreaHash: " + AreaHash + "\n";
            if (Dynamic != null || Dynamics != null)
                yaml += "      Dynamic:" + Dynamic + "\n";
            if (Dynamics != null)
            {
                foreach (var item in Dynamics)
                {
                    yaml += "        " + item.Key + ": " + item.Value.Trim() + "\n";
                }
            }
            yaml += "      Gyaml: " + Gyaml + "\n";
            yaml += "      Hash: " + Hash + "\n";
            if (InLinks != null)
                yaml += "      InLinks: " + InLinks + "\n";
            yaml += "      Layer: " + Layer + "\n";
            yaml += "      Name: " + Name + "\n";
            yaml += "      Rotate:" + "\n";
            yaml += "        - " + Rotate.x.ToString("0.00000") + "\n";
            yaml += "        - " + Rotate.y.ToString("0.00000") + "\n";
            yaml += "        - " + Rotate.z.ToString("0.00000") + "\n";
            yaml += "      Scale:" + "\n";
            yaml += "        - " + Scale.x.ToString("0.00000") + "\n";
            yaml += "        - " + Scale.y.ToString("0.00000") + "\n";
            yaml += "        - " + Scale.z.ToString("0.00000") + "\n";
            yaml += "      Translate:" + "\n";
            yaml += "        - " + Translate.x.ToString("0.00000") + "\n";
            yaml += "        - " + Translate.y.ToString("0.00000") + "\n";
            yaml += "        - " + Translate.z.ToString("0.00000") + "\n";
            return yaml;
        }
    }

    [System.Serializable]
    public class Dynamic
    {
        public string Key;
        public string Value;

        public Dynamic(string inKey, string inValue)
        {
            Key = inKey;
            Value = inValue;
        }
    }

    [System.Serializable]
    public class BGUnit
    {
        //We're ignoring BeltRails currently, because I'm not entirely sure what it is used for as of yet.
        public string ModelType = "!l 0";
        public string SkinDivision = "!l 0";
        public List<Walls> walls;

        public override string ToString()
        {
            string yaml = "";
            yaml += "    - ModelType: " + ModelType + "\n";
            yaml += "      SkinDivision: " + ModelType + "\n";
            yaml += "      Walls:" + "\n";
            foreach (var item in walls)
            {
                yaml += item.ToString();
            }
            return yaml;
        }

    }
    [System.Serializable]
    public class Walls
    {
        //ExternalRail
        public string IsClosed = "true";
        public List<Vector3> Points = new List<Vector3>();

        public override string ToString()
        {
            string yaml = "";
            yaml += "        - ExternalRail:" + "\n";
            yaml += "            IsClosed: " + IsClosed + "\n";
            yaml += "            Points:" + "\n";
            foreach (var item in Points)
            {
                yaml += "              - Translate:" + "\n";
                yaml += "                  - " + item.x.ToString("0.00000") + "\n";
                yaml += "                  - " + item.y.ToString("0.00000") + "\n";
                yaml += "                  - " + item.z.ToString("0.00000") + "\n";
            }

            return yaml;
        }

    }
}