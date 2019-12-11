using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // speed
    public int speed = 8;

    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0f, 0f, 0f);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement[0] = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement[0] = 1f;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            movement[1] = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movement[1] = -1f;
        }
        this.transform.Translate(movement*Time.deltaTime*speed);

    }

}
