using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CAMERAController : MonoBehaviour
{
    public GameObject followObject;

    public GameObject startBound;
    public GameObject startYBound;
    public GameObject endBound;
    public GameObject endYBound;
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
            if (startYBound != null && endYBound != null)
            {
                if (newPos.y <= startYBound.transform.position.y)
                {
                    newPos.y = startYBound.transform.position.y;
                }
                if (newPos.y >= endYBound.transform.position.y)
                {
                    newPos.y = endYBound.transform.position.y;
                }
            }
            else
            {
                newPos.y = transform.position.y;
            }
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }
}
