using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// lua转化txt工具
/// </summary>
public class LuaCopyEditor : Editor
{
    [MenuItem("SLua/Lua转化为txt文件")]
    public static void CopyLuaToTxt()
    {
        //计时
        DateTime beginTime= DateTime.Now;

        //建立/清空 新文件夹
        string newPath=Application.dataPath+"/LuaTxt/";
        if(!Directory.Exists(newPath))
            Directory.CreateDirectory(newPath);
        else
        {
            var oldTxts=Directory.GetFiles(newPath,"*.txt");
            for(int i=0;i<oldTxts.Length;++i)
            {
                File.Delete(oldTxts[i]);
            }
        }

        //得到Lua文件夹下的所有lua文件（包括子文件夹里的lua文件）
        string path=Application.dataPath+"/Lua/";
        //判断路径
        if(!Directory.Exists(path))
            return;
        var luas=new List<string>(16);
        foreach(var lua in Directory.GetFiles(path,"*.lua"))
        {
            luas.Add(lua);
        }
        var folders=new Stack<string>(4);
        foreach(string newFolderPath in Directory.GetDirectories(path))
        {
            var newFolder=newFolderPath+"/";
            folders.Push(newFolder);
        }
        while(folders.Count>0)
        {
            var folder=folders.Pop();
            foreach(string newFolder in Directory.GetDirectories(folder))
            {
                folders.Push(newFolder);
            }
            foreach(var lua in Directory.GetFiles(folder,"*.lua"))
            {
                luas.Add(lua);
            }
        }
        
        //将lua文件copy到新文件夹 并加上.txt
        string newFile;
        var newFileNames=new List<string>(luas.Count);
        for(int i=0;i<luas.Count;++i)
        {
            newFile=newPath+luas[i].Substring(luas[i].LastIndexOf("/")+1)+".txt";
            newFileNames.Add(newFile);
            File.Copy(luas[i],newFile);
        }
        //刷新
        AssetDatabase.Refresh();

        //将新文件打入ab包
        for (int i = 0; i < newFileNames.Count; i++)
        {
            AssetImporter importer=AssetImporter.GetAtPath(
                newFileNames[i].Substring(newFileNames[i].IndexOf("Assets")));
            if(importer != null)
            {
                importer.assetBundleName="lua";
            }
        }

        var time= DateTime.Now-beginTime;
        Debug.Log("完成Lua文件夹下.lua文件转.txt文件，用时："+time.TotalMilliseconds+"ms");
    }
}
