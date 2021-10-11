using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabPanel : MonoBehaviour
{
    public Animator animator;
    public bool isOpen;
    public GameObject[] panels;
    public TextMeshProUGUI midPanelText;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Display()
    {
        animator.SetBool("IsOpen", true);
        isOpen = true;
    }

    public void Close()
    {
        animator.SetBool("IsOpen", false);
        isOpen = false;
    }

    public void ChangePanelOnClick(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == index)
            {
                panels[i].SetActive(true);
                switch (i)
                {
                    case 0:
                        midPanelText.text = "技能";
                        break;
                    case 1:
                        midPanelText.text = "裝備";
                        break;
                    case 2:
                        midPanelText.text = "背包";
                        break;
                    case 3:
                        midPanelText.text = "設定";
                        break;
                    default:
                        midPanelText.text = "背包";
                        break;
                }
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }

}
