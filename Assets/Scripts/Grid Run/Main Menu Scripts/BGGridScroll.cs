using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGGridScroll : MonoBehaviour
{
    public float scrollXPos;
    public float scrollYPos;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        meshRenderer.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup * scrollXPos, Time.realtimeSinceStartup * scrollYPos);
    }
}
