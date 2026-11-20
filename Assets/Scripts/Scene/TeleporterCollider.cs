using System;
using UnityEngine;

public class TeleporterCollider : MonoBehaviour
{
    [SerializeField] private Teleporter teleporter;
    private DateTime lastTeleport = DateTime.UnixEpoch;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Teleporter") && (DateTime.Now - lastTeleport).TotalSeconds > 1)
        {
            lastTeleport = DateTime.Now;
            teleporter.Teleport(transform);
        }
    }
}
