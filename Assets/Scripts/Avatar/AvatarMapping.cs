using System;
using UnityEngine;

public class AvatarMapping : MonoBehaviour
{
    [SerializeField] private MapTrans head;
    [SerializeField] private MapTrans leftHand;
    [SerializeField] private MapTrans rightHand;

    [SerializeField] private Transform headBodyMap;
    [SerializeField] private Vector3 bodyOffSet;
    [SerializeField] private bool hasMapped = false;

    public PlayerMapRef mapRef;


    private void Start()
    {
        if(mapRef != null) MapPlayerMap(mapRef);
    }

    private void OnValidate()
    {
        if (head.bodyTarget == null) head.bodyTarget = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
        if (rightHand.bodyTarget == null) rightHand.bodyTarget = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1);
        if (leftHand.bodyTarget == null) leftHand.bodyTarget = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2);
        if (headBodyMap == null)
        {
            headBodyMap = transform.GetChild(0).GetChild(0).GetChild(0);
            bodyOffSet = head.bodyTarget.position - headBodyMap.position;
        }
    }

    public void MapPlayerMap(PlayerMapRef playerMapRef)
    {
        mapRef = playerMapRef;
        // if (mapRef == null) mapRef = FindObjectOfType<PlayerMapRef>();
        if (head.playerTarget == null) head.playerTarget = mapRef.head;
        if (leftHand.playerTarget == null) leftHand.playerTarget = mapRef.leftHand;
        if (rightHand.playerTarget == null) rightHand.playerTarget = mapRef.rightHand;
        
        hasMapped = true;
    }

    private void Update()
    {
        if (!hasMapped) return;
        headBodyMap.position = head.bodyTarget.position + bodyOffSet;
        headBodyMap.rotation = head.bodyTarget.rotation;
        head.MapTransform();
        leftHand.MapTransform();
        rightHand.MapTransform();

    }
}

[Serializable]
public struct MapTrans
{
    public Transform playerTarget;
    public Transform bodyTarget;
    [SerializeField] private Vector3 posOffset;
    [SerializeField] private Vector3 rotOffset;

    public void MapTransform()
    {
        bodyTarget.position = playerTarget.position + posOffset;
        bodyTarget.rotation = playerTarget.rotation * Quaternion.Euler(rotOffset);
    }
}
