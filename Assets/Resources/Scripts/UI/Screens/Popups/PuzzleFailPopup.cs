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

        protected override void Start()
        {
            base.Start();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onPuzzleFail -= Show;
            }
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