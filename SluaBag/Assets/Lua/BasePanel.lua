    Object:subClass("BasePanel")--面向对象
    BasePanel.PanelObj=nil
    BasePanel.name=nil
    BasePanel.controls={}--模拟字典键为控件名值为控件


function BasePanel:Init()
    if self.PanelObj==nil then
        self.PanelObj=ABMgr.GetInstance():LoadRes("ui",self.name,GameObject)
        self.PanelObj.transform:SetParent(Canvas,false)
        --self.panelObj.transform:SetAsLastSibling()
        --找按钮并存储
        local allControls=self.PanelObj:GetComponentsInChildren(UIBehaviour)
        for i = 1, allControls.Length-1 do
            local controlName=allControls[i].name
            if string.find(controlName,'btn') ~=nil 
            or string.find(controlName,'img') ~=nil
            or string.find(controlName,'txt') ~=nil 
            or string.find(controlName,'tog') ~=nil
            or string.find(controlName,'sv') ~=nil
            then
                --为了确定得到的控件的类型
                local typeName=allControls[i]:GetType().Name
                --每个控件obj 建一张表 存obj下的全部控件
                --存储形式
                --{btnRole={Image=控件,Button=控件}}
                if self.controls[controlName] == nil then
                    self.controls[controlName] = {[typeName]=allControls[i]}
                else
                    self.controls[controlName][typeName]=allControls[i]
                end
            end
        end
    end
end
--得到控件根据控件名字
function BasePanel:GetControl(objName,typeName)
    if self.controls[objName]~=nil then
        local control=self.controls[objName]
        if control[typeName]~=nil then
            return control[typeName]
        end
    end
    return nil
end
function BasePanel:ShowMe()
    if self.PanelObj==nil then
        self:Init()
    else
        self.PanelObj:SetActive(true)
    end
end

function BasePanel:HideMe()
    self.PanelObj:SetActive(false)
end