using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour{

    public GameObject panel;
    public Button nextPage;
    public Button prevPage;

    public void OpenPanel() {
        if (panel != null) {

            Animator animator = panel.GetComponent<Animator>();
            if (animator != null) {
                bool isOpen = animator.GetBool("open");

                animator.SetBool("open", !isOpen);
            }
        }
    }
}
