using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class VibrationControls : MonoBehaviour
{
    public static VibrationControls instance;

    [SerializeField] private float vibrationDuration = 250f;
    private void Awake()
    {
        instance = this;
    }
     public void Vibrate()
     {
         if (PlayerPrefs.GetInt("Haptics", 1) == 1) 
         {
             /*if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
             {
                //StartCoroutine(VibrateCoroutine());
                HapticFeedback.HeavyFeedback();
             }
             else
             {
                 Debug.LogWarning("Vibration is only supported on Android devices.");
             }*/
                HapticFeedback.HeavyFeedback();
         }

     }
     private IEnumerator VibrateCoroutine()
     {
         Handheld.Vibrate();
         yield return new WaitForSeconds(vibrationDuration / 1000f);
     }
}
