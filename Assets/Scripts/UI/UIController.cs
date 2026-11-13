using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject Pointer;

    public void PointerEnable(bool isActive)
    {
        Pointer.SetActive(isActive);
    }
}
