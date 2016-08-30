using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LazyLoadingImages
{
    [Activity(Label = "LazyLoadingImages", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        List<RootObject> listContents;
        ListView listView;
        static readonly int INITIAL_SELECTION = 4;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ListView);
            populateList();
        }

        public void initializeList()
        {
            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
            {
                int i = e.Position;

                Intent intent = new Intent(this, typeof(fullReceipe));
                intent.PutExtra("MyItem", JsonConvert.SerializeObject(listContents[e.Position]));
                this.StartActivity(intent);
            };
            listView.Adapter = new myAdapter(this, listContents);// setting adapter
            // Important - Set the ChoiceMode
            listView.ChoiceMode = ChoiceMode.Single;
        }

        public void populateList()
        {
            WebClient c = new WebClient();
            c.DownloadStringAsync(new Uri("https://raw.githubusercontent.com/raywenderlich/recipes/master/Recipes.json"));
            c.DownloadStringCompleted += C_DownloadStringCompleted;
        }

        private void C_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rootObj = JsonConvert.DeserializeObject<List<RootObject>>(e.Result);

            listContents = new List<RootObject>();

            foreach (var item in rootObj)
            {
                listContents.Add(new RootObject { name = item.name, imageURL = item.imageURL, ingredients = item.ingredients, steps = item.steps });
            }
            initializeList();
            listView.SetItemChecked(INITIAL_SELECTION, true);
        }
    }
}

