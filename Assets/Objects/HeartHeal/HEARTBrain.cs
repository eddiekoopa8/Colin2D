using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEARTBrain : MonoBehaviour
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
            GameObject.Find("SPARKAudio").GetComponent<AudioSource>().Play();
            COLINControl.GetInstance(col.gameObject).Heal(10);
            Destroy(gameObject);
        }
    }
}
