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
        
        #region MonoBehaviour
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
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        #endregion

        #region Controls

        public override void Show()
        {
            base.Show();
            m_Background.sprite = GameManager.Instance.m_CurrentTownData.m_Background;
        }

        #endregion

        #region Events

        

        #endregion
    }
}
