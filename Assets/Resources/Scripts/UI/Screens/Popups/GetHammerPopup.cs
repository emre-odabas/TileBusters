using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;

namespace GameCore.UI
{
    public class GetHammerPopup : CoreScreen<GetHammerPopup>, IPopup
    {
        #region FIELDS

        //[FoldoutGroup("Components"), SerializeField] private

        #endregion

        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
        }

        #endregion

        #region RECALL FUNCTIONS

        

        #endregion

        #region BUTTONS

        public void OnClickPlayPuzzle()
        {
            Hide();
            GameManager.Instance.StartPlay_Puzzle();
        }

        #endregion
    }
}