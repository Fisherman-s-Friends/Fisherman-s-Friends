using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    [RequireComponent(typeof(Image))]
    public class SpeakerController : MonoBehaviour, ISpeakerController
    {
        private Image speakerImage;

        private void Start()
        {
            if (!TryGetComponent(out speakerImage))
            {
                throw new ArgumentNullException("speaker",
                    "Couldn't find IDialogWindowManager attached to the game object");
            }
        }

        public void UpdateSpeaker(Actor actor)
        {
            if (actor == null)
            {
                speakerImage.color = new Color(0, 0, 0, 0);
                return;
            }

            speakerImage.sprite = actor.portrait;
            speakerImage.color = Color.white;
        }
    }
}