using System.Collections.Generic;
using System.IO;
using SFB;
using UnityEngine;

public class FileManager : MonoBehaviour {

    private string folderPath;

    public void OpenFileExplorer() {
        string[] path = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);
        if (path.Length == 0) return;
        folderPath = path[0];
        SetupUI.Instance.SetPath(folderPath);
        FillList();
    }

    private void FillList() {
        AudioManager.Instance.ClearSongs();
        List<FileInfo> fileInfos = new List<FileInfo>();
        
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        foreach (FileInfo file in dir.GetFiles("*.*")) {
            if (file.Extension == ".mp3" || file.Extension == ".ogg") fileInfos.Add(file);
        }

        SetupUI.Instance.SetNbSongLoad(fileInfos.Count);
        AudioManager.Instance.AddSongs(fileInfos);
        
    }
}
