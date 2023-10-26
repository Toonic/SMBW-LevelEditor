using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorObject : MonoBehaviour
{
    public Course.Actor actor;
    public TMPro.TextMeshPro text;

    public void InitActor(Course.Actor inActor)
    {
        actor = inActor;
        gameObject.name = actor.Gyaml;
        text.text = actor.Gyaml;
        transform.position = actor.Translate;
        transform.localScale = actor.Scale;
        transform.rotation = Quaternion.Euler(actor.Rotate);
    }

    public Course.Actor ConvertBack()
    {
        actor.Translate = transform.position;
        actor.Scale = transform.localScale;
        actor.Rotate = transform.localRotation.eulerAngles;
        if (actor.InLinks == null)
            actor.Hash = "!ul " + Random.Range(0, 2147483646);
        return actor;
    }

}
