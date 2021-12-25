Object:subClass("ItemGrid")
--c成员变量
-- ItemGrid.obj=nil
-- ItemGrid.imgIcon=nil
-- ItemGrid.Number=nil

function ItemGrid:Init(father)
    self.obj=ABMgr.GetInstance():LoadRes("ui","ItemGrid",GameObject)
    --设置父对象
    self.obj.transform:SetParent(father.content,false)
    self.imgIcon=self.obj.transform:Find("imgIcon"):GetComponent("Image")
    self.Number=self.obj.transform:Find("Number"):GetComponent("Text")--找到了

    
end

--初始化格子信息
--data是外部传入的道具信息得到里面的ID和NUM
function ItemGrid:InitData(data)
    --通过ID读取配置表图标信息
    local itemData=ItemData[data.id]
    --根据名字加载图集在加载图集中图标
    local strs=string.split(itemData.icon,"_")
    local spriteAtlas=ABMgr.GetInstance():LoadRes(strs[1],strs[2],SpriteAtlas)
    --print(spriteAtlas)--得到图集成功
    --print( self.imgIcon.sprite)
    self.imgIcon.sprite=spriteAtlas:GetSprite(strs[3])
    self.Number.text=data.num
    -- body
end

function ItemGrid:Destroy()
    --print("执行Destory")
    GameObject.Destroy(self.obj)
    self.obj=nil
end