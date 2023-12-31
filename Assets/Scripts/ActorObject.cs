using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ActorObject : MonoBehaviour
{
    public Course.Actor actor;
    public TMPro.TextMeshPro text;
    public SpriteRenderer spriteRenderer;

    public void ConfigureSprite()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(actor.Gyaml);
        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            text.gameObject.SetActive(false);
        }
    }

    public void ConfigureName()
    {
        if (gameObject.name != actor.Gyaml) gameObject.name = actor.Gyaml;
    }

    public void InitActor(Course.Actor inActor)
    {
        actor = inActor;
        gameObject.name = actor.Gyaml;
        text.text = actor.Gyaml;
        transform.position = actor.Translate;
        transform.localScale = actor.Scale;
        transform.rotation = Quaternion.Euler(actor.Rotate);

        ConfigureSprite();
        ConfigureName();
    }

    public Course.Actor ConvertBack()
    {
        actor.Translate = transform.position;
        actor.Scale = transform.localScale;
        actor.Rotate = transform.localRotation.eulerAngles;
        if (actor.InLinks == null || actor.InLinks == "")
            actor.Hash = "!ul " + Random.Range(0, 2147483646);
        return actor;
    }

}
