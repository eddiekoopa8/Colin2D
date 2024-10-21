using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using Unity.VisualScripting;

public class COLINControl : MonoBehaviour
{
    // rewriting platforming engine NIGHTMARE....
    Rigidbody2D body;
    Animator anim;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }
}
