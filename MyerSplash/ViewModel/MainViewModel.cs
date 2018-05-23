using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using JP.Utils.Helper;
using Microsoft.QueryStringDotNET;
using MyerSplash.Common;
using MyerSplash.Data;
using MyerSplash.Model;
using MyerSplash.ViewModel.DataViewModel;
using MyerSplashShared.API;
using MyerSplashShared.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;
using Windows.UI.Core;

namespace MyerSplash.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private const int NEW_INDEX = 0;
        private const int FEATURED_INDEX = 1;
        private const int RANDOM_INDEX = 2;

        public const string NEW_NAME = "NEW";
        public const string FEATURED_NAME = "FEATURED";
        public const string RANDOM_NAME = "RANDOM";

        public Dictionary<int, string> INDEX_TO_NAME = new Dictionary<int, string>()
        {
            { NEW_INDEX,NEW_NAME },
            { FEATURED_INDEX,FEATURED_NAME },
            { RANDOM_INDEX,RANDOM_NAME }
        };

        private string DefaultTitleName
        {
            get
            {
                var key = AppSettings.Instance.DefaultCategory;
                if (INDEX_TO_NAME.ContainsKey(key))
                {
                    return INDEX_TO_NAME[AppSettings.Instance.DefaultCategory];
                }
                else
                {
                    return "MyerSplash";
                }
            }
        }

        private Dictionary<int, ImageDataViewModel> _vms = new Dictionary<int, ImageDataViewModel>();

        private ImageDataViewModel _dataVM;
        public ImageDataViewModel DataVM
        {
            get
            {
                return _dataVM;
            }
            set
            {
                if (_dataVM != value)
                {
                    _dataVM = value;
                    RaisePropertyChanged(() => DataVM);
                }
            }
        }

        public bool IsInView { get; set; }

        private ObservableCollection<string> _tabs;
        public ObservableCollection<string> Tabs
        {
            get
            {
                return _tabs;
            }
            set
            {
                if (_tabs != value)
                {
                    _tabs = value;
                    RaisePropertyChanged(() => Tabs);
                }
            }
        }

        #region Search

        private bool _showSearchBar;
        public bool ShowSearchBar
        {
            get
            {
                return _showSearchBar;
            }
            set
            {
                if (_showSearchBar != value)
                {
                    _showSearchBar = value;
                    RaisePropertyChanged(() => ShowSearchBar);
                }
            }
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get
            {
                return _searchKeyword;
            }
            set
            {
                if (_searchKeyword != value)
                {
                    _searchKeyword = value;
                    RaisePropertyChanged(() => SearchKeyword);
                }
            }
        }

        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                if (_searchCommand != null) return _searchCommand;
                return _searchCommand = new RelayCommand(() =>
                  {
                      ShowSearchBar = true;
                      NavigationService.AddOperation(() =>
                          {
                              if (ShowSearchBar)
                              {
                                  ShowSearchBar = false;
                                  return true;
                              }
                              else return false;
                          });
                  });
            }
        }

        private RelayCommand _hideSearchCommand;
        public RelayCommand HideSearchCommand
        {
            get
            {
                if (_hideSearchCommand != null) return _hideSearchCommand;
                return _hideSearchCommand = new RelayCommand(() =>
                  {
                      ShowSearchBar = false;
                  });
            }
        }

        private RelayCommand _beginSearchCommand;
        public RelayCommand BeginSearchCommand
        {
            get
            {
                if (_beginSearchCommand != null) return _beginSearchCommand;
                return _beginSearchCommand = new RelayCommand(async () =>
                  {
                      if (ShowSearchBar)
                      {
                          ShowSearchBar = false;
                          await SearchByKeywordAsync();
                          SearchKeyword = "";
                      }
                  });
            }
        }

        #endregion Search

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand != null) return _refreshCommand;
                return _refreshCommand = new RelayCommand(async () =>
                  {
                      await RefreshListAsync();
                  });
            }
        }

        private RelayCommand _retryCommand;
        public RelayCommand RetryCommand
        {
            get
            {
                if (_retryCommand != null) return _retryCommand;
                return _retryCommand = new RelayCommand(async () =>
                  {
                      FooterLoadingVisibility = Visibility.Visible;
                      FooterReloadVisibility = Visibility.Collapsed;
                      await DataVM.RetryAsync();
                  });
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    RaisePropertyChanged(() => IsRefreshing);
                }
            }
        }

        private Visibility _footerLoadingVisibility;
        public Visibility FooterLoadingVisibility
        {
            get
            {
                return _footerLoadingVisibility;
            }
            set
            {
                if (_footerLoadingVisibility != value)
                {
                    _footerLoadingVisibility = value;
                    RaisePropertyChanged(() => FooterLoadingVisibility);
                }
            }
        }

        private Visibility _endVisiblity;
        public Visibility EndVisibility
        {
            get
            {
                return _endVisiblity;
            }
            set
            {
                if (_endVisiblity != value)
                {
                    _endVisiblity = value;
                    RaisePropertyChanged(() => EndVisibility);
                }
            }
        }

        private Visibility _noItemHintVisibility;
        public Visibility NoItemHintVisibility
        {
            get
            {
                return _noItemHintVisibility;
            }
            set
            {
                if (_noItemHintVisibility != value)
                {
                    _noItemHintVisibility = value;
                    RaisePropertyChanged(() => NoItemHintVisibility);
                }
            }
        }

        private Visibility _noNetworkHintVisibility;
        public Visibility NoNetworkHintVisibility
        {
            get
            {
                return _noNetworkHintVisibility;
            }
            set
            {
                if (_noNetworkHintVisibility != value)
                {
                    _noNetworkHintVisibility = value;
                    RaisePropertyChanged(() => NoNetworkHintVisibility);
                }
            }
        }

        private Visibility _footerReloadVisibility;
        public Visibility FooterReloadVisibility
        {
            get
            {
                return _footerReloadVisibility;
            }
            set
            {
                if (_footerReloadVisibility != value)
                {
                    _footerReloadVisibility = value;
                    RaisePropertyChanged(() => FooterReloadVisibility);
                }
            }
        }

        private RelayCommand _goToSettingsCommand;
        public RelayCommand GoToSettingsCommand
        {
            get
            {
                if (_goToSettingsCommand != null) return _goToSettingsCommand;
                return _goToSettingsCommand = new RelayCommand(() =>
                  {
                      ShowSettingsUC = true;
                      NavigationService.AddOperation(() =>
                          {
                              if (ShowSettingsUC)
                              {
                                  ShowSettingsUC = false;
                                  return true;
                              }
                              return false;
                          });
                  });
            }
        }

        private bool _showAboutUC;
        public bool ShowAboutUC
        {
            get
            {
                return _showAboutUC;
            }
            set
            {
                if (_showAboutUC != value)
                {
                    _showAboutUC = value;
                    RaisePropertyChanged(() => ShowAboutUC);
                }
            }
        }

        private bool _showDownloadsUC;
        public bool ShowDownloadsUC
        {
            get
            {
                return _showDownloadsUC;
            }
            set
            {
                if (_showDownloadsUC != value)
                {
                    _showDownloadsUC = value;
                    RaisePropertyChanged(() => ShowDownloadsUC);
                }
            }
        }

        private bool _showSettingsUC;
        public bool ShowSettingsUC
        {
            get
            {
                return _showSettingsUC;
            }
            set
            {
                if (_showSettingsUC != value)
                {
                    _showSettingsUC = value;
                    RaisePropertyChanged(() => ShowSettingsUC);
                }
            }
        }

        private RelayCommand _showDownloadsCommand;
        public RelayCommand ShowDownloadsCommand
        {
            get
            {
                if (_showDownloadsCommand != null) return _showDownloadsCommand;
                return _showDownloadsCommand = new RelayCommand(() =>
                  {
                      ShowDownloadsUC = !ShowDownloadsUC;

                      if (ShowDownloadsUC)
                      {
                          NavigationService.AddOperation(() =>
                          {
                              if (ShowDownloadsUC)
                              {
                                  ShowDownloadsUC = false;
                                  return true;
                              }
                              return false;
                          });
                      }
                  });
            }
        }

        private RelayCommand _goToAboutCommand;
        public RelayCommand GoToAboutCommand
        {
            get
            {
                if (_goToAboutCommand != null) return _goToAboutCommand;
                return _goToAboutCommand = new RelayCommand(() =>
                  {
                      ShowAboutUC = true;
                      NavigationService.AddOperation(() =>
                          {
                              if (ShowAboutUC)
                              {
                                  ShowAboutUC = false;
                                  return true;
                              }
                              return false;
                          });
                  });
            }
        }

        private RelayCommand _toggleFullScreenCommand;
        public RelayCommand ToggleFullScreenCommand
        {
            get
            {
                if (_toggleFullScreenCommand != null) return _toggleFullScreenCommand;
                return _toggleFullScreenCommand = new RelayCommand(() =>
                  {
                      var isInFullScreen = ApplicationView.GetForCurrentView().IsFullScreenMode;
                      if (!isInFullScreen)
                      {
                          ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                      }
                      else
                      {
                          ApplicationView.GetForCurrentView().ExitFullScreenMode();
                      }
                  });
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    var lastValue = value;

                    _selectedIndex = value;
                    RaisePropertyChanged(() => SelectedIndex);
                    if (value == -1)
                    {
                        return;
                    }

                    if (lastValue != -1)
                    {
                        DataVM = CreateOrCacheDataVm(value);
                        if (DataVM != null && DataVM.DataList.Count == 0)
                        {
                            var task = RefreshListAsync();
                        }
                    }
                }
            }
        }

        private UnsplashImageFactory _normalFactory;
        public UnsplashImageFactory NormalFactory
        {
            get
            {
                return _normalFactory ?? (_normalFactory = new UnsplashImageFactory(false));
            }
        }

        private UnsplashImageFactory _featuredFactory;
        public UnsplashImageFactory FeaturedFactory
        {
            get
            {
                return _featuredFactory ?? (_featuredFactory = new UnsplashImageFactory(true));
            }
        }

        public bool IsFirstActived { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MainViewModel()
        {
            FooterLoadingVisibility = Visibility.Collapsed;
            NoItemHintVisibility = Visibility.Collapsed;
            NoNetworkHintVisibility = Visibility.Collapsed;
            FooterReloadVisibility = Visibility.Collapsed;
            EndVisibility = Visibility.Collapsed;
            IsRefreshing = true;
            ShowDownloadsUC = false;

            SelectedIndex = -1;
            Tabs = new ObservableCollection<string>();

            DataVM = new ImageDataViewModel(this,
                new ImageService(Request.GetNewImages, NormalFactory));
        }

        private ImageDataViewModel CreateOrCacheDataVm(int index)
        {
            ImageDataViewModel vm = null;
            if (_vms.ContainsKey(index))
            {
                vm = _vms[index];
            }

            if (vm == null)
            {
                switch (index)
                {
                    case NEW_INDEX:
                        vm = new ImageDataViewModel(this, new ImageService(Request.GetNewImages, NormalFactory));
                        break;
                    case FEATURED_INDEX:
                        vm = new ImageDataViewModel(this, new ImageService(Request.GetFeaturedImages, FeaturedFactory));
                        break;
                    case RANDOM_INDEX:
                        vm = new RandomImagesDataViewModel(this, new RandomImageService(NormalFactory));
                        break;
                }

                if (vm != null)
                {
                    _vms[index] = vm;
                }
            }

            return vm;
        }

        private async Task SearchByKeywordAsync()
        {
            var searchService = new SearchImageService(NormalFactory, SearchKeyword);

            var list = INDEX_TO_NAME.Select(s => s.Value).ToList();
            list.Add(SearchKeyword.ToUpper());

            Tabs = new ObservableCollection<string>(list);

            SelectedIndex = Tabs.Count - 1;
            DataVM = new SearchResultViewModel(this, searchService);

            _vms[SelectedIndex] = DataVM;

            await RefreshListAsync();
        }

        private async Task RefreshListAsync()
        {
            IsRefreshing = true;
            await DataVM.RefreshAsync();

            if (SelectedIndex == NEW_INDEX && AppSettings.Instance.EnableTodayRecommendation)
            {
                await InsertTodayWallpaperAsync();
            }

            IsRefreshing = false;
        }

        private async Task InsertTodayWallpaperAsync()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");

            if (DataVM.DataList.Count > 0 && DataVM.DataList[0].Image.ID != date)
            {
                var image = UnsplashImageFactory.CreateTodayImage();
                var imageItem = new ImageItem(image);
                DataVM.DataList.Insert(0, imageItem);
                imageItem.Init();
                await imageItem.DownloadBitmapForListAsync();
            }
        }

        public void Activate(object param)
        {
            var task = HandleLaunchArg(param as string);

            if (DeviceHelper.IsDesktop)
            {
                //var key = (string)App.Current.Resources["CoachKey"];
                //if (!LocalSettingHelper.HasValue(key))
                //{
                //    LocalSettingHelper.AddValue(key, true);
                //    var uc = new TipsControl();
                //    var task2 = PopupService.Instance.ShowAsync(uc);
                //}
            }
        }

        private async Task HandleLaunchArg(string arg)
        {
            if (arg == Value.SEARCH)
            {
                ShowSearchBar = true;
            }
            else if (arg == Value.DOWNLOADS)
            {
                ShowDownloadsUC = true;
            }
            else
            {
                var queryStr = QueryString.Parse(arg);
                var action = queryStr[Key.ACTION_KEY];
                if (!queryStr.Contains(Key.FILE_PATH_KEY))
                {
                    return;
                }
                var filePath = queryStr[Key.FILE_PATH_KEY];
                if (filePath != null)
                {
                    switch (action)
                    {
                        case Value.SET_AS:
                            await WallpaperSettingHelper.SetAsBackgroundAsync(await StorageFile.GetFileFromPathAsync(filePath));
                            break;
                        case Value.VIEW:
                            await Launcher.LaunchFileAsync(await StorageFile.GetFileFromPathAsync(filePath));
                            break;
                    }
                }
            }
        }

        public void Deactivate(object param)
        {
        }

        public void OnLoaded()
        {
            SelectedIndex = NEW_INDEX;
            Tabs = new ObservableCollection<string>(INDEX_TO_NAME.Select(s => s.Value).ToList());
        }
    }
}