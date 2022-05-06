using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    void Start()
    {
        Debug.Log("space ship activated");
        AudioManager.instance.PlaySound(AudioManager.Sound.FireBurning,gameObject.transform.position);
    }
}
