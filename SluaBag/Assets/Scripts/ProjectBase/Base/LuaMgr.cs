using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SLua;


/// <summary>
///lua管理器
///保证lua解析器的唯一性
/// </summary>
public class LuaMgr : BaseManager<LuaMgr>
{
    
    private  LuaSvr _LuaSvr;
    public LuaSvr LuaSvr
    {
        get
        {
            return _LuaSvr;
        }
    }
   
    /// <summary>
    /// 初始化解析器函数
    /// </summary>
    public void Init()
    {
        if (_LuaSvr != null) return;
        else
        {
        _LuaSvr = new LuaSvr();
        //LuaSvr.mainState.loaderDelegate  += LuaLoader;
        LuaSvr.mainState.loaderDelegate  += ABLoader;
        }
    }   
    /// <summary>
    /// 执行lua脚本
    /// </summary>
    /// <param name="fileName"></param>
    public void DoLuaFile(string fileName)
    {
        if (_LuaSvr == null)
        {
            Debug.LogWarning("解析器未初始化");
            return;
        }
        else
        {
            _LuaSvr.init(null, () => // 如果不用init方法初始化的话,在Lua中是不能import的
		{
            _LuaSvr.start(fileName); 
			
		});

        }
        
    }
    /// <summary>
    /// 读取自定义路径资源
    /// </summary>
    /// <param name="strFile"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static  byte[] LuaLoader(string strFile,ref string filePath)
    { 
        string filename = Application.dataPath + "/Lua/" + strFile.Replace('.', '/') + ".lua";
        if (File.Exists(filename))
        {
           
            return File.ReadAllBytes(filename);
        }
        else
        {
            Debug.Log("LuaLoader重定向失败，文件名为"+filePath);
        }
        
        return null;
    }
    /// <summary>
    /// 读取AB资源
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private byte[] ABLoader(string strFile,ref string filePath)
    {
       
        //加载AB包
        var luaAsset = ABMgr.GetInstance().LoadRes<TextAsset>("lua", strFile + ".lua");
        if (luaAsset != null)
        {
            return luaAsset.bytes;
        }
        else
        {
            Debug.Log("ABLoader重定向失败，文件名为"+filePath);
        }
        
        return null;
    }
}
