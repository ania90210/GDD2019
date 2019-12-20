using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // speed
<<<<<<< Updated upstream
    public float speed = 8;
=======
    public int speed = 6;
>>>>>>> Stashed changes
    public Vector3 orientation;
    // Start is called before the first frame update
    void Start()
    {
        this.orientation = Vector3.right; // start facing right
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0f, 0f, 0f);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement[0] = -1f;
            this.orientation = movement;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement[0] = 1f;
            this.orientation = movement;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            movement[1] = 1f;
            this.orientation = movement;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movement[1] = -1f;
            this.orientation = movement;

        }
        //this.orientation = movement; don t do this here, or will always update to 0 0 0 
        this.transform.Translate(movement * Time.deltaTime * speed);

    }

}
