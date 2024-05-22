using UnityEngine;
using GameCore.Core.UI;
using GameCore.Managers;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using GameCore.Controllers;

namespace GameCore.UI
{
    public class PuzzleScreen : CoreScreen<PuzzleScreen>
    {
        [FoldoutGroup("Components")]
        [FoldoutGroup("Components/Utilities"), SerializeField] private Image m_Background; 
        
        #region MONOBEHAVIOUR
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.onStartPlayPuzzle += Show;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.onStartPlayPuzzle -= Show;
            }
        }

        #endregion

        #region OVERRIDES

        public override void Show()
        {
            base.Show();
            m_Background.sprite = TownDataList.Instance.GetCurrentTownData().m_Background;

            //Temp
            foreach (var item in GetComponentsInChildren<CurrencyBar>())
            {
                item.UpdateCurrency(false);
            }
        }

        #endregion


        #region BUTTONS

        public void OnClickBack()
        {
            GameManager.Instance.GoHome();
        }

        #endregion
    }
}
