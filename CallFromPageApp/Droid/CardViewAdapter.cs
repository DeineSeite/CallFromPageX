using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Content;
using System.Threading;
using System.Collections.ObjectModel;
using System.Globalization;
using Android.Gms.Maps.Model;
using Android.Support.V7.Widget;
using CallFromPageApp.Droid;

namespace CallFromPageApp.Droid
{
    class CardViewAdapter : BaseAdapter<ContactNotificationItem>
    {
        string TAG = "X: " + typeof(CardView).Name;
        //private readonly BaseFragment _baseFragment;
        private readonly List<ContactNotificationItem> _notifications;
        //List<CategoryModel> selectedCategory;
        //List<CategoryModel> categories;
        //int _pageNumber = 1;
        //bool _isFav = false;
        //SqliteService conn;
        private Context _context;

        //private MarkerUrlBuilder _markerUrlBuilder;

        public CardViewAdapter(Context context)
        {
            _context = context;
            
            // conn = Utils.GetDatabaseService();
            // _baseFragment = context;
            //selectedCategory = conn.GetSubSelectedCategory();
            //_markerUrlBuilder = new MarkerUrlBuilder
            //{
            //   LanguageId = _baseFragment.CurrentLang.Id,
            //  MainCategoryId = BaseFragment.SelectedMainCategory.Id,
            // SubCategoriesList = selectedCategory.Select(x => x.Id).ToList()
            //};
            //categories = conn.GetDataList<CategoryModel>();
            _notifications = new List<ContactNotificationItem>();
            // this._isFav = isFav;
            /*if (!isFav)
            {
                var latLngBounds = _baseFragment.MainActivity.MapPage.LatLngBounds;
                var inBounds = _baseFragment.MainActivity.MapPage.ProductsInBounds
                    .Where(x => latLngBounds.Contains(new LatLng(
                        double.Parse(x.Lat, CultureInfo.InvariantCulture),
                        double.Parse(x.Lng, CultureInfo.InvariantCulture))
                        ));
                _notifications = new List<ProductMarkerModel>(Utils.SortProductsByDistance(inBounds));
            }*/
            // _baseFragment.ShowSpinner(_notifications.Count == 0);

            StartLoadAsync();
        }

        public void Add()
        {
            StartLoadAsync();
            //NotifyDataSetChanged();
        }

        private async void StartLoadAsync()
        {
            if (await LoadData())
            {
                // _baseFragment.ShowSpinner(false);
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                var inflater = LayoutInflater.From(_context);
                view = inflater.Inflate(Resource.Layout.row, parent, false);
                view.SetBackgroundColor(Color.White);
            }

            //var imageView = view.FindViewById<ImageView>(Resource.Id.mainImageView);
            var title = view.FindViewById<TextView>(Resource.Id.titleTextView);
            var adress = view.FindViewById<TextView>(Resource.Id.adressTextView);
            var dateTextView = view.FindViewById<TextView>(Resource.Id.dateTextView);
            var distanceLayout = view.FindViewById<LinearLayout>(Resource.Id.distanceLayout);

            title.Text = _notifications[position].Title;
            adress.Text = _notifications[position].Message;
            dateTextView.Text = _notifications[position].DateCreated.ToString();



            /* if ( !string.IsNullOrEmpty(_notifications[position].Distance))
             {
                 positionTextView.Text = _notifications[position].Distance + " km";
             }
             else
             {
                 distanceLayout.Visibility = ViewStates.Gone;
             }

             string imageUrl = "";
             if (!string.IsNullOrEmpty(_notifications[position].ProdImg))
             {
                 imageUrl = _notifications[position].ProdImg;
             }
             else
             {
                 //lat.low, long.low, lat.hig
                 CategoryModel catImage;
                 if (selectedCategory.Count != 0)
                 {
                     catImage = selectedCategory.FirstOrDefault(x => _notifications[position].Categories.Any(y => y == x.Id));
                 }
                 else
                 {
                     catImage = selectedCategory.FirstOrDefault(x => _notifications[position].Categories.Any(y => y == x.Id));

                 }
                 if (catImage != null)
                 {
                     imageUrl = Utils.RESOURCE_PATH + catImage.Icon;
                     imageView.SetBackgroundColor(Color.ParseColor(catImage.Color));
                 }
             }
             Picasso.With(_baseFragment.MainActivity).Load(imageUrl).Resize(60, 60).CenterInside().Into(imageView);
             if (_notifications.Count - 5 == position)
             {
                 ThreadPool.QueueUserWorkItem(async o => await LoadNextData());

             }*/
            return view;
        }

        private async Task<bool> LoadData()
        {
            var conn = Utils.GetDatabaseService();
            //IEnumerable<ContactNotificationItem> newNotifications;

            //conn.DeleteRows();

            /*ContactNotificationItem item = new ContactNotificationItem();
            item.Id = 1;
            item.Message = "new";
            item.Shown = true;
            item.Title = "set";
            item.DateCreated = new DateTime(2016, 12, 8, 16, 45, 00);

            _notifications.Add(item);

            item = new ContactNotificationItem();
            item.Id = 2;
            item.Message = "new 1";
            item.Shown = true;
            item.Title = "set 1";
            item.DateCreated = new DateTime(2016, 12, 8, 13, 45, 00);

            _notifications.Add(item);

            conn.InsertUpdateProductList<ContactNotificationItem>(_notifications);
            */

            _notifications.Clear();

            List<ContactNotificationItem> list = conn.GetDataList<ContactNotificationItem>().OrderByDescending(u => u.Id).ToList();

            _notifications.AddRange(list);

            NotifyDataSetChanged();



            /*if (_isFav)
            {
                var user = conn.GetDataList<UserModel>().FirstOrDefault();
                if (user == null) return true;
                newProducts = await RestApiService.GetFavorites(user.Id, _pageNumber);
                newProducts = Utils.SortProductsByDistance(newProducts);
            }
            else
            {
                var latLngBounds = _baseFragment.MainActivity.MapPage.LatLngBounds;
                if (latLngBounds != null)
                {
                    if (_notifications.Count < NohandicapLibrary.DefaultCountMarkersToLoad)
                    {
                        _markerUrlBuilder.PageNumber = 1;
                    }
                    else
                    {
                        _pageNumber++;
                        _markerUrlBuilder.PageNumber = _pageNumber;
                    }
                    _markerUrlBuilder.SetBounds(latLngBounds.Southwest.Latitude, latLngBounds.Southwest.Longitude,
                        latLngBounds.Northeast.Latitude, latLngBounds.Northeast.Longitude);


                }
                var position = _baseFragment.MainActivity.CurrentLocation;
                if (position != null)
                {
                    _markerUrlBuilder.SetMyLocation(position.Latitude, position.Longitude);
                }
                newProducts = await _markerUrlBuilder.LoadDataAsync();
                // if (position == null)
                // {
                 //    newProducts = newProducts.OrderBy(x => x.Name);
                // }

            }

            foreach (var product in newProducts)
            {
               _baseFragment.MainActivity.RunOnUiThread(() =>
                {
                    _notifications.Add(product);
                    NotifyDataSetChanged();
                });
            }

*/
            return true;

        }


        public override ContactNotificationItem this[int position] => _notifications[position];

        public override int Count => _notifications.Count;

        public override long GetItemId(int position) => (long)_notifications[position].Id;

    }
}
