using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMPPCard : MonoBehaviour {
    public Sprite frontSprite;
    public Sprite backSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start() {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(Vector3.Angle(transform.up, transform.position - Camera.main.transform.position));
        if (Vector3.Angle(transform.up, transform.position - Camera.main.transform.position) <= 90) {
            spriteRenderer.sprite = backSprite;
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.sprite = frontSprite;
            spriteRenderer.flipX = false;
        }
    }
}
