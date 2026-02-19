using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    //Rigidbody rb;
    public float BPM;
    public float additionSpeed = 0;

    MeshRenderer mr;
    Color old;
    // bool isStarting = false;

    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        old = mr.material.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        BPM = (BPM / 60f) + additionSpeed;
        //rb.velocity = new Vector3(0, -BPM, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position -= new Vector3(0, BPM * Time.deltaTime, 0);

        if (GameManagerBassHero.Instance.isPlayerInStarMode)
        {
            mr.material.color = new Color(0, 255, 255);
        }
        else if (!GameManagerBassHero.Instance.isPlayerInStarMode)
        {
            mr.material.color = old;
        }
    }
}
