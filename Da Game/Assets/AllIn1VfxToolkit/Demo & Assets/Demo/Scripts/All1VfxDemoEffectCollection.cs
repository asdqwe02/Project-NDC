using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [CreateAssetMenu(fileName = "All1VfxDemoEffect", menuName = "AllIn1Vfx/DemoEffectCollection")]
    public class All1VfxDemoEffectCollection : ScriptableObject
    {
        public All1VfxDemoEffect[] demoEffectCollection;
    }
}