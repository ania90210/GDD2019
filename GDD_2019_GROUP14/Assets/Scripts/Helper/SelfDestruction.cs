using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    public void SelfDestroyIn(float seconds) {
        StartCoroutine(IESelfDestroy(seconds));
    }
    IEnumerator IESelfDestroy(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
        yield return null;
    }
}
