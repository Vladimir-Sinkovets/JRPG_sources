using Assets.Game.Scripts.Common.Input;
using Assets.Game.Scripts.Locations.Services.Input;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Locations.DialogueSystem
{
    public class DialogueSystemInputMapSwitcher : MonoBehaviour
    {
        [Inject] private IUIInputController _UIInputController;
        [Inject] private ILocationInputController _locationInputController;

        public void OnConversationStartedHandler()
        {
            //_UIInputController.EnableMap();
            _locationInputController.DisableMap(); // todo: Add game pause

            DialogueManager.SetDialogueSystemInput(false);
        }

        public void OnConversationEndedHandler()
        {
            //_UIInputController.DisableMap();
            _locationInputController.EnableMap();

            DialogueManager.SetDialogueSystemInput(true);
        }
    }
}