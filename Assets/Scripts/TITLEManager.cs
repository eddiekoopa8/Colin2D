using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TITLEManager : MonoBehaviour
{
    Image image;
    Color colour;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        colour = Color.white;
        colour.a = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        image.color = colour;

        if (colour.a < 0.18f)
        {
            colour.a += 0.002f;
        }
    }
}
