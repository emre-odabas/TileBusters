using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Events;
using System.Linq;


namespace GameCore.Core.UI
{

    public class CoreCarousel<CarouselItem, CarouselItemData> : MonoBehaviour
        where CarouselItem : CoreCarouselItem
    {
        [System.Serializable]
        public class ItemData
        {
            public CarouselItem m_Item;
            public CarouselItemData m_Data;
            public int m_Page;
        }
        #region Events
        public UnityAction<int> onPageChange;
        #endregion
        [FoldoutGroup("Buttons", expanded: true)]
        public CoreButton m_BtnPrev;
        [FoldoutGroup("Buttons")]
        public CoreButton m_BtnNext;

        [FoldoutGroup("Components", expanded: true)]
        public ScrollRect m_ScrollRect;
        [FoldoutGroup("Components")]
        public RectTransform m_Content;
        [FoldoutGroup("Components")]
        public GridLayoutGroup m_PaginationGrid;
        [FoldoutGroup("Components")]
        public GridLayoutGroup m_Grid;

        [FoldoutGroup("Carousel", expanded: true)]
        public CarouselItem m_CarouselItemPrefab;
        [FoldoutGroup("Carousel")]
        public CoreCarouselPaginationItem m_CarouselPaginationItemPrefab;
        [FoldoutGroup("Carousel")]
        public int m_MaxItemCountPerRow = 3;
        [FoldoutGroup("Carousel")]
        public int m_MaxItemCountPerPage = 6;
        [FoldoutGroup("Carousel")]
        public int m_CurrentPage = 1;
        [FoldoutGroup("Carousel")]
        public bool m_PaginationActive = true;
        

        [FoldoutGroup("Item List", expanded: true), ReadOnly]
        public List<ItemData> m_CarouselItems = new List<ItemData>();
        [FoldoutGroup("Item List", expanded: true), ReadOnly]
        public List<CoreCarouselPaginationItem> m_PaginationItems = new List<CoreCarouselPaginationItem>();

        Vector2 m_GridSize;
        Vector2 m_GridSpacing;
        float m_PageWidth = 0;
        int m_PaginationCount;
        

        #region MonoBehaviour
        protected virtual void Awake()
        {
            m_BtnNext.onClick.AddListener(() => OnClickButtonNext());
            m_BtnPrev.onClick.AddListener(() => OnClickButtonPrev());
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {

        }
        // Update is called once per frame
        protected virtual void Update()
        {

        }
        #endregion
        #region Setup
        public virtual void Setup(List<CarouselItemData> _items)
        {
            if (m_CarouselItems.Count > 0)
            {
                foreach (var _item in m_CarouselItems)
                {
                    Destroy(_item.m_Item.gameObject);
                }
            }
            if (m_PaginationItems.Count > 0)
            {
                foreach (var _item in m_PaginationItems)
                {
                    Destroy(_item.gameObject);
                }
            }

            m_CarouselItems.Clear();
            m_PaginationItems.Clear();
            m_GridSize = m_Grid.cellSize;
            m_GridSpacing = m_Grid.spacing;
            SetGridSize(_items.Count);

            foreach (var _itemData in _items)
            {
                ItemData _carouselItemData = new ItemData();
                _carouselItemData.m_Item = CreatePrefab(_itemData);
                _carouselItemData.m_Data = _itemData;
                m_CarouselItems.Add(_carouselItemData);
                _carouselItemData.m_Item.Setup();
            }

            CreatePagination();
            UpdateGrid(m_CarouselItems);
            ShowPage(m_CurrentPage);

            m_CurrentPage = 1;

            if (m_PaginationActive)
                m_PaginationGrid.gameObject.SetActive(true);
            else
                m_PaginationGrid.gameObject.SetActive(false);
        }
        public virtual CarouselItem CreatePrefab(CarouselItemData _itemData)
        {
            CarouselItem cloneItem = Instantiate(m_CarouselItemPrefab, m_Grid.transform);
            AddListenerCarouselItem(cloneItem);
            return cloneItem;
        }
        public virtual void UpdateGrid()
        {
            UpdateGrid(m_CarouselItems);
        }
        public virtual void UpdateGrid(List<ItemData> _itemDatas)
        {
            for (int i = 0; i < _itemDatas.Count; i++)
            {
                //Page Calculation
                float _division = (float)(i + 1) / (float)m_MaxItemCountPerPage;
                int _page = 0;
                if (_division == 0) _page = 1;
                else _page = (int)Mathf.Ceil(_division);
                m_CarouselItems[i].m_Page = _page;
            }
        }
        void SetGridSize(int itemCount)
        {
            float _cellSize = m_GridSize.x + m_GridSpacing.x;
            m_PageWidth = _cellSize * m_MaxItemCountPerRow;
            float division = (float)itemCount / (float)m_MaxItemCountPerPage;
            m_PaginationCount = (int)Mathf.Ceil(division);
            m_Grid.GetComponent<RectTransform>().sizeDelta = new Vector2(m_PageWidth * m_PaginationCount, m_Grid.GetComponent<RectTransform>().sizeDelta.y);
        }
        void CreatePagination()
        {
            for (int i = 1; i <= m_PaginationCount; i++)
            {
                CoreCarouselPaginationItem clonePagination = Instantiate(m_CarouselPaginationItemPrefab, m_PaginationGrid.transform);
                m_PaginationItems.Add(clonePagination);
                AddListenerPagination(clonePagination, i);
            }
        }
        void AddListenerPagination(CoreCarouselPaginationItem _button, int _pageIndex)
        {
            if (_button.m_Button == null) _button.m_Button = _button.GetComponent<CoreButton>();
            _button.m_Button.onClick.AddListener(() => OnClickButtonPagination(_pageIndex));
        }
        void AddListenerCarouselItem(CarouselItem _carouselItem)
        {
            if (_carouselItem.m_Button == null) _carouselItem.m_Button = _carouselItem.GetComponent<CoreButton>();
            _carouselItem.m_Button.onClick.AddListener(() => OnClickButtonCarouselItem(_carouselItem));
        }
        #endregion
        #region Pagination / Slide
        public virtual void NextPage()
        {
            m_CurrentPage++;
            if (m_CurrentPage > m_PaginationCount)
                m_CurrentPage = 1;
            ShowPage(m_CurrentPage);
        }
        public virtual void PrevPage()
        {
            m_CurrentPage--;
            if (m_CurrentPage < 1)
                m_CurrentPage = m_PaginationCount;
            ShowPage(m_CurrentPage);
        }
        public virtual void ShowPage(int _pageIndex)
        {
            m_Grid.GetComponent<RectTransform>().DOLocalMoveX(m_PageWidth * -(_pageIndex - 1), 0.2f)
            .OnComplete(() =>
            {
                onPageChange?.Invoke(_pageIndex);
            });
            foreach (var _paginationItem in m_PaginationItems)
            {
                _paginationItem.UnSelect();
            }
            m_PaginationItems[_pageIndex - 1].Select();
        }
        #endregion
        #region Events
        protected virtual void OnClickButtonPagination(int _pageIndex)
        {
            ShowPage(_pageIndex);
        }
        protected virtual void OnClickButtonCarouselItem(CarouselItem _item)
        {
            UpdateGrid();
        }
        protected virtual void OnClickButtonPrev()
        {
            PrevPage();
        }
        protected virtual void OnClickButtonNext()
        {
            NextPage();
        }
        #endregion
        #region Items
        public List<ItemData> GetItemsInPage(int _pageIndex)
        {
            if (_pageIndex > m_PaginationCount) return null;
            return m_CarouselItems.Where(x => x.m_Page == _pageIndex).ToList();
        }
        #endregion
    }
}
