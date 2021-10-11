using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    public GameObject noSkillText;
    public GameObject[] skillButtons;
    public GameObject[] skillInfos;

    public void OnLearnSkill(int index)
    {
        if (noSkillText.activeSelf)
        {
            noSkillText.SetActive(false);
        }

        skillButtons[index].SetActive(true);
        if (isFirstLearn())
        {
            Debug.Log($"infoindex {index} actived");
            skillInfos[index].SetActive(true);
        }
    }

    public void OnSkillButtonClick(int index)
    {
        for (int i = 0; i < skillInfos.Length; i++)
        {
            if (i == index)
            {
                skillInfos[i].SetActive(true);
            }
            else
            {
                skillInfos[i].SetActive(false);
            }
        }
    }

    public bool isFirstLearn()
    {
        foreach (GameObject buttom in skillButtons)
        {
            if (buttom.activeSelf)
            {
                Debug.Log("is first learn");
                return true;
            }
        }
        Debug.Log("not first learn");
        return false;
    }

}
