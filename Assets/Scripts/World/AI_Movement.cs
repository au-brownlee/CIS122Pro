using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{

    Animator animator;

    public float moveSpeed = 0.2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;

    int WalkDirection;

    public bool isWalking;

    public bool isDead = false;
    public float DecayTime = 2;
    internal float DeathTimer = 0;
    public string LootName = "???";
    public float LootMaxScore;
    public float LootHeigh = 0;

    void Start()
    {
        animator = GetComponent<Animator>();

        //So that all the prefabs don't move/stop at the same time
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);


        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
    }

    void Update()
    {
        // wait for DecayTime, then spawn loot and destroy creature
        if (isDead)
        {
            DeathTimer += Time.deltaTime;
            if (DeathTimer >= DecayTime)
            {
                if (LootName != "???")
                {
                    InventorySystem.Instance.SpawnNewObject(LootName, 
                        LootMaxScore, LootMaxScore, gameObject, new Vector3(0, LootHeigh, 0));

                }
                Destroy(gameObject);
            }
        }
        // if walking go to selected direction
        else if (isWalking)
        {
            animator.SetBool("isRunning", true);

            walkCounter -= Time.deltaTime;

            switch (WalkDirection)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0f, 90, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0f, -90, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0f, 180, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }

            if (walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;
                //stop movement
                transform.position = stopPosition;
                animator.SetBool("isRunning", false);
                //reset the waitCounter
                waitCounter = waitTime;
            }
        }
        else // wait and choose new direction
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);

        isWalking = true;
        walkCounter = walkTime;
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
    }
}