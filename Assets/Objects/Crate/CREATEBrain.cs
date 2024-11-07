using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CREATEBrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (COLINControl.Verify(col.gameObject))
        {
            Debug.Log("COL!");
            if (COLINControl.Charging(col.gameObject))
            {
                Debug.Log("COL GOOD!");
                Destroy(gameObject);
                COLINControl.GetInstance(col.gameObject).Knock();
            }
        }
    }
}
