using UnityEngine;
public class ClickableSoundObject : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
   public void OnMouseDown()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(clickSound);
        }
    }
}