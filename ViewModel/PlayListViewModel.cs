using System.Collections.ObjectModel;
using Interface_MediaPlayer.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using Microsoft.Win32;

namespace Interface_MediaPlayer.ViewModel
{
    public class PlayListViewModel : ObservableObject
    {
        private Model.Media? _midiaAtual;
        private Model.Media _selectMedia;
        private Uri _source;
        public ObservableCollection<Media> playList { get; set; }
        public RelayCommand Pausex { get; set; }
        public RelayCommand Playx { get; set; }
        public RelayCommand Stopx { get; set; }
        public RelayCommand Openx { get; set; }
        public RelayCommand Selectx { get; set; }
        public RelayCommand Deletarx { get; set; }
        public RelayCommand Sairx { get; set; }
        private void PauseCMD()
        {
            MidiaAtual.MediaState = "Paused";
            WeakReferenceMessenger.Default.Send(this);
            UpdateButtons();
        }
        private void PlayCMD()
        {
            MidiaAtual.MediaState = "Playing";
            Source = this.MidiaAtual.Source;
            WeakReferenceMessenger.Default.Send(this);
            UpdateButtons();
        }
        private void StopCMD()
        {
            MidiaAtual.MediaState = "Stopped";
            WeakReferenceMessenger.Default.Send(this);
            UpdateButtons();
        }
        private void OpenCMD()
        {
            string[] paths, files;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == true)
            {
                files = ofd.SafeFileNames;
                paths = ofd.FileNames;
                for (int x = 0; x < files.Length; x++)
                {
                    Model.Media midia = new Model.Media(files[x], paths[x]);
                    playList.Add(midia);
                    MidiaAtual = midia;
                }
                PlayCMD();
            }
        }
        private void SelectCMD()
        {
            MidiaAtual = SelectMedia;
            PlayCMD();
        }
        private void DeletarCMD()
        {
            if (SelectMedia == MidiaAtual)
            {
                StopCMD();
                MidiaAtual = null;
            }
            playList.Remove(SelectMedia);
        }
        private void SairCMD()
        {
            System.Windows.Application.Current.Shutdown();
        }
        private bool CanPauseCMD()
        {
            if (MidiaAtual != null)
            {
                return MidiaAtual.MediaState == "Playing";
            }
            return false;
        }
        private bool CanPlayCMD()
        {
            if (MidiaAtual != null)
            {
                return MidiaAtual.MediaState != "Playing";
            }
            return false;
        }
        private bool CanStopCMD()
        {
            if (MidiaAtual != null)
            {
                return MidiaAtual.MediaState != "Stopped";
            }
            return false;
        }
        private bool CanSelectCMD()
        {
            if (SelectMedia != null && SelectMedia != this.MidiaAtual)
                return true;
            return false;
        }
        private bool CanDeletarCMD()
        {
            return SelectMedia != null;
        }
        private void UpdateButtons()
        {
            Pausex.NotifyCanExecuteChanged();
            Playx.NotifyCanExecuteChanged();
            Stopx.NotifyCanExecuteChanged();
            Deletarx.NotifyCanExecuteChanged();
            Selectx.NotifyCanExecuteChanged();
        }
        private void NextMedia()
        {
            int index = 0;
            while (MidiaAtual != playList[index])
                index++;
            MidiaAtual = playList[(index + 1) % playList.Count];
        }
        public Model.Media? MidiaAtual
        {
            get { return _midiaAtual; }
            set
            {
                SetProperty(ref _midiaAtual, value);
                UpdateButtons();
            }
        }
        public Model.Media SelectMedia
        {
            get { return _selectMedia; }
            set
            {
                SetProperty(ref _selectMedia, value);
                UpdateButtons();
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
        public PlayListViewModel()
        {
            Pausex = new RelayCommand(PauseCMD, CanPauseCMD);
            Playx = new RelayCommand(PlayCMD, CanPlayCMD);
            Stopx = new RelayCommand(StopCMD, CanStopCMD);
            Openx = new RelayCommand(OpenCMD);
            Selectx = new RelayCommand(SelectCMD, CanSelectCMD);
            Deletarx = new RelayCommand(DeletarCMD, CanDeletarCMD);
            Sairx = new RelayCommand(SairCMD);
            playList = new ObservableCollection<Media>();
            WeakReferenceMessenger.Default.Register<MainWindow>(this, (r, m) =>
            {
                NextMedia();
                PlayCMD();
            });
        }
    }
}