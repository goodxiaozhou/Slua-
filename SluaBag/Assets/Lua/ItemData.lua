
--首先吧Json表冲AB包加载出来
local txt=ABMgr.GetInstance():LoadRes("json","ItemData",TextAsset )

--获取文本信息
local itemList=Json.decode(txt.text)
print(itemList)
--加载出来是一个数组数据结构需要转存
--热和地方使用采用全局变量
--一张用来存储全局道具信息的表，键值对形式ID：信息
ItemData={}
for _, value in pairs(itemList) do
    ItemData[value.id]=value
end
