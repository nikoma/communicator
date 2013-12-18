using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Remwave.Client
{

    public class Emoticon
    {

        string _Tag;

        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        string _Filename;

        public string Filename
        {
            get { return _Filename; }
            set { _Filename = value; }
        }

        int _Width;

        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        int _Height;

        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        
        public Emoticon(string tag, string key, int width, int height)
        {
            _Tag = tag;
            _Filename = key;
            _Width  = width;
            _Height = height;
        }
    }

    public class Emoticons 
    {
        private List<Emoticon> _emoticons = new List<Emoticon>();

        public Emoticons()
        {

        }
        public void Add(Emoticon item)
        {
            _emoticons.Add(item);
        }
        public List<Emoticon> List
        {
            get { return _emoticons; }
            set { _emoticons = value; }
        }

        public void LoadDefault(ImageList source)
        {

            _emoticons.Add(new Emoticon(":D", "emoticon_grin.png", 16,16));
            _emoticons.Add(new Emoticon(":-D", "emoticon_grin.png", 16, 16));
            _emoticons.Add(new Emoticon(":grin:", "emoticon_grin.png", 16, 16));
            _emoticons.Add(new Emoticon(":)", "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon(":-)", "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon(":smile:", "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon("^_^", "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon(":(", "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":-(", "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":sad:", "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":o", "emoticon_surprised.png", 16, 16));
            _emoticons.Add(new Emoticon(":-o", "emoticon_surprised.png", 16, 16));
            _emoticons.Add(new Emoticon(":eek:", "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon("8O", "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon("8-O", "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon(":shock:", "emoticon_surprised.png", 16, 16));
            _emoticons.Add(new Emoticon("8)", "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon("8-)", "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon(":cool:", "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon(":'(", "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":'-(", "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(";-(", "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(";(", "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":lol:", "emoticon_happy.png", 16, 16));
            _emoticons.Add(new Emoticon("xD", "emoticon_happy.png", 16, 16));
            _emoticons.Add(new Emoticon("XD", "emoticon_happy.png", 16, 16));
            _emoticons.Add(new Emoticon(":wink:", "emoticon_wink.png", 16, 16));
            _emoticons.Add(new Emoticon(";)", "emoticon_wink.png", 16, 16));
            _emoticons.Add(new Emoticon(";-)", "emoticon_wink.png", 16, 16));
            _emoticons.Add(new Emoticon(":evil:", "emoticon_evilgrin.png", 16, 16));
            _emoticons.Add(new Emoticon("B-)", "emoticon_evilgrin.png", 16, 16));
            _emoticons.Add(new Emoticon(":p", "emoticon_tongue.png", 16, 16));
            _emoticons.Add(new Emoticon(":-p", "emoticon_tongue.png", 16, 16));
            _emoticons.Add(new Emoticon(":-p", "emoticon_tongue.png", 16, 16));


        }
       
    }

    
}
