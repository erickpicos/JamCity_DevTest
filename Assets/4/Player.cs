using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed = 2;
    [SerializeField] private AudioClip audioMetal;
    [SerializeField] private AudioClip audioGrass;
    [SerializeField] private AudioClip audioWood;
    [SerializeField] private AudioClip[] clipsRandoms;
    [SerializeField] private AudioSource audioSourceSteps; 
    [SerializeField] private AudioSource audioSourceSounds;  
    public bool IsMoving { get; private set; }
    public FloorType CurrentFloor { get; private set; }
    
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += Speed * Time.deltaTime * direction;

        IsMoving = direction.sqrMagnitude > 0.1f;
        CurrentFloor = GetFloorUnderneath();

        if (IsMoving)
        {
            switch (CurrentFloor)
            {
                case FloorType.Grass: audioSourceSteps.clip = audioGrass; break;
                case FloorType.Metal: audioSourceSteps.clip = audioMetal; break;
                case FloorType.Wood:  audioSourceSteps.clip = audioWood; break;
                case FloorType.None:  audioSourceSteps.clip = null; break;
            }
            if (!audioSourceSteps.isPlaying) { audioSourceSteps.Play(); }
        }
        else { audioSourceSteps.Stop(); }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int index = Random.Range(0, clipsRandoms.Length);
            audioSourceSounds.clip = clipsRandoms[index];
            audioSourceSounds.Play();
        }
        
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