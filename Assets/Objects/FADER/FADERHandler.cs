using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FADERHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer render;
    private float tickDelayWait = 20;
    private float tick = 0;

    bool fadeIn = false;
    bool fadeOut = false;

    void Start()
    {
        // Requires renderer
        render = GetComponent<SpriteRenderer>();
        Debug.Assert(render != null);

        // Make sure it is infront
        render.sortingOrder = -999999999;
        // If not enabled, enable it.
        render.enabled = true;

        // Make sure the fadeer covers the screen by setting the scale values to the maximum float value.
        Vector3 newScale = transform.localScale;
        newScale.x = 99999999999;
        newScale.y = 99999999999;
        transform.localScale = newScale;

        FadeIn();
    }

    public void FadeIn()
    {
        Debug.Log("FADERTask: in");

        fadeIn = true;
        fadeOut = false;
        Color newCol = render.color;
        newCol.a = 1;
        render.color = newCol;
    }

    public void FadeOut()
    {
        Debug.Log("FADERTask: out");
        fadeIn = false;
        fadeOut = true;
        Color newCol = render.color;
        newCol.a = 0;
        render.color = newCol;
    }

    public bool FadeDone()
    {
        if (fadeIn)
        {
            return render.color.a <= 0;
        }
        else if (fadeOut)
        {
            return render.color.a >= 1;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tick >= tickDelayWait) {
            if (fadeIn)
            {
                if (render.color.a >= 0)
                {
                    Color newCol = render.color;
                    newCol.a -= Time.deltaTime / 0.15f;
                    render.color = newCol;
                }
            }
            else if (fadeOut)
            {
                if (render.color.a <= 1)
                {
                    Color newCol = render.color;
                    newCol.a += Time.deltaTime / 0.15f;
                    render.color = newCol;
                }
            }
        }
        else
        {
            tick += 1;
        }
    }
}
