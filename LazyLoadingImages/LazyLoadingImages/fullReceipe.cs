using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System.Net;
using Android.Graphics;
using Android.Text;

namespace LazyLoadingImages
{
    public class fullReceipe: Activity
    {
       
        ImageView iv;
        TextView tvIngredients;
        TextView tvSteps;
        RootObject myObj;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Getting the object from previous activity
            myObj = JsonConvert.DeserializeObject<RootObject>(Intent.GetStringExtra("MyItem"));
            setTitle(myObj.name);

            SetContentView(Resource.Layout.fullReceipe);

            //Getting the widgets
            iv = FindViewById<ImageView>(Resource.Id.iv);
            tvIngredients = FindViewById<TextView>(Resource.Id.tvIngredients);
            tvSteps = FindViewById<TextView>(Resource.Id.tvSteps);

            string ingredients = "";
            Toast.MakeText(this, "object:" + myObj.name, ToastLength.Long).Show();
            foreach (var item in myObj.ingredients)
            {
                ingredients = ingredients + "quantitiy: " + item.quantity;
                ingredients = ingredients + "\n" + "name: " + item.name;
                ingredients = ingredients + "\n" + "type: " + item.type + "\n" + "\n";

            }

            string steps = "";

            foreach (var item in myObj.steps)
            {
                steps = steps + item;
            }

            //For downloading image in background
            WebClient web = new WebClient();
            web.DownloadDataAsync(new Uri(myObj.imageURL));
            web.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted);

            //ADDING THE CONTENT
            tvIngredients.Text = ingredients.ToString();
            tvSteps.Text = steps;
        }

        void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                RunOnUiThread(() =>
                    Toast.MakeText(this, e.Error.Message, ToastLength.Short).Show());
            }
            else
            {
                Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);
                RunOnUiThread(() => //As you can't make changes in UI from a background thread
                {
                    iv.SetImageBitmap(bm);
                });
            }
        }

        public void setTitle(string t)
        {
            var title = new SpannableString(t);
            ActionBar.TitleFormatted = title;
        }
    }
}