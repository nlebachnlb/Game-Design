using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : BaseCollectible
{
    public bool isCollected;

    private Collider2D col;

    public GameObject position;

    public override void OnCollected(PlayerController player)
    {
        this.isCollected = true;
        col.enabled = false;
        this.transform.position = position.transform.position;
        visual.gameObject.transform.localPosition = Vector2.zero;
        visual.enabled = false;
        light.SetActive(false);
        particle.SetActive(false);
    }

    private void FixedUpdate()
    {
    }

    private void Awake()
    {
        this.isCollected = false; 
        col = GetComponent<Collider2D>();
        visual = GetComponentInChildren<FloatAnimation>();
    }

    private FloatAnimation visual;

    [SerializeField]
    private GameObject light, particle;
}
