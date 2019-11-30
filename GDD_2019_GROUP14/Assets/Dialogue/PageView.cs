using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageView : MonoBehaviour {
    [SerializeField] private Text header;
    [SerializeField] private Text body;

    public void SetHeader(string str) {
        if (header == null) {
            Debug.LogError("Page is missing a header component");
        }

        header.text = str;
    }

    public void SetBody(string str) {
        if (body == null) {
            Debug.LogError("Page is missing a body component");
        }

        body.text = str;
    }
}
