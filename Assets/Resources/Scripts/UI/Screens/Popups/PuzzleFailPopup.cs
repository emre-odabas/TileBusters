using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;

namespace GameCore.UI
{
    public class PuzzleFailPopup : CoreScreen<PuzzleFailPopup>, IPopup
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
            GameManager.Instance.onPuzzleFail += Show;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onPuzzleFail -= Show;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        #endregion

        #region RECALL FUNCTIONS

        

        #endregion

        #region BUTTONS

        public void OnClickTopToContinue()
        {
            Hide();
            GameManager.Instance.GoHome();
        }

        #endregion
    }
}