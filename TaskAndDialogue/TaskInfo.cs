using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskInfo : MonoBehaviour
{
    public Task infoTask;
    public TextMeshProUGUI description;
    public TextMeshProUGUI condition;
    public GameObject completeBackGround;
    public void UpdateCondition()
    {
        condition.text = $"{infoTask.count} / {infoTask.needAmount}";
    }
}

