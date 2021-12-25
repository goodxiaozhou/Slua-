PlayerData={}
--玩家信息，只包含背包信息
PlayerData.equips={}
PlayerData.items={}
PlayerData.gems={}
PlayerData.bag={[1]=PlayerData.equips,[2]=PlayerData.items,[3]=PlayerData.gems}
--为玩家数据写一个初始化方法

function PlayerData : Init()
    table.insert(self.equips,{id = 1 , num = 1})
    table.insert(self.equips,{id = 2 , num = 1})
    table.insert(self.equips,{id=7,num=1})
    table.insert(self.equips,{id=8,num=1})
    table.insert(self.equips,{id=9,num=1})
    table.insert(self.equips,{id=10,num=1})
    table.insert(self.items,{id=3,num=10})
    table.insert(self.items,{id=4,num=20})
    table.insert(self.gems, {id=5,num=50})
    table.insert(self.gems,{id=6,num=100})
    
end



