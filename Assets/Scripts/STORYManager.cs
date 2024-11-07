using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STORYManager : MonoBehaviour
{
    SCENEManager scene;
    FADERHandler fader;
    bool fading = false;
    // Start is called before the first frame update
    void Start()
    {
        fader = GameObject.Find("FADERObject").GetComponent<FADERHandler>();
        scene = GameObject.Find("SCENEManager").GetComponent<SCENEManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            fader.FadeOut();
            fading = true;
        }

        if (fading && fader.FadeDone())
        {
            scene.ChangeScene("Scenes/Level01");
        }
    }
}
