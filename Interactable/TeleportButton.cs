using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public TeleportArea buttonArea;
    public TeleportPlatform[] teleportPlatforms;

    public void TeleportButtonOnClick()
    {
        foreach (TeleportPlatform tp in teleportPlatforms)
        {
            if (tp.platformArea == buttonArea)
            {
                tp.HideTeleportScreen();
                AudioManager.instance.teleportSound.Play();
                PlayerManager.instance.teleportTargetPoint = tp.transform.position;
                PlayerManager.instance.TeleportDisappear();
                if (TaskManager.instance.inBossBattle)
                {
                    TaskManager.instance.StopBossBattle();
                }
                if (tp.platformArea == TeleportArea.Area7)
                {
                    TaskManager.instance.StartBossBattle();
                }
            }
        }
    }
}
