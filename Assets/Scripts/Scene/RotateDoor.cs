using DG.Tweening;
using UnityEngine;

public class RotateDoor : MonoBehaviour
{
    [SerializeField] private Vector3 closeRotation;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float closeTime;
    [SerializeField] private float openTime;
    
    public void Close()
    {
        transform.DORotate(closeRotation, closeTime);
    }
    public void Open()
    {
        transform.DORotate(openRotation, openTime);
    }
}
