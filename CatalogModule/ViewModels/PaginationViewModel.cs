using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace CatalogModule.ViewModels
{
    public class PaginationViewModel : BindableBase
    {
        private DelegateCommand _firstPageCommand;
        private DelegateCommand _previousPageCommand;
        private DelegateCommand _nextPageCommand;
        private DelegateCommand _lastPageCommand;
        private int _startIndex;
        private int _itemsPerPage;
        private int _totalItems;

        public PaginationViewModel()
        {
            _startIndex = 1;
            _totalItems = 0;
            //_itemsPerPage = GetItemsPerPageFromConfig();
            _itemsPerPage = 100;
        }

        //private int GetItemsPerPageFromConfig()
        //{
        //    var moduleConfigs = ModuleConfigs.GetConfigs(CatalogConstants.Module, CatalogConstants.Section);

        //    if (moduleConfigs.TryGetValue(nameof(ItemsPerPage), out string itemsPerPageFromConfig))
        //    {
        //        if (!Int32.TryParse(itemsPerPageFromConfig, out int number))
        //        {
        //            return 20;
        //        };
        //        return number;
        //    }
        //    else
        //    {
        //        return 20;
        //    }
        //}

        public int ItemsPerPage
        {
            get => _itemsPerPage;
            private set { SetProperty(ref _itemsPerPage, value); }
        }

        public int StartIndex
        {
            get => _startIndex;
        }

        public int EndIndex
        {
            get => _startIndex + _itemsPerPage - 1 < _totalItems ?
                   _startIndex + _itemsPerPage - 1 :
                   _totalItems;
        }

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                SetProperty(ref _totalItems, value);

                // in case current view with start index > totalItems
                // this happens when we are at last page and applying a filter which returns a subset of items belong to a page 
                // less than current page
                if (_startIndex > value)
                {
                    _startIndex = 1;
                    _skipItems = 0;
                }

                if (value == 0)
                {
                    _startIndex = 0;
                }

                RaisePropertyChanged(nameof(StartIndex));
                RaisePropertyChanged(nameof(EndIndex));
                RaisePropertyChanged(nameof(CanGoToNextPage));
                RaisePropertyChanged(nameof(CanGoToPreviousPage));
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            private set
            {
                SetProperty(ref _currentPage, value);
                ChangePage();
            }
        }

        public ICommand FirstPageCommand => _firstPageCommand ?? (_firstPageCommand = new DelegateCommand(GoToFirstPage).ObservesCanExecute(() => CanGoToPreviousPage));
        public ICommand PreviousPageCommand => _previousPageCommand ?? (_previousPageCommand = new DelegateCommand(GoToPreviousPage).ObservesCanExecute(() => CanGoToPreviousPage));
        public ICommand NextPageCommand => _nextPageCommand ?? (_nextPageCommand = new DelegateCommand(GoToNextPage).ObservesCanExecute(() => CanGoToNextPage));
        public ICommand LastPageCommand => _lastPageCommand ?? (_lastPageCommand = new DelegateCommand(GoToLastPage).ObservesCanExecute(() => CanGoToNextPage));

        private void GoToFirstPage()
        {
            _currentPage = 0;
            _startIndex = 1;
            ChangePage();
        }

        private void GoToPreviousPage()
        {
            _currentPage -= 1;
            _startIndex -= _itemsPerPage;
            ChangePage();
        }

        private void GoToNextPage()
        {
            _currentPage += 1;
            _startIndex += _itemsPerPage;
            ChangePage();
        }

        private void GoToLastPage()
        {
            var totalPages = Math.Ceiling((double)_totalItems / _itemsPerPage);
            _currentPage = (int)totalPages - 1;
            _startIndex = _itemsPerPage * ((int)totalPages - 1) + 1;
            ChangePage();
        }

        private int _skipItems;

        public int SkipItems
        {
            get { return _skipItems; }
            private set { SetProperty(ref _skipItems, value); }
        }

        private void ChangePage()
        {
            _skipItems = (_startIndex / _itemsPerPage) * _itemsPerPage;
            RaisePropertyChanged(nameof(CurrentPage));
            RaisePropertyChanged(nameof(StartIndex));
            RaisePropertyChanged(nameof(EndIndex));
            RaisePropertyChanged(nameof(CanGoToNextPage));
            RaisePropertyChanged(nameof(CanGoToPreviousPage));
        }

        /// <summary>
        /// Return true when current item index is bigger than number of item per page, otherwise return false
        /// </summary>
        private bool CanGoToPreviousPage
        {
            get
            {
                return _startIndex - _itemsPerPage > 0 ? true : false;
            }
        }

        private bool CanGoToNextPage
        {
            get
            {
                return _startIndex + _itemsPerPage - 1 < _totalItems ? true : false;
            }
        }
    }
}
