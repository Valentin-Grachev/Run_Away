using UnityEngine;

public class HidableArea : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _areaRenderer;

    void Start()
    {
        _areaRenderer.color = Color.clear;
    }

}
