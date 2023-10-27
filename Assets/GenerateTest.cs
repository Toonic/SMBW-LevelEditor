using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;

public class GenerateTest : MonoBehaviour
{

    public GameObject actorPrefab;
    public TextAsset textAsset;
    private StringReader stringReader;
    public int count = 0;
    public int startingCount = 0;

    [Button]
    public void GenerateAll()
    {
        StartCoroutine(DoTheThing());
    }

    private IEnumerator DoTheThing()
    {
        count = startingCount;
        stringReader = new StringReader(textAsset.text);
        string line = stringReader.ReadLine();
        Vector3 spawnPos = Vector3.zero;
        while (line != null)
        {
            if (line.StartsWith("Enemy") || line.StartsWith("Object") || line.StartsWith("Block"))
            {
                spawnPos = Vector3.right * count * 2;
                spawnPos += Vector3.up * 5;
                GameObject obj = Instantiate(actorPrefab);
                obj.GetComponent<ActorObject>().actor.Gyaml = line;
                obj.transform.position = spawnPos;
                obj.transform.localScale = Vector3.one;

                count++;
            }
            line = stringReader.ReadLine();
        }

        yield return null;
    }

}
