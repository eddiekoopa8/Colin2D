using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts;

public class HUDManager : MonoBehaviour
{
    public TMP_Text health, healthS;
    public TMP_Text spark, sparkS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spark.text = GAMEManager.StarsCollected.ToString();
        sparkS.text = GAMEManager.StarsCollected.ToString();
    }
}
