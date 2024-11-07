using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEVELBrain : MonoBehaviour
{
    public Transform next;
    public string nextLevel;
    public Transform death;
    public Transform player;
    public FADERHandler fader;
    public SCENEManager scene;

    bool playerDie = false;
    bool nextLevelGo = false;
    bool fadingOut = false;

    void Start()
    {
    }

    public void PlayerDied()
    {
        playerDie = true;
        fadingOut = false;
    }

    public static LEVELBrain GetInstance()
    {
        return GameObject.Find("LEVELBrain").GetComponent<LEVELBrain>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.position.y <= death.position.y)
        {
            playerDie = true;
        }

        if (player.position.x >= next.position.x)
        {
            nextLevelGo = true;
        }

        if ((playerDie || nextLevelGo) && !fadingOut)
        {
            fader.FadeOut();
            fadingOut = true;
        }

        if (fader.FadeDone() && playerDie && fadingOut)
        {
            scene.Restart();
        }

        if (fader.FadeDone() && nextLevelGo && fadingOut)
        {
            GAMEManager.PushStars();
            scene.ChangeScene(nextLevel);
        }
    }
}
