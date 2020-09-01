using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMPPCard : MonoBehaviour
{
    public Sprite frontSprite;
    public Sprite backSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(transform.up, Camera.current.transform.forward) < 90) {
            spriteRenderer.sprite = frontSprite;
        } else {
            spriteRenderer.sprite = backSprite;
        }
    }
}
