using Unity.VisualScripting;
using UnityEngine;

public class Object_CheckPoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawPoint;
    public bool isActive { get; private set; }
    private Animator anim;
    private AudioSource fireAudioSource;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
       fireAudioSource = GetComponent<AudioSource>();
    }

    public string GetCheckpointId() => checkpointId;

    public Vector3 GetPostion() => respawPoint == null ? transform.position : respawPoint.position;

    public void ActiveteCheckPoint(bool activate)
    {
        isActive = activate;
        anim.SetBool("isActive", activate);

        if(isActive && fireAudioSource.isPlaying == false)
            fireAudioSource.Play();
        

        if(isActive == false)
            fireAudioSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActiveteCheckPoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.unlockedCheckpoints.TryGetValue(checkpointId, out active);
        ActiveteCheckPoint(active);
       
    }

    public void SaveData(ref GameData data)
    {
        if(isActive == false)
            return;

        if (data.unlockedCheckpoints.ContainsKey(checkpointId) == false)
            data.unlockedCheckpoints.Add(checkpointId, true);
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = System.Guid.NewGuid().ToString();
        }
#endif
    }
}
