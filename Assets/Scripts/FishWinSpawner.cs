using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWinSpawner : MonoBehaviour
{


   


    public GameObject fish;
        private float timer = 0.0f;
        public float instantiationInterval = 0.1f;

        void Update()
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Check if the timer has reached the instantiation interval
            if (timer >= instantiationInterval)
            {
                // Instantiate the object
                Instantiate(fish, transform.position, transform.rotation);

                // Reset the timer
                timer = 0.0f;
            }
        }
    }


