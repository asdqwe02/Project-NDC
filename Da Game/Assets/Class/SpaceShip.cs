using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.FireBurning,gameObject.transform.position);
    }
}
