using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPIKE : MonoBehaviour
{
    public void Start()
    {
        
    }

    public void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (COLINControl.Verify(col.gameObject))
        {
            Debug.Log("COL!");
            COLINControl.GetInstance(col.gameObject).Die();
        }
    }
}
