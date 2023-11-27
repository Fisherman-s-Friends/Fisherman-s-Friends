using UnityEngine;

public class FishWinDespawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }
}
