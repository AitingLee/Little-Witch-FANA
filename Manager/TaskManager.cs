using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    private static TaskManager _instance;
    public static TaskManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public TaskInfo[] taskInfos;

    public List<Task> allTasks;
    List<Task> currentTasks;
    public List<TaskTrigger> allTriggers;
    public List<Indicator> lightIndicators;
    public List<Indicator> mapIndicators;

    public Villager playerVillager;
    public Villager[] allVillagers;

    public GameObject endCanvas;


    public bool bossBattleActived, inBossBattle;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        playerVillager = PlayerManager.instance.gameObject.GetComponent<Villager>();
        allTasks = new List<Task>();
        currentTasks = new List<Task>();
        InitAllTasks();
        InitAllTrigger();
    }

    private void Update()
    {
        CheckCollectionCondition();
    }

    public void InitAllTasks()
    {
        Task task_0 = new Task(0, TaskType.Move, CharacterName.Fana, CharacterName.Fana);
        task_0.description = "移動至光圈位置";
        playerVillager.dialogues[0].triggerTask = task_0;
        playerVillager.dialogues[2].finishTask = task_0;
        playerVillager.dialogues[0].clickAccept = false;
        task_0.indicatorExecuting = lightIndicators[0];
        task_0.activeTriggerWhenStart = allTriggers[1];
        task_0.activeTasksWhenFinish.Add(1);
        allTasks.Add(task_0);
        //移動至光圈時直接觸發下一段對話

        Task task_1 = new Task(1, TaskType.Move, CharacterName.Fana, CharacterName.Fana);
        task_1.description = "移動至光圈位置";
        playerVillager.dialogues[2].triggerTask = task_1;
        playerVillager.dialogues[2].clickAccept = false;
        playerVillager.dialogues[3].finishTask = task_1;
        task_1.indicatorExecuting = lightIndicators[1];
        task_1.activeTriggerWhenStart = allTriggers[2];
        task_1.activeTasksWhenFinish.Add(2);
        allTasks.Add(task_1);
        //移動至光圈時直接觸發下一段對話

        Task task_2 = new Task(2, TaskType.Fight, CharacterName.Fana, CharacterName.Fana);
        task_2.description = "打敗扶桑花";
        playerVillager.dialogues[4].triggerTask = task_2;
        playerVillager.dialogues[4].clickAccept = false;
        task_2.needAmount = 1;
        task_2.targetMobs.Add(MobType.flower);
        task_2.activeTriggerWhenFinish = allTriggers[4];
        task_2.activeTasksWhenFinish.Add(3);
        allTasks.Add(task_2);
        //打敗扶桑花開啟下一段對話

        Task task_3 = new Task(3, TaskType.Move, CharacterName.Fana, CharacterName.Fana);
        task_3.description = "調查元素圖騰";
        playerVillager.dialogues[5].triggerTask = task_3;
        playerVillager.dialogues[5].clickAccept = false;
        task_3.indicatorExecuting = lightIndicators[2];
        task_3.activeTriggerWhenFinish = allTriggers[5];
        task_3.activeTasksWhenFinish.Add(4);
        allTasks.Add(task_3);
        //獲取土元素開啟下一段任務


        Task task_4 = new Task(4, TaskType.Move, CharacterName.Fana, CharacterName.Fana);
        task_4.description = "傳送至村莊";
        playerVillager.dialogues[6].triggerTask = task_4;
        playerVillager.dialogues[8].finishTask = task_4;
        playerVillager.dialogues[6].clickAccept = false;
        task_4.indicatorExecuting = lightIndicators[3];
        task_4.activeTriggerWhenStart = allTriggers[6];
        task_4.activeTasksWhenFinish.Add(5);
        allTasks.Add(task_4);
        //觸碰村莊傳送鎮旁的觸發器開始下一任務

        Task task_5 = new Task(5, TaskType.Move, CharacterName.Fana, CharacterName.Fefe);
        task_5.description = "與村長對話";
        playerVillager.dialogues[8].triggerTask = task_5;
        playerVillager.dialogues[8].clickAccept = false;
        allVillagers[0].dialogues[1].finishTask = task_5;  //0號村民 - 村長 的對話 1會完成任務5
        task_5.indicatorExecuting = lightIndicators[4];
        task_5.indicatorCanReply = allVillagers[0].tick;
        task_5.activeTasksWhenFinish.Add(6);
        task_5.activeTasksWhenFinish.Add(7);
        task_5.activeTasksWhenFinish.Add(8);
        task_5.activeTasksWhenFinish.Add(10);
        task_5.activeTasksWhenFinish.Add(12);
        allTasks.Add(task_5);
        //跟村長接取下一任務

        Task task_6 = new Task(6, TaskType.Collection, CharacterName.Fefe, CharacterName.Fefe);
        task_6.description = "獲取雞蛋";
        task_6.needAmount = 8;
        allVillagers[0].dialogues[4].triggerTask = task_6;  //0號村民 - 村長 的對話 4 可接取任務6
        allVillagers[0].dialogues[4].clickAccept = true;
        task_6.indicatorAcceptable = allVillagers[0].exclamation;
        task_6.indicatorExecuting = mapIndicators[0];
        task_6.indicatorCanReply = allVillagers[0].tick;
        task_6.giverGivingDialogue = allVillagers[0].dialogues[4];  //接任務 0號村民 - 村長 的對話 4
        task_6.giverExecutingDialogue = allVillagers[0].dialogues[5];  //執行任務 0號村民 - 村長 的對話 5
        task_6.replierReplyDialogue = allVillagers[0].dialogues[8];  //回任務 0號村民 - 村長 的對話 8
        allVillagers[0].dialogues[8].finishTask = task_6;
        task_6.targetItem = ItemName.egg;
        task_6.reward = new ItemInformation (ItemName.roastChicken, ItemManager.instance.GetItemSO(ItemName.roastChicken), 10);
        allTasks.Add(task_6);

        Task task_7 = new Task(7, TaskType.Fight, CharacterName.Bae, CharacterName.Bae);
        task_7.description = "打敗蜘蛛";
        task_7.needAmount = 5;
        allVillagers[1].dialogues[2].triggerTask = task_7;  //1號村民 - 貝拉 的對話 2 可接取任務
        allVillagers[1].dialogues[2].clickAccept = true;
        task_7.indicatorAcceptable = allVillagers[1].exclamation;
        task_7.indicatorExecuting = mapIndicators[1];
        task_7.indicatorCanReply = allVillagers[1].tick;
        task_7.giverGivingDialogue = allVillagers[1].dialogues[2];  //接任務  1號村民 - 貝拉 的對話 2
        task_7.giverExecutingDialogue = allVillagers[1].dialogues[3];  //執行任務  1號村民 - 貝拉 的對話 3
        task_7.replierReplyDialogue = allVillagers[1].dialogues[4];  //回任務 1號村民 - 貝拉 的對話 4
        allVillagers[1].dialogues[4].finishTask = task_7;
        task_7.targetMobs.Add(MobType.spider);
        task_7.targetMobs.Add(MobType.spiderDark);
        task_7.targetMobs.Add(MobType.spiderToxin);
        task_7.reward = new ItemInformation(ItemName.silverRing, ItemManager.instance.GetItemSO(ItemName.silverRing), 1);
        allTasks.Add(task_7);

        Task task_8 = new Task(8, TaskType.Collection, CharacterName.Mary, CharacterName.Mary);
        task_8.description = "獲取蜂蜜";
        task_8.needAmount = 5;
        allVillagers[2].dialogues[1].triggerTask = task_8;  //2號村民 - 瑪莉 的對話 1 可接取任務
        allVillagers[2].dialogues[1].clickAccept = true;
        task_8.indicatorAcceptable = allVillagers[2].exclamation;
        task_8.indicatorExecuting = mapIndicators[2];
        task_8.indicatorCanReply = allVillagers[2].tick;
        task_8.giverGivingDialogue = allVillagers[2].dialogues[1];  //接任務 2號村民 - 瑪莉 的對話 1
        task_8.giverExecutingDialogue = allVillagers[2].dialogues[2];  //執行任務  2號村民 - 瑪莉 的對話 2
        task_8.replierReplyDialogue = allVillagers[2].dialogues[3];  //回任務 2號村民 - 瑪莉 的對話 3
        allVillagers[2].dialogues[3].finishTask = task_8;
        task_8.activeTasksWhenFinish.Add(9);
        task_8.targetItem = ItemName.honey;
        task_8.reward = new ItemInformation(ItemName.moonStone, ItemManager.instance.GetItemSO(ItemName.moonStone), 1);
        allTasks.Add(task_8);

        Task task_9 = new Task(9, TaskType.Collection, CharacterName.Mary, CharacterName.Mary);
        task_9.description = "獲取蜂蜜";
        task_9.repeatable = true;
        task_9.needAmount = 15;
        allVillagers[2].dialogues[4].triggerTask = task_9;  //2號村民 - 瑪莉 的對話 4 可接取任務
        allVillagers[2].dialogues[4].clickAccept = true;
        task_9.indicatorAcceptable = allVillagers[2].repeat;
        task_9.indicatorExecuting = mapIndicators[2];
        task_9.indicatorCanReply = allVillagers[2].tick;
        task_9.giverGivingDialogue = allVillagers[2].dialogues[4];  //接任務 2號村民 - 瑪莉 的對話 4
        task_9.giverExecutingDialogue = allVillagers[2].dialogues[5];  //執行任務  2號村民 - 瑪莉 的對話 5
        task_9.replierReplyDialogue = allVillagers[2].dialogues[6];  //回任務 2號村民 - 瑪莉 的對話 6
        allVillagers[2].dialogues[6].finishTask = task_9;
        task_9.reward = new ItemInformation(ItemName.moonStone, ItemManager.instance.GetItemSO(ItemName.moonStone), 1);
        task_9.targetItem = ItemName.honey;
        task_9.activeTasksWhenFinish.Add(9);
        allTasks.Add(task_9);

        Task task_10 = new Task(10, TaskType.Fight, CharacterName.Rae, CharacterName.Rae);
        task_10.description = "打敗蝙蝠領主";
        task_10.needAmount = 1;
        allVillagers[3].dialogues[1].triggerTask = task_10;  //3號村民 - 蕾蕾 的對話 1 可接取任務
        allVillagers[3].dialogues[1].clickAccept = true;
        task_10.indicatorAcceptable = allVillagers[3].exclamation;
        task_10.indicatorExecuting = mapIndicators[3];
        task_10.indicatorCanReply = allVillagers[3].tick;
        task_10.giverGivingDialogue = allVillagers[3].dialogues[1];  //接任務 3號村民 - 蕾蕾 的對話 1
        task_10.giverExecutingDialogue = allVillagers[3].dialogues[2];  //執行任務  3號村民 - 蕾蕾 的對話 2
        task_10.replierReplyDialogue = allVillagers[3].dialogues[3];  //回任務 3號村民 - 蕾蕾 的對話 3
        allVillagers[3].dialogues[3].finishTask = task_10;
        task_10.activeTasksWhenFinish.Add(11);
        //task_10.activeTasksWhenFinish.Add(12);
        task_10.targetMobs.Add(MobType.batLord);
        task_10.reward = new ItemInformation(ItemName.amethyst, ItemManager.instance.GetItemSO(ItemName.amethyst), 1);
        allTasks.Add(task_10);

        Task task_11 = new Task(11, TaskType.Fight, CharacterName.Rae, CharacterName.Rae);
        task_11.description = "打敗蝙蝠";
        task_11.repeatable = true;
        task_11.needAmount = 20;
        allVillagers[3].dialogues[4].triggerTask = task_11;  //3號村民 - 蕾蕾 的對話 4 可接取任務
        allVillagers[3].dialogues[4].clickAccept = true;
        task_11.indicatorAcceptable = allVillagers[3].repeat;
        task_11.indicatorExecuting = mapIndicators[3];
        task_11.indicatorCanReply = allVillagers[3].tick;
        task_11.giverGivingDialogue = allVillagers[3].dialogues[4];  //接任務 3號村民 - 蕾蕾 的對話 4
        task_11.giverExecutingDialogue = allVillagers[3].dialogues[5];  //執行任務  3號村民 - 蕾蕾 的對話 5
        task_11.replierReplyDialogue = allVillagers[3].dialogues[6];  //回任務3號村民 - 蕾蕾 的對話 6
        allVillagers[3].dialogues[6].finishTask = task_11;
        task_11.reward = new ItemInformation(ItemName.amethyst, ItemManager.instance.GetItemSO(ItemName.amethyst), 1);
        task_11.targetMobs.Add(MobType.bat);
        task_11.targetMobs.Add(MobType.vampire);
        task_11.targetMobs.Add(MobType.batLord);
        task_11.activeTasksWhenFinish.Add(11);
        allTasks.Add(task_11);

        Task task_12 = new Task(12, TaskType.Fight, CharacterName.Capay, CharacterName.Capay);
        task_12.description = "調查雪山";
        task_12.needAmount = 1;
        allVillagers[4].dialogues[1].triggerTask = task_12;  //4號村民 - 卡佩 的對話 1 可接取任務
        allVillagers[4].dialogues[1].clickAccept = true;
        task_12.indicatorAcceptable = allVillagers[4].exclamation;
        task_12.indicatorExecuting = mapIndicators[4];
        task_12.indicatorCanReply = allVillagers[4].tick;
        task_12.giverGivingDialogue = allVillagers[4].dialogues[1];  //接任務 4號村民 - 卡佩 的對話 1
        task_12.giverExecutingDialogue = allVillagers[4].dialogues[2];  //執行任務 4號村民 - 卡佩 的對話 2
        task_12.replierReplyDialogue = allVillagers[4].dialogues[3];  //回任務 4號村民 - 卡佩 的對話 3
        allVillagers[4].dialogues[3].finishTask = task_12;
        task_12.activeTasksWhenFinish.Add(13);
        task_12.activeTasksWhenFinish.Add(14);
        task_12.targetMobs.Add(MobType.phantom);
        task_12.reward = new ItemInformation(ItemName.cryolite, ItemManager.instance.GetItemSO(ItemName.cryolite), 1);
        allTasks.Add(task_12);

        Task task_13 = new Task(13, TaskType.Fight, CharacterName.Capay, CharacterName.Capay);
        task_13.description = "打敗幽靈";
        task_13.repeatable = true;
        task_13.needAmount = 20;
        allVillagers[4].dialogues[4].triggerTask = task_13;  //4號村民 - 卡佩 的對話 4 可接取任務
        allVillagers[4].dialogues[4].clickAccept = true;
        task_13.indicatorAcceptable = allVillagers[4].repeat;
        task_13.indicatorExecuting = mapIndicators[4];
        task_13.indicatorCanReply = allVillagers[4].tick;
        task_13.giverGivingDialogue = allVillagers[4].dialogues[4];  //接任務 4號村民 - 卡佩 的對話 4
        task_13.giverExecutingDialogue = allVillagers[4].dialogues[5];  //執行任務  4號村民 - 卡佩 的對話 5
        task_13.replierReplyDialogue = allVillagers[4].dialogues[6];  //回任務 4號村民 - 卡佩 的對話 6
        allVillagers[4].dialogues[6].finishTask = task_13;
        task_13.targetMobs.Add(MobType.spooky);
        task_13.targetMobs.Add(MobType.ghost);
        task_13.targetMobs.Add(MobType.phantom);
        task_13.reward = new ItemInformation(ItemName.cryolite, ItemManager.instance.GetItemSO(ItemName.cryolite), 1);
        allTasks.Add(task_13);

        Task task_14 = new Task(14, TaskType.Fight, CharacterName.Fefe, CharacterName.Fana);
        task_14.description = "戰勝火龍";
        task_14.needAmount = 1;
        allVillagers[0].dialogues[6].triggerTask = task_14;  //0號村民 - 菲菲 的對話 6 可接取任務
        allVillagers[0].dialogues[6].clickAccept = false;
        task_14.indicatorAcceptable = allVillagers[0].exclamation;
        task_14.indicatorExecuting = mapIndicators[5];
        task_14.giverGivingDialogue = allVillagers[0].dialogues[6];  //接任務 0號村民 - 菲菲 的對話 6
        task_14.giverExecutingDialogue = allVillagers[0].dialogues[7];  //執行任務  0號村民 - 菲菲 的對話 7
        task_14.replierReplyDialogue = playerVillager.dialogues[11];  //回任務 主角 的對話 11
        playerVillager.dialogues[11].finishTask = task_14;
        task_14.targetMobs.Add(MobType.dragon);
        allTasks.Add(task_14);
    }

    public void InitAllTrigger()
    {

        //觸發任務0 找村莊
        allTriggers[0].triggerDialogue = playerVillager.dialogues[0];

        //觸發任務1 戰鬥教學 移動至中段
        allTriggers[1].triggerDialogue = playerVillager.dialogues[2];

        //迴光石教學
        allTriggers[2].triggerDialogue = playerVillager.dialogues[3];

        //觸發任務2 打花花
        allTriggers[3].triggerDialogue = playerVillager.dialogues[4];

        //觸發任務3 打敗花花啟動 找圖騰
        allTriggers[4].triggerDialogue = playerVillager.dialogues[5];

        //觸發任務4 獲取土圖騰後啟動
        allTriggers[5].triggerDialogue = playerVillager.dialogues[6];

        //任務4開始時啟動 傳送教學
        allTriggers[6].triggerDialogue = playerVillager.dialogues[7];

        //任務4開始時啟動 完成傳送 完成任務4
        allTriggers[7].triggerDialogue = playerVillager.dialogues[8];

    }

    public void CanAcceptTask(Task task)
    {
        task.acceptable = true;
        if (task.giverGivingDialogue != null)
        {
            foreach (Villager villager in allVillagers)
            {
                if (villager.characterName == task.giver)
                {
                    villager.currentDialogue = task.giverGivingDialogue;
                }
            }
        }

        if (task.indicatorAcceptable != null)
        {
            task.indicatorAcceptable.TurnOn();
        }

    }


    public void StartExecuteTask(Task task)
    {
        currentTasks.Add(task);

        foreach (Villager villager in allVillagers)
        {
            if (villager.characterName == task.giver)
            {
                villager.currentDialogue = task.giverExecutingDialogue;
            }
        }

        if (task.activeTriggerWhenStart != null)
        {
            task.activeTriggerWhenStart.ActiveTaskTrigger();
        }

        if (task.indicatorAcceptable != null)
        {
            task.indicatorAcceptable.TurnOff();
        }
        if (task.indicatorExecuting != null)
        {
            task.indicatorExecuting.TurnOn();
        }

        for (int i = 0; i < taskInfos.Length; i++)
        {
            if (!taskInfos[i].gameObject.activeSelf)
            {
                SetTaskInfo(i, task);
                break;
            }
        }

        if (task.taskNo == 14)
        {
            ActiveBossBattle();
        }
    }

    public void AddKilledMob(MobAI mob)
    {
        if (currentTasks.Count > 0)
        {
            for (int i = 0; i < currentTasks.Count; i++)
            {
                foreach (MobType type in currentTasks[i].targetMobs)
                {
                    if (type == mob.mobtype)
                    {
                        if (currentTasks[i].count < currentTasks[i].needAmount)
                        {
                            currentTasks[i].count++;
                            taskInfos[i].UpdateCondition();
                        }

                        if (currentTasks[i].count >= currentTasks[i].needAmount)
                        {
                            CanReplyTask(currentTasks[i]);
                        }
                    }
                }
            }
        }
    }

    private void CheckCollectionCondition()
    {
        if (currentTasks.Count > 0)
        {
            for (int i = 0; i < currentTasks.Count; i++)
            {
                if (currentTasks[i].taskType == TaskType.Collection)
                {
                    CountItemNum(currentTasks[i]);
                    taskInfos[i].UpdateCondition();
                }
            }
        }
    }
    public void CountItemNum(Task task)
    {
        foreach (Slot slot in CanvasManager.instance.inventory.slots)
        {
            ItemInformation itemInfo = slot.storeItem.itemInformation;
            if (itemInfo.itemName == task.targetItem)
            {
                task.count = itemInfo.itemNumber;

                if (task.count >= task.needAmount)
                {
                    if (!task.canReply)
                    {
                        CanReplyTask(task);
                    }
                }
                else
                {
                    if (task.canReply)
                    {
                        DisableCanReplyTask(task);
                    }
                }
            }
        }
    }


    public void SetTaskInfo(int infoNo, Task task)
    {
        taskInfos[infoNo].infoTask = task;
        taskInfos[infoNo].description.text = task.description;
        taskInfos[infoNo].gameObject.SetActive(true);

        if (task.needAmount != 0)
        {
            taskInfos[infoNo].condition.text = $"{task.count} / {task.needAmount}";
            taskInfos[infoNo].condition.gameObject.SetActive(true);
        }
        else
        {
            taskInfos[infoNo].condition.gameObject.SetActive(false);
        }

        if (task.canReply)
        {
            taskInfos[infoNo].completeBackGround.SetActive(true);
        }
        else
        {
            taskInfos[infoNo].completeBackGround.SetActive(false);
        }
    }


    public void CanReplyTask(Task task)
    {
        task.canReply = true;
        if (task.replier == CharacterName.Fana)
        {
            if (task.taskNo == 14)
            {
                DialogueManager.instance.StartDialogue(task.replierReplyDialogue);
                return;
            }
            FinishTask(task);
        }

        foreach (Villager villager in allVillagers)
        {
            if (villager.characterName == task.replier)
            {
                villager.currentDialogue = task.replierReplyDialogue;
            }
            else if (villager.characterName == task.giver)
            {
                villager.currentDialogue = villager.dialogues[0];
            }
        }

        if (task.indicatorExecuting != null)
        {
            task.indicatorExecuting.TurnOff();
        }
        if (task.indicatorCanReply != null)
        {
            task.indicatorCanReply.TurnOn();
        }

        for (int i = 0; i < currentTasks.Count; i++)
        {
            if (taskInfos[i].infoTask.taskNo == task.taskNo)
            {
                taskInfos[i].completeBackGround.SetActive(true);
            }
        }

    }

    public void DisableCanReplyTask(Task task)
    {
        task.canReply = false;

        foreach (Villager villager in allVillagers)
        {
            if (villager.characterName == task.giver)
            {
                villager.currentDialogue = task.giverExecutingDialogue;
            }
            else if (villager.characterName == task.replier)
            {
                villager.currentDialogue = villager.dialogues[0];
            }


        }

        if (task.indicatorExecuting != null)
        {
            task.indicatorExecuting.TurnOn();
        }
        if (task.indicatorCanReply != null)
        {
            task.indicatorCanReply.TurnOff();
        }

        for (int i = 0; i < currentTasks.Count; i++)
        {
            if (taskInfos[i].infoTask.taskNo == task.taskNo)
            {
                taskInfos[i].completeBackGround.SetActive(false);
            }
        }
    }
    public bool HandInItem(Task task)
    {
        foreach (Slot slot in CanvasManager.instance.inventory.slots)
        {
            ItemInformation itemInfo = slot.storeItem.itemInformation;
            if (itemInfo.itemName == task.targetItem)
            {
                if (itemInfo.itemNumber >= task.needAmount)
                {
                    slot.storeItem.DecreaseAmount(task.needAmount);
                    return true;
                }
                else
                {
                    DisableCanReplyTask(task);
                }
            }
        }
        return false;
    }
    public void FinishTask(Task task)
    {
        Debug.Log($"Finishing Task {task.description}");
        if (!CheckCanGetReward(task))
        {
            Debug.Log("Finishing Task Get Reward Fail");
            return;
        }

        if (task.targetItem != ItemName.empty)
        {
            if (!HandInItem(task))
            {
                return;
            }
        }

        foreach (Villager villager in allVillagers)
        {
            if (villager.characterName == task.replier)
            {
                villager.currentDialogue = villager.dialogues[0];
            }
        }

        if (task.activeTasksWhenFinish.Count > 0)
        {
            foreach (int i in task.activeTasksWhenFinish)
            {
                CanAcceptTask(allTasks[i]);
            }
        }

        for (int i = 0; i < currentTasks.Count; i++)
        {
            if (taskInfos[i].infoTask.taskNo == task.taskNo)
            {
                if (i < currentTasks.Count -1)
                {
                    for (int j = i; j < currentTasks.Count- 1; j++)
                    {
                        SetTaskInfo(j, taskInfos[j + 1].infoTask);
                    }
                }
                Debug.Log($"Close Task Info {currentTasks.Count - 1} task {task.description}");
                taskInfos[currentTasks.Count-1].gameObject.SetActive(false);
                currentTasks.Remove(task);
            }
        }

        if (task.indicatorExecuting != null)
        {
            Debug.Log($"task.indicatorExecuting name {task.indicatorExecuting.name} turn off");
            task.indicatorExecuting.TurnOff();
        }

        if (task.indicatorCanReply != null)
        {
            Debug.Log($"task.indicatorExecuting name {task.indicatorExecuting.name} turn off");
            task.indicatorCanReply.TurnOff();
        }


        if (task.activeTriggerWhenFinish != null)
        {
            task.activeTriggerWhenFinish.ActiveTaskTrigger();
        }

        if (task.reward != null)
        {
            CanvasManager.instance.inventory.PutInBag(task.reward);
        }

        if (task.taskNo == 14)
        {
            FinishBossBattle();
        }
    }
    private bool CheckCanGetReward(Task task)
    {
        if (task.reward == null)
        {
            return true;
        }
        foreach (Slot slot in CanvasManager.instance.inventory.slots)
        {
            if (slot.storeItem.itemInformation.itemName == task.reward.itemName)
            {
                return true;
            }
            else if (slot.storeItem.itemInformation.itemName == ItemName.empty)
            {
                return true;
            }
        }
        return false;
    }

    //For Boss Battle
    public void ActiveBossBattle()
    {
        TeleportPlatform bossPortal = ItemManager.instance.teleportPlatforms[6];
        bossPortal.portalFX.SetActive(true);
        bossPortal.isActived = true;
        bossBattleActived = true;
    }

    public void StartBossBattle()
    {
        CanvasManager.instance.bossHealthBar.SetActive(true);
        inBossBattle = true;
    }

    public void StopBossBattle()
    {
        CanvasManager.instance.bossHealthBar.SetActive(false);
        inBossBattle = false;
    }

    public void FinishBossBattle()
    {
        Debug.Log("Finish Task");
        CanvasManager.instance.gameObject.SetActive(false);
        endCanvas.SetActive(true);
        Invoke("BackToMenu", 5f);
        StartCoroutine(AudioFade.FadeIn("OverVolume", 0.2f, Mathf.SmoothStep));
        StartCoroutine(AudioFade.FadeOut("BossVolume", 0.2f, Mathf.SmoothStep));
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
        StartCoroutine(AudioFade.FadeIn("MenuVolume", 1.5f, Mathf.SmoothStep));
        StartCoroutine(AudioFade.FadeOut("OverVolume", 1.5f, Mathf.SmoothStep));
    }

}
