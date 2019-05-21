using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleTagAlong : MonoBehaviour
{   
    public float Speed = 0.5f;

    private bool FollowPlayer = true;

    public void SetFollowPlayer(bool enabled)
    {
        FollowPlayer = enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowPlayer)
        {
            Vector3 headPosition = Camera.main.transform.position;

            headPosition.y = transform.position.y;

            Vector3 Destination = headPosition + Vector3.right;

            transform.position = Vector3.Lerp(transform.position, Destination, Speed * Time.deltaTime);
        }
        
    }
}
