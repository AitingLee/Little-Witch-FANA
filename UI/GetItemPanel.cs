using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemPanel : MonoBehaviour
{
    public GameObject[] m_panels;
    public Item2D[] m_item2Ds;
    public GameObject gatherSelect;

    LightOrb currentOrb;
    Stack<int> indexToTakeOut;

    TreasureChest currentChest;

    List<ItemInformation> currentItemList;
    int currentSelectIndex;

    private void Start()
    {
        m_item2Ds = new Item2D[m_panels.Length];
        for (int i = 0; i < m_panels.Length; i++)
        {
            m_item2Ds[i] = m_panels[i].transform.GetChild(0).GetComponent<Item2D>();
        }
        currentItemList = new List<ItemInformation>();
    }

    public void Display(List<ItemInformation> itemInformations, LightOrb lightOrb)
    {
        currentOrb = lightOrb;
        currentChest = null;
        Display(itemInformations);
    }

    public void Display(List<ItemInformation> itemInformations, TreasureChest chest)
    {
        currentChest = chest;
        currentOrb = null;
        Display(itemInformations);
    }

    public void Display(List<ItemInformation> itemInformations)
    {
        indexToTakeOut = new Stack<int>();
        currentItemList = itemInformations;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        if (!gatherSelect.activeSelf)
        {
            gatherSelect.SetActive(true);
        }

        gatherSelect.transform.position = m_panels[0].transform.position;
        currentSelectIndex = 0;

        int count = itemInformations.Count;
        for (int i = 0; i < m_panels.Length; i++)
        {
            if (i < count)
            {
                m_panels[i].SetActive(true);
            }
            else
            {
                m_panels[i].SetActive(false);
            }
        }

        for (int i = 0; i < count; i++)
        {
            m_item2Ds[i].itemInformation = itemInformations[i];
            m_item2Ds[i].UpdateInfo();
            if (!m_item2Ds[i].gameObject.activeSelf)
            {
                m_item2Ds[i].gameObject.SetActive(true);
            }
        }
    }

    public void GatherItemFromPanel()
    {
        CanvasManager.instance.inventory.PutInBag(currentItemList[currentSelectIndex]);
        indexToTakeOut.Push(currentSelectIndex);
        m_item2Ds[currentSelectIndex].gameObject.SetActive(false);
        SetPanelToNext();
    }

    public void SetPanelToNext()
    {
        if (currentSelectIndex < currentItemList.Count -1 )
        {
            currentSelectIndex++;
            gatherSelect.transform.position = m_panels[currentSelectIndex].transform.position;
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        if (currentOrb != null)
        {
            if (currentItemList.Count > indexToTakeOut.Count)
            {
                foreach (int i in indexToTakeOut)
                {
                    currentItemList.RemoveAt(i);
                }
                currentOrb.orbItemInfos = currentItemList;
            }
            else
            {
                currentOrb.orbItemInfos = null;
                currentOrb.gameObject.SetActive(false);
            }
        }

        if (currentChest != null)
        {
            if (currentItemList.Count > indexToTakeOut.Count)
            {
                foreach (int i in indexToTakeOut)
                {
                    currentItemList.RemoveAt(i);
                }
                currentChest.chestItemInfos = currentItemList;
            }
            else
            {
                currentChest.chestItemInfos = null;
                currentChest.EmptyChest();
            }
        }

        currentOrb = null;
        currentChest = null;

        if (gameObject != null && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

}
