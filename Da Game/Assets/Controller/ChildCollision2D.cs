using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This class is no longer being used",true)]
public class ChildCollision2D : MonoBehaviour
{
    [SerializeField] private Component parentScript;
    public void SetUpParentScript(Component ParentScript)
    {
        parentScript = ParentScript;
    }
    private void OnCollisionEnter2D (Collision2D other) {
        
    }
    private void OnCollisionStay2D(Collision2D other) {
        
    }
    private void OnCollisionExit2D(Collision2D other) {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
    }
    private void OnTriggerExit2D(Collider2D other) {
        
    }
    private void OnTriggerStay2D(Collider2D other) {
        
    }
}
