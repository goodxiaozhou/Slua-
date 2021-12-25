local bagType={Equip=1,Item=2,Gem=3}--1装备 2道具 3宝石
BagPanel={}
BasePanel:subClass("BagPanel")
--c成员变量
BagPanel.name="BagPanel"
BagPanel.Content=nil
BagPanel.bagType=nil
BagPanel.items={}--当前显示的格子

--成员方法
function BagPanel:Init ()
    if IsNull(self.PanelObj) then
        self.base.Init(self)
        self.content=self.PanelObj.transform:Find("svBag"):Find("Viewport"):Find("Content")
        --为控件加逻辑
        self:GetControl('btnClose','Button').onClick:AddListener(function () self:HideMe()  end)
    self:GetControl('togEquip','Toggle').onValueChanged:AddListener(function (value)
        if(value) then
            self:ChangeType(bagType.Equip)
        end
    end)
    self:GetControl('togItem','Toggle').onValueChanged:AddListener(function (value)
        if(value) then
            self:ChangeType(bagType.Item)
        end

    end)
    self:GetControl('togGem','Toggle').onValueChanged:AddListener(function (value)
        if(value) then
            self:ChangeType(bagType.Gem)
        end
    end)
            end
    
                 end

function BagPanel:ShowMe()
    self.base.ShowMe(self)
    self:ChangeType(bagType.Equip)
   
end

function BagPanel:ChangeType(type)
    --当前背包状态与要切换的状态不同时 才会切换
    if self.bagType==type then
        return;
    end
    self.bagType=type
    --把老的格子删掉
    for i = 1, #self.items do
        self.items[i]:Destroy()
    end
    self.items={}
    --根据新格子创建
    local nowItem=PlayerData.bag[type]
    for i = 1, #nowItem do
        local grid=ItemGrid:new()
        grid:Init(self)
        grid:InitData(nowItem[i])
        table.insert(self.items,grid)
    end    
end