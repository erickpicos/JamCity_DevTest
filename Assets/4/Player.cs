using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed = 2;

    public bool IsMoving { get; private set; }
    public FloorType CurrentFloor { get; private set; }

    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += Speed * Time.deltaTime * direction;

        IsMoving = direction.sqrMagnitude > 0.1f;
        CurrentFloor = GetFloorUnderneath();

        Debug.Log($"IsMoving: {IsMoving}, Floor: {CurrentFloor}");
    }

    private FloorType GetFloorUnderneath()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (!Physics.Raycast(ray, out RaycastHit hit, 50))
            return FloorType.None;

        Floor floor = hit.collider.GetComponent<Floor>();
        if (floor == null)
            return FloorType.None;

        return floor.Type;
    }
}