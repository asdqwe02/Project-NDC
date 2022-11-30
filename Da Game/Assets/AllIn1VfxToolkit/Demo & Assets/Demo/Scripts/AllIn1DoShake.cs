using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1DoShake : MonoBehaviour
    {
        [SerializeField] private bool doShakeOnStart;
        [SerializeField] private float shakeAmount;
        
        private void Start()
        {
            if(doShakeOnStart) DoShake();
        }

        private void DoShake()
        {
            AllIn1Shaker.i.DoCameraShake(shakeAmount);
        }
    }
}