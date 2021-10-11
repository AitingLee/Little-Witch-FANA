using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    public List<ItemInformation> orbItemInfos;


    private void OnEnable()
    {
        orbItemInfos = new List<ItemInformation>();
    }

    public void DisplayGetItemPanel()
    {
        CanvasManager.instance.getItemPanel.Display(orbItemInfos, this);
    }

}
