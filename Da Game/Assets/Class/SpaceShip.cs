using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlaySound(AudioManager.Sound.FireBurning,gameObject.transform.position);
    }
}
