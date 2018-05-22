using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleProtector : MonoBehaviour {

    GameObject player;
    GameObject enemy;

    BoxCollider col;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        enemy = GameObject.FindGameObjectWithTag(Tags.enemy);
        col = GetComponent<BoxCollider>();
    }

    void Start()
    {
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            col.isTrigger = false;
        } 
        else if(other.gameObject == enemy)
        {
            col.isTrigger = true;
        }
    }
}