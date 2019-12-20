using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{

    // drop a light into variable 
    public UnityEngine.Experimental.Rendering.LWRP.Light2D flashLight = null;
    MovementScript playerOriented;

    // boxcollider2d trigger
    BoxCollider2D destructionArea;
    // initial size and offset of destruction area, which is set for facing to the right
    float tmpOffx;
    float tmpOffy;
    float xSize;
    float ySize;


    // get UI
    public Transform batteryUI;
    Color batteryColor;
    // flash light reload, time it can stay on, seconds
    public float maxBattery = 1.0f;
    public float reduceRecover = 2.0f; // by how much you reduce the recovery
    float dangerZone;
    float battery;
    bool recoveryMode = false;

    // Start is called before the first frame update
    void Start()
    {
        flashLight.enabled = false;
        playerOriented = GetComponent<MovementScript>();
        destructionArea = GetComponents<BoxCollider2D>()[1]; // take the second collider

        // save components into tmp variables
        tmpOffx = destructionArea.offset.x;
        tmpOffy = destructionArea.offset.y;
        xSize = destructionArea.size.x;
        ySize = destructionArea.size.y;

        // set battery to amx battery
        battery = maxBattery;
        dangerZone = maxBattery * 0.3f;
        batteryUI = batteryUI.Find("Bar");
        batteryColor = batteryUI.Find("BarSprite").GetComponent<SpriteRenderer>().color;
        SetUpRotation();
        StartCoroutine(changeColorBattery());
    }

    void SetUpRotation()
    {
        float degree = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(playerOriented.orientation, Vector3.right));
        if (playerOriented.orientation.y == -1)
        {
            degree *= -1;
        }
        degree -= 90;
        flashLight.transform.eulerAngles = new Vector3(0, 0, degree);
    }

    void SetUpCollider()
    {
        if (playerOriented.orientation.x != 0)
        {
            destructionArea.offset = new Vector2(playerOriented.orientation.x * tmpOffx, tmpOffy);
            destructionArea.size = new Vector2(xSize, ySize);
        }
        else if (playerOriented.orientation.y != 0)
        {
            destructionArea.offset = new Vector2(tmpOffy, playerOriented.orientation.y * tmpOffx);
            destructionArea.size = new Vector2(ySize, xSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (flashLight.enabled)
        {
            SetUpRotation();
            SetUpCollider();
        }
        BatteryRefresher();
        if (Input.GetButtonDown("Fire1") && !recoveryMode)
        {
            flashLight.enabled = true ^ flashLight.enabled;
        }
    }

    void BatteryRefresher()
    {
        // make sure it is always between 0 and maxBattery
        // if flashlight turned on -> lower battery level
        if (flashLight.enabled && battery > 0)
        {
            battery -= Time.deltaTime;
        }

        // if flashlight turned off -> increase battery level
        if (recoveryMode)
        {
            battery += Time.deltaTime / reduceRecover;
        }

        if (battery <= 0)
        {
            flashLight.enabled = false;
            recoveryMode = true;
        }

        if (battery >= maxBattery)
        {
            recoveryMode = false;
        }
        batteryUI.localScale = new Vector3(BatteryPercent(), 1, 1);
    }

    float BatteryPercent()
    {
        float progress = Mathf.Clamp01(battery / maxBattery);
        return progress;
    }

    IEnumerator changeColorBattery()
    {
        for (; ; )
        {
            if (battery <= dangerZone && !recoveryMode)
            {
                batteryUI.Find("BarSprite").GetComponent<SpriteRenderer>().color =
                    batteryColor == batteryUI.Find("BarSprite").GetComponent<SpriteRenderer>().color ? Color.white : batteryColor;
            }
            if (recoveryMode)
            {
                batteryUI.Find("BarSprite").GetComponent<SpriteRenderer>().color = batteryColor;
            }
            yield return new WaitForSeconds(0.22f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && flashLight.enabled)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && flashLight.enabled)
        {
            Destroy(collision.gameObject);
        }
    }
}
