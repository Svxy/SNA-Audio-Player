using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Audio_Player
{

    public partial class MainWindow : Window
    {

        private void RemovePath_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete this path?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (mainPL.AudioList.SequenceEqual(CurrentList))
                {
                    string r = ((Button)sender).Content.ToString(); // get the directory to delete
                    mainPL.AudioList = mainPL.AudioList.Where(x => x.DirectoryName != r).ToList(); //clean the main playlist from the files of this directory
                    Paths.Remove(((Button)sender).Content.ToString()); //remove a directory from the list
                    ListPaths.ItemsSource = new List<string>(Paths); // update directory list
                    Play.ItemsSource = mainPL.AudioList; //update playlist page
                    CurrentList = mainPL.AudioList; //update playlist
                    CurrentIndex = 0; //start playback from the beginning
                }
                else
                {
                    // the same, only without updating the playlist page and playlist
                    string r = ((Button)sender).Content.ToString(); 
                    mainPL.AudioList = mainPL.AudioList.Where(x => x.DirectoryName != r).ToList();
                    Paths.Remove(((Button)sender).Content.ToString());
                    ListPaths.ItemsSource = new List<string>(Paths);
                }
                mainPL.GetTime();
            }
        }

        private void AddPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog Dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (Dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!Paths.Contains(Dialog.SelectedPath)) //if there is no such path in the list
                {
                    Paths.Add(Dialog.SelectedPath); // add path
                    ListPaths.ItemsSource = new List<string>(Paths); // display a list of paths
                    if (mainPL.AudioList.SequenceEqual(CurrentList)) // check which playlist is displayed, if the main one...
                    {
                        mainPL.AudioList.AddRange(load_audios.GetMusic(Dialog.SelectedPath)); // update playlist
                        Play.ItemsSource = new List<Audio>(mainPL.AudioList); // update the current playlist
                        CurrentList = mainPL.AudioList; // update current playlist data
                        CurrentIndex = 0; //after the end of the song, playback will start from the beginning of the list.
                    }
                    else
                    {
                        mainPL.AudioList.AddRange(load_audios.GetMusic(Dialog.SelectedPath));  //if the playlist is custom - update only the main playlist and that's it
                    }
                    mainPL.GetTime();
                }
                else
                {
                    MessageBox.Show("This path already exists.");
                }
            }
        }

    }

}