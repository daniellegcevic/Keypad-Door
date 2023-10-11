using UnityEngine;
using System.Collections;

public class Keypad : MonoBehaviour
{
    #region Variables

        #region References

            [Header("References")]
            public GameObject keypadLightOff;
            public GameObject keypadLightRed;
            public GameObject keypadLightGreen;
            public InteractableObject objectItUnlocks;
            public ViewableObject viewableObject;

        #endregion

        #region Access Code

            [Header("Access Code")]
            public int firstDigit;
            public int secondDigit;
            public int thirdDigit;
            public int fourthDigit;

        #endregion

        #region Sound Effects

            [Header("Sound Effects")]
            [SerializeField] private string accessDeniedSoundEffect;
            [SerializeField] private string accessGrantedSoundEffect;

        #endregion

        #region DEBUG

            private bool accessDenied;
            private bool firstDigitCorrect;
            private bool secondDigitCorrect;
            private bool thirdDigitCorrect;
            private bool accessGranted;

            private bool keypadLightActive;

        #endregion

    #endregion

    #region Custom Methods

        public void PressButton(int keypadNumber)
        {
            if(keypadNumber == 11)
            {
                if(accessDenied)
                {
                    StartCoroutine(AccessDenied());
                }
                else if(accessGranted)
                {
                    StartCoroutine(AccessGranted());
                }
                else
                {
                    StartCoroutine(AccessDenied());
                }
            }
            else if(!accessDenied)
            {
                if(!firstDigitCorrect)
                {
                    if(keypadNumber == firstDigit)
                    {
                        firstDigitCorrect = true;
                    }
                    else
                    {
                        accessDenied = true;
                    }
                }
                else if(!secondDigitCorrect)
                {
                    if(keypadNumber == secondDigit)
                    {
                        secondDigitCorrect = true;
                    }
                    else
                    {
                        accessDenied = true;
                    } 
                }
                else if(!thirdDigitCorrect)
                {
                    if(keypadNumber == thirdDigit)
                    {
                        thirdDigitCorrect = true;
                    }
                    else
                    {
                        accessDenied = true;
                    } 
                }
                else
                {
                    if(keypadNumber == fourthDigit)
                    {
                        accessGranted = true;
                    }
                    else
                    {
                        accessDenied = true;
                    } 
                }
            }
        }

        public void PerformObjectAction()
        {
            if(objectItUnlocks)
            {
                objectItUnlocks.UnlockObject();
                viewableObject.objectDisabled = true;
            }
        }

    #endregion

    #region Coroutines

        private IEnumerator AccessDenied()
        {
            if(!keypadLightActive)
            {
                keypadLightActive = true;

                yield return new WaitForSeconds(0.16f);

                firstDigitCorrect = false;
                secondDigitCorrect = false;
                thirdDigitCorrect = false;
                accessDenied = false;

                keypadLightRed.SetActive(true);
                keypadLightOff.SetActive(false);
                SoundEffectManager.instance.PlaySoundEffect(accessDeniedSoundEffect, gameObject);

                yield return new WaitForSeconds(2f);

                keypadLightOff.SetActive(true);
                keypadLightRed.SetActive(false);

                keypadLightActive = false;
            }
        }

        private IEnumerator AccessGranted()
        {
            if(!keypadLightActive)
            {
                keypadLightActive = true;

                viewableObject.exitingDisabled = true;

                yield return new WaitForSeconds(0.16f);

                keypadLightGreen.SetActive(true);
                keypadLightOff.SetActive(false);
                SoundEffectManager.instance.PlaySoundEffect(accessGrantedSoundEffect, gameObject);

                viewableObject.objectDisabled = true;
                viewableObject.objectCollider.enabled = true;
                
                yield return new WaitForSeconds(1f);

                objectItUnlocks.UnlockObject();
                viewableObject.EndViewing();

                yield return new WaitForSeconds(3f);

                keypadLightOff.SetActive(true);
                keypadLightGreen.SetActive(false);

                keypadLightActive = false;
            }
        }

    #endregion
}