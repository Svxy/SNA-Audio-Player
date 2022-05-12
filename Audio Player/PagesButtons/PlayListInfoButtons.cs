using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Audio_Player
{

    public partial class MainWindow : Window
    {

        private void RenamePL_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //make the textbox with the playlist name editable
            PlayListName_TB.IsReadOnly = false;
            PlayListName_TB.IsHitTestVisible = true;
            PlayListName_TB.BorderThickness = new Thickness(1);
            PlayListName_TB.CaretBrush = Brushes.White;
            PlayListName_TB.CaretIndex = PlayListName_TB.Text.Length;
            PlayListName_TB.Focus();
            SettPopup.IsOpen = false;
        }

        private void PlayListName_TB_KeyDown(object sender, KeyEventArgs e)
        {
            //remove the ability to edit the textbox with the name (these names are updated)
            if (e.Key == Key.Enter && PlayListName_TB.IsHitTestVisible)
            {
                PlayListName_TB.IsHitTestVisible = false;
                PlayListName_TB.IsReadOnly = true;
                PlayListName_TB.BorderThickness = new Thickness(0);
                PlayListName_TB.IsReadOnlyCaretVisible = false;
            }
        }

        private void PlayPlayList_Click(object sender, MouseButtonEventArgs e)
        {
            CurrentList = new List<Audio>((PListInfo.DataContext as PlayList).AudioList); //assign the current playlist the list of playlist songs
            SetVisibleMain(); //switch panel
            PlayPLAnimate(); //animate the switch
            Play.ItemsSource = CurrentList;
        }

        private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //display settings menu
            if (!SettPopup.IsOpen)
                SettPopup.IsOpen = true;
            else
                SettPopup.IsOpen = false;
        }


        private void RemovePL_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SettPopup.IsOpen = false;
            MyDialogWindow dialog = new MyDialogWindow();
            dialog.TX.Text = "Delete this playlist?";
            dialog.Owner = this;
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                playLists.Remove((sender as TextBlock).DataContext as PlayList); //delete playlist
                PL_ListBox.ItemsSource = new List<PlayList>(playLists); //refresh playlist window
                SetVisiblePlayListsControl(); //display playlist window
                CenterAnim(PListControl);
            }
        }

        private void PlayListAddAudio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //create a window for adding songs
            AddAudioWind NewWind = new AddAudioWind();
            NewWind.Owner = Application.Current.MainWindow;
            this.Opacity = 0.2;
            NewWind.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            NewWind.Index = playLists.IndexOf((sender as TextBlock).DataContext as PlayList); //pass the playlist data to the new window

            NewWind.ListOfAudio.ItemsSource = mainPL.AudioList.Where(x => playLists[NewWind.Index].AudioList.Count
                (s => s.DirectoryName == x.DirectoryName && s.Name == x.Name) == 0); // display only songs that are not in the playlist
            NewWind.Show();
        }

        private void RemoveAudioFromPlayList_Click(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Delete this audio file? ", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int ind = playLists.IndexOf(PListInfo.DataContext as PlayList); //get song index
                playLists[ind].AudioList.Remove((sender as TextBlock).DataContext as Audio); //remove from playlists
                playLists[ind].GetTime(); //recalculate time
                PListInfo.DataContext = null;
                PListInfo.DataContext = playLists[ind]; //refresh playlist window
            }
        }

        private void PlayPLAnimate() //play button animation for playlist
        {
            DoubleAnimation Anim = new DoubleAnimation();
            Anim.Completed += delegate
            {
                if (CurrentList.Count > 0)
                {
                    ChangeAudio(CurrentList[0]); // after the end of the animation, start playing from the first song of the playlist
                }
            };
            Anim.From = 0;
            Anim.To = 1;
            Anim.Duration = new Duration(TimeSpan.FromSeconds(1));
            PlayGrid.BeginAnimation(OpacityProperty, Anim);
        }

    }

}