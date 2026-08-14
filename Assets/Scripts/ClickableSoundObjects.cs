using UnityEngine;
public class ClickableSoundObject : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
   public void OnMouseDown()
   {
       if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
       {
           return;
       }

       if (SoundManager.Instance != null)
       {
           SoundManager.Instance.PlaySFX(clickSound);
       }
   }
}