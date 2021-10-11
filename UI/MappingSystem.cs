using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MappingSystem : MonoBehaviour
{
    public Image playerIconMini, playerIconWorld;
    public Image map;


    const float theta = -43.2266f;
    const float width = 689;
    const float height = 436;

    Vector2 MapOrigin;
    void Start()
    {
        MapOrigin = new Vector2(-620, 407);
    }


    void Update()
    {
        UpdatePlayerMinimap(PlayerManager.instance.transform.position);
        UpdatePlayerWorldmap(PlayerManager.instance.transform.position);
    }

    Vector2 MiniMapping(Vector2 position)
    {
        Vector2 coord = Vector2.zero;
        Debug.Log(Vector2.Angle(new Vector2(1, 0), new Vector2(502, -473)));
        return coord;
    }
    Vector2 Mapping(Vector2 xy)
    {
        Vector2 result;
        xy += new Vector2(560, 316);
        Vector2 MapXY;
        MapXY.x = Mathf.Cos(theta) * xy.x - Mathf.Sin(theta) * xy.y;
        MapXY.y = Mathf.Sin(theta) * xy.x + Mathf.Cos(theta) * xy.y;
        result.y = 1 - (MapXY.x / 677);
        result.x = 1 - (MapXY.y / 1108);
        return result;
    }

    void UpdatePlayerMinimap(Vector3 worldPos)
    {
        Vector2 mapRes = Mapping(new Vector2(worldPos.x, worldPos.z));
        float angle = PlayerManager.instance.transform.eulerAngles.y;
        while (angle > 360)
        {
            angle -= 360;
        }
        float iconAngle = 270 + 270 - angle;
        playerIconMini.rectTransform.rotation = Quaternion.Euler(0, 0, iconAngle);
        map.rectTransform.anchoredPosition = new Vector2((0.5f - mapRes.x) * width + 7f , -(0.5f - mapRes.y) * height - 13);
    }
    void UpdatePlayerWorldmap(Vector3 worldPos)
    {
        Vector2 mapRes = Mapping(new Vector2(worldPos.x, worldPos.z));
        float angle = PlayerManager.instance.transform.eulerAngles.y;
        while (angle > 360)
        {
            angle -= 360;
        }
        float iconAngle = 270 + 270 - angle;
        playerIconWorld.rectTransform.rotation = Quaternion.Euler(0, 0, iconAngle);
        Debug.Log($"res = {mapRes.x} , {mapRes.y}");
        playerIconWorld.rectTransform.anchoredPosition = new Vector2(MapOrigin.x -10 + (mapRes.x * width * 1.8f),  MapOrigin.y + 30 - (mapRes.y * height * 1.8f));
    }
}
