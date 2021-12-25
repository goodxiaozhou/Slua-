import "UnityEngine"
if not UnityEngine.GameObject or not  UnityEngine.UI then
    error("Click Make/All to generate lua wrap file")
end

function IsNull(obj)
    if obj==nil or obj:Equals(nil) then
        return true
    end
    return false
end


function main()
    -- 创建Cube对象
    --local cube = GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube)
    --local pos = Vector3(10,10,10)+Vector3(1,1,1)
	--cube.transform.position = pos
    
    Canvas=GameObject.Find("Canvas").transform
    --初始化类别名
    require("InitClass")
    --初始化道具信息
    require("PlayerData")
    require("ItemData")
    --玩家信息
    --1.单机游戏 本地存储 1.PlayerPrefs 2.json或者2进制
    --2.网络游戏服务器获取
    PlayerData:Init()
    --print(PlayerData.items[1].num)--测试
    require("BasePanel")
    require("MainPanel")
    require("BagPanel")
    require("ItemGrid")
    MainPanel:ShowMe()
end


