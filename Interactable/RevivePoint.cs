using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePoint : MonoBehaviour
{
    MaterialPropertyBlock block;
    public Renderer thisRenderer;
    public GameObject activedIcon, activedMini, activedFX;
    private void Awake()
    {
        block = new MaterialPropertyBlock();
        ClearRecordPoint();
    }

    public void SetRecordPoint()
    {
        PlayerManager.instance.currentRevivePoint.ClearRecordPoint();
        PlayerManager.instance.currentRevivePoint = this;
        PlayerManager.instance.revivePoint = transform.position + transform.forward * 4;
        Debug.Log($"Player Change Record Point To {gameObject.name}");
        thisRenderer.GetPropertyBlock(block);
        block.SetColor("_EmissionColor", Color.white);
        thisRenderer.SetPropertyBlock(block);
        activedIcon.SetActive(true);
        activedMini.SetActive(true);
        activedFX.SetActive(true);
    }

    public void ClearRecordPoint()
    {
        thisRenderer.GetPropertyBlock(block);
        block.SetFloat("_Clipping_Level", 0f);
        block.SetColor("_EmissionColor", Color.black);
        thisRenderer.SetPropertyBlock(block);
        activedIcon.SetActive(false);
        activedMini.SetActive(false);
        activedFX.SetActive(false);
    }


}
