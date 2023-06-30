using UnityEngine;

public class RectangleObstacle : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private Vector2 _margin;


    private void Start()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        localScale.y = Mathf.Abs(localScale.y);
        localScale.z = Mathf.Abs(localScale.z);
        transform.localScale = localScale;

        Vector2 worldBoxColliderScale = _boxCollider.size * transform.localScale;

        worldBoxColliderScale = worldBoxColliderScale - _margin;

        _boxCollider.size = worldBoxColliderScale / transform.localScale;

    }



}
