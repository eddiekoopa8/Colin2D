using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERAController : MonoBehaviour
{
    public GameObject followObject;

    public GameObject startBound;
    public GameObject endBound;
    public GameObject skybox;

    private Vector3 followPos;
    void Start()
    {
        followPos = followObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        followPos = followObject.transform.position;

        if (followObject)
        {
            Vector3 newPos = followPos;
            if (newPos.x <= startBound.transform.position.x)
            {
                newPos.x = startBound.transform.position.x;
            }
            if (newPos.x >= endBound.transform.position.x)
            {
                newPos.x = endBound.transform.position.x;
            }
            newPos.z = transform.position.z;
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }
}
