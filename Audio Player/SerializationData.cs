using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Audio_Player
{
    public partial class MainWindow : Window
    {
        private void DeserializeData()
        {
            // read data
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("P_S.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                    Paths = (List<string>)formatter.Deserialize(fs);
            }
            ListPaths.ItemsSource = new List<string>(Paths);

            using (FileStream fs = new FileStream("P_List.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                    mainPL.AudioList = (List<Audio>)formatter.Deserialize(fs);
            }

            using (FileStream fs = new FileStream("P_lists.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0) // if playlist list is not empty
                {
                    playLists = (List<PlayList>)formatter.Deserialize(fs); // rewriting data
                    mainPL.GetTime();
                    playLists[0] = mainPL; // rewrite the main playlist to the beginning
                }
                else
                {
                    mainPL.GetTime();
                    playLists.Add(mainPL); //add master playlist
                }
            }

            //update windows
            ListPaths.ItemsSource = new List<string>(Paths);
            PL_ListBox.ItemsSource = new List<PlayList>(playLists);
            Play.ItemsSource = new List<Audio>(mainPL.AudioList);
        }

        private void SerializeData()
        {
            //save paths, main playlist and playlists
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("P_S.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Paths);
            }
            using (FileStream fs = new FileStream("P_List.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, mainPL.AudioList);
            }
            using (FileStream fs = new FileStream("P_lists.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, playLists);
            }
        }

    }
}