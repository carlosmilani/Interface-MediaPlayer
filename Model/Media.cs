using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Interface_MediaPlayer.Model
{
    public class Media : ObservableObject
    {
        private string _fileName;
        private Uri _source;
        private string _mediaState = "Stopped";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                SetProperty(ref _fileName, value);
            }
        }
        public Uri Source
        {
            get { return _source; }
            set
            {
                SetProperty(ref _source, value);
            }
        }
        public string MediaState
        {
            get { return _mediaState; }
            set
            {
                SetProperty(ref _mediaState, value);
            }
        }
        public Media(string file, string path)
        {
            this._fileName = file;
            this._source = new Uri(path);
        }
    }
}