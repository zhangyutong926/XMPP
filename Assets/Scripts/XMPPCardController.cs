using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class XMPPCardController : NetworkBehaviour {

    public GameObject localCardPrefab;
    private GameObject localCard;

    // Start is called before the first frame update
    void Start() {
        if (isClient) {
            localCard = Instantiate(localCardPrefab);
        }
    }

    void OnDestroy() {
        if (isClient) {
            Destroy(localCard);
        }
    }

    // Update is called once per frame
    void Update() {
        if (isClient) {
            localCard.transform.position = transform.position;
            localCard.transform.rotation = transform.rotation;
            localCard.transform.localScale = transform.localScale;
        }
    }
}
