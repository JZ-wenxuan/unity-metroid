﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirController : MonoBehaviour
{
    Rigidbody rigid;
    bool isRising = false;
    
    public GameObject jump;
    public GameObject Runjump;
    float roll_height = float.PositiveInfinity;
    public float rollHeightOffset = 1;
    public AudioClip jumpSE;
    GameObject hook;
    bool isRolling;
    void Awake()
    {
        rigid = this.GetComponentInParent<Rigidbody>();
        hook = PlayerState.instance.Hook;
    }

    private void OnEnable() {
        roll_height = float.PositiveInfinity;

        if (PlayerState.instance.isJumping) {
            isRising = true;
            if (PlayerState.instance.isRunning()) {
                roll_height = gameObject.transform.position.y + rollHeightOffset;
            }
            AudioSource.PlayClipAtPoint(jumpSE, transform.position);
        }
        isRolling = false;
        jump.SetActive(true);
        Runjump.SetActive(false);
        
    }
    void Update()
    {
        if (isRising && PlayerState.instance.isControlable() && Input.GetKeyUp(KeyCode.Z)) {
            Vector3 newVelocity = rigid.velocity;
            newVelocity.y = 0;
            rigid.velocity = newVelocity;
        }

        if (!isRising && PlayerState.instance.hasHook && PlayerState.instance.isControlable() && Input.GetKeyDown(KeyCode.Z)) {
            hook.SetActive(true);
            isRolling = false;
            roll_height = float.PositiveInfinity;
            jump.SetActive(true);
            Runjump.SetActive(false);
        }


        if (!isRolling) {
            if (gameObject.transform.position.y > roll_height) {
                isRolling = true;
                jump.SetActive(false);
                Runjump.SetActive(true);
            }
        } else {
            if (gameObject.transform.position.y < roll_height) {
                isRolling = false;
                roll_height = float.PositiveInfinity;
                jump.SetActive(true);
                Runjump.SetActive(false);
            }
        }
        if (rigid.velocity.y <= 0) isRising = false;
        if (!isRising && PlayerState.instance.isGrounded()) {
            PlayerState.instance.hitGround();
        }
    }

    
}
