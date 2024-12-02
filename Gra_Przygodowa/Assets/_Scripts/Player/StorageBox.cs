using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets._Scripts.Enums.Enum;

public class StorageBox : MonoBehaviour
{
    public bool isPlayerInRange;
    public float distanceToObject = 10f;

    // All items in storage box
    [SerializeField] public List<string> items;


    public BoxType thisBoxType;

    private void Update()
    {
        isPlayerInRange = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, this.transform.position) < distanceToObject;
    }

}
