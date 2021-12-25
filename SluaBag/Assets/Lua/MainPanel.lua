BasePanel:subClass("MainPanel")
MainPanel.name="MainPanel"



--实例化面板对象，为面板处理对应逻辑
function MainPanel : Init()
    if IsNull(self.PanelObj) then
        self.base.Init(self)
        --找到对应控件
        --为控件加上监听
        self:GetControl("btnRole","Button").onClick:AddListener(function()
            self:BtnRoleClick()
        end)     
    end
    
end
function MainPanel:BtnRoleClick()
    BagPanel:ShowMe()
end