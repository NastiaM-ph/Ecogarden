 
using UnityEngine;
using TMPro;

public class MuteButtonUI : MonoBehaviour

{
    [SerializeField] private TMP_Text muteText;

    public void ToggleMute()

    {
        SoundManager.Instance.ToggleMute();
 
        if (SoundManager.Instance.IsMuted())

        {
            muteText.text = "ON";
        }
        else

        {
            muteText.text = "OFF";

        }

    }

}

