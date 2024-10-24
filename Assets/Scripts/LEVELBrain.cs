using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMEBrain : MonoBehaviour
{
    public GameObject death;
    public GameObject player;
    public FADERHandler fader;
    public SCENEManager scene;

    bool playerDie = false;

    void Start()
    {
        Debug.Assert(death != null);
        Debug.Assert(player != null);
        Debug.Assert(fader != null);
        Debug.Assert(scene != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y >= death.transform.position.y)
        {
            playerDie = true;
        }

        if (playerDie)
        {
            fader.FadeOut();
        }

        if (fader.FadeDone() && playerDie)
        {
            scene.Restart();
        }
    }
}
