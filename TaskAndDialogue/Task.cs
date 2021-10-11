using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType { Move, Collection, Fight}

public class Task
{
    public int taskNo;
    public TaskType taskType;
    public bool acceptable, executing, canReply, finished;
    public CharacterName giver, replier;

    public Dialogue giverGivingDialogue;
    public Dialogue giverExecutingDialogue;
    public Dialogue replierReplyDialogue;

    public string description;
    public Indicator indicatorAcceptable, indicatorExecuting, indicatorCanReply;
    public TaskTrigger activeTriggerWhenStart;
    public TaskTrigger activeTriggerWhenFinish;
    public List<int> activeTasksWhenFinish;

    public int count, needAmount;   // for colleciton and fight
    public bool repeatable; //for fight
    public Vector3 targetPosition;    //for move
    public List<MobType> targetMobs;
    public ItemName targetItem;

    public ItemInformation reward;

    public Task(int _taskNo, TaskType _taskType, CharacterName _giver, CharacterName _replier)
    {
        taskNo = _taskNo;
        taskType = _taskType;
        giver = _giver;
        replier = _replier;
        activeTasksWhenFinish = new List<int>();
        targetMobs = new List<MobType>();
    }

}
