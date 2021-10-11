using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    public int triggerNo;
    public Dialogue triggerDialogue;
    public float waitTime;
    public bool isActive;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerManager.instance.playerCombatMotion.isAiming)
            {
                return;
            }
            if (isActive)
            {
                if (waitTime != 0)
                {
                    Invoke("TriggerDialogue", waitTime);
                }
                else
                {
                    TriggerDialogue();
                }
            }
        }
    }

    private void TriggerDialogue()
    {
        Debug.Log("Start Dialogue");
        DialogueManager.instance.StartDialogue(triggerDialogue);
        gameObject.SetActive(false);
    }

    public void ActiveTaskTrigger()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

}
