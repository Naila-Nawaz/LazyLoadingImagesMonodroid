using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Com.Androidquery;
using Android.Graphics;

namespace LazyLoadingImages
{
    public class myAdapter: BaseAdapter<RootObject>
    {
        readonly Activity context;
        List<RootObject> objects = new List<RootObject>();
        View view;

        public myAdapter(Activity context, List<RootObject> objects)
        {
            this.context = context;
            this.objects = objects;
        }

        public override RootObject this[int position]
        {
            get { return objects[position]; }
        }

        public override int Count
        {
            get { return objects.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            RootObject obj = objects[position];
            view = convertView;

            if (view == null) // no view to re-use, create new
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.listItem, null);
            }
            
            //using AQuery library for downloading images asynchronously
            AQuery aq = new AQuery(view);

            TextView tvName = view.FindViewById<TextView>(Resource.Id.tvName);
            tvName.Text = obj.name;

            Bitmap imgLoading = aq.GetCachedImage(Resource.Drawable.img_loading);

            if (aq.ShouldDelay(position, view, parent, obj.imageURL))
            {
                ((AQuery)aq.Id(Resource.Id.Image)).Image(imgLoading, 0.75f);
            }
            else
            {
                ((AQuery)aq.Id(Resource.Id.Image)).Image(obj.imageURL, true, true, 0, 0, imgLoading, 0, 0.75f);
            }

            return view;
        }
    }
}