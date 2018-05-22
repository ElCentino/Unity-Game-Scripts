using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerInstructions : MonoBehaviour {

    private Transform powerup;
    private Rigidbody rig;
    Vector3 relPos;

    public float speed = 3f;

    void Start()
    {
        powerup = GameObject.FindGameObjectWithTag(Tags.powerUP).transform;
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        relPos = powerup.position - transform.position;

        Quaternion pos = Quaternion.LookRotation(-relPos);
        Quaternion newPos = Quaternion.Lerp(rig.rotation, pos, speed * Time.deltaTime);
        rig.MoveRotation(newPos);
    }
}