using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnimationScript : MonoBehaviour
{
    public Animator animator;

    public bool playerIsClose = false;

    public float range = 5f;


    public Transform Player;

    void Update()
    {
        float distance = Vector3.Distance(Player.position, transform.position);

        if (distance < range)
        {
            playerIsClose = true;
        }
        else
        {
            playerIsClose = false;
        }

        animator.SetBool("PlayerClose", playerIsClose);
    }
}
