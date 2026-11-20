using System;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform endPosition;

    public void Teleport(Transform target)
    {
        CharacterController cc = target.GetComponent<CharacterController>();
        cc.enabled = false;
        target.position = endPosition.position;
        target.Rotate(0, Quaternion.Angle(transform.rotation, endPosition.rotation), 0);
        cc.enabled = true;
    }
}
