using System.Collections.Generic;

namespace LazyLoadingImages
{
    public class Ingredient
    {
        public string quantity { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }
    public class RootObject
    {
        public string name { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public List<string> steps { get; set; }
        public List<int> timers { get; set; }
        public string imageURL { get; set; }
        public string originalURL { get; set; }
        public string imageUrl { get; internal set; }
    }
}