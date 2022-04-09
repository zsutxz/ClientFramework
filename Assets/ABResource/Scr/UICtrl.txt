--- UI控制器基类

UICtrl ={}
--获取对象
function UICtrl : New(form)
    local subTable = {}
    self.__index = self
    setmetatable(subTable,self)  --设置元方法
    subTable.form = form
    subTable.Instance = function()  --实例方法
        return  subTable
    end
    subTable.CloseForm = function()  --关闭窗体
        UIMgr:CloseForm(subTable.form.name)
    end
    subTable.OpenForm =function(formName)  --打开窗体
        UIMgr:OpenForm(formName)
    end
    subTable.AddButtonListener=function(btn,delegate)  --添加按钮事件
        UIHelper.AddButtonListenser(btn,delegate)
    end
    subTable.RemoveButtonListener =function(btn,delegate)  --移除按钮单个事件
        UIHelper.RemoveButtonListenser(btn,delegate)
    end
    subTable.RemoveButtonAllListener =function(btn)   --移除按钮全部事件
        UIHelper.RemoveAllButtonListenser(btn)
    end
    return subTable
end
