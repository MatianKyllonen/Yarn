using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public List<Sprite> spriteList;

    // Start is called before the first frame update
    void Start()
    {
        ChangeSprite();
    }

    void ChangeSprite()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteList.Count > 0)
        {
            int randomIndex = Random.Range(0, spriteList.Count);
            spriteRenderer.sprite = spriteList[randomIndex];
        }
    }
}
