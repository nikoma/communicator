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
        private string _path; 
        public Emoticons(string path)
        {
            this._path = path;
            this.LoadDefault();
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

        public void LoadDefault()
        {
            _emoticons.Clear();
            _emoticons.Add(new Emoticon(":IN:", _path + "messageIn.png", 16, 16));
            _emoticons.Add(new Emoticon(":OUT:", _path + "messageOut.png", 16, 16));
            _emoticons.Add(new Emoticon(":D", _path+"emoticon_grin.png", 16,16));
            _emoticons.Add(new Emoticon(":-D", _path + "emoticon_grin.png", 16, 16));
            _emoticons.Add(new Emoticon(":grin:", _path + "emoticon_grin.png", 16, 16));
            _emoticons.Add(new Emoticon(":)", _path + "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon(":-)", _path + "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon(":smile:", _path + "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon("^_^", _path + "emoticon_smile.png", 16, 16));
            _emoticons.Add(new Emoticon(":(", _path + "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":-(", _path + "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":sad:", _path + "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":o", _path + "emoticon_surprised.png", 16, 16));
            _emoticons.Add(new Emoticon(":-o", _path + "emoticon_surprised.png", 16, 16));
            _emoticons.Add(new Emoticon(":eek:", _path + "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon("8O", _path + "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon("8-O", _path + "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon(":shock:", _path + "emoticon_surprised.png", 16, 16));
            _emoticons.Add(new Emoticon("8)", _path + "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon("8-)", _path + "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon(":cool:", _path + "emoticon_waii.png", 16, 16));
            _emoticons.Add(new Emoticon(":'(", _path + "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":'-(", _path + "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(";-(", _path + "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(";(", _path + "emoticon_unhappy.png", 16, 16));
            _emoticons.Add(new Emoticon(":lol:", _path + "emoticon_happy.png", 16, 16));
            _emoticons.Add(new Emoticon("xD", _path + "emoticon_happy.png", 16, 16));
            _emoticons.Add(new Emoticon("XD", _path + "emoticon_happy.png", 16, 16));
            _emoticons.Add(new Emoticon(":wink:", _path + "emoticon_wink.png", 16, 16));
            _emoticons.Add(new Emoticon(";)", _path + "emoticon_wink.png", 16, 16));
            _emoticons.Add(new Emoticon(";-)", _path + "emoticon_wink.png", 16, 16));
            _emoticons.Add(new Emoticon(":evil:", _path + "emoticon_evilgrin.png", 16, 16));
            _emoticons.Add(new Emoticon("B-)", _path + "emoticon_evilgrin.png", 16, 16));
            _emoticons.Add(new Emoticon(":p", _path + "emoticon_tongue.png", 16, 16));
            _emoticons.Add(new Emoticon(":-p", _path + "emoticon_tongue.png", 16, 16));
            _emoticons.Add(new Emoticon(":-p", _path + "emoticon_tongue.png", 16, 16));


        }
       
    }

    
}
