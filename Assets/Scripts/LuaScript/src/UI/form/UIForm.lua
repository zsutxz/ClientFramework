--- UI窗体基类

UIForm={}

function UIForm :New(name)
    local subTbale = {}
    self.__index = self
    setmetatable(subTbale,self)   -- 设置元方法
    subTbale.Instance = function()  --实例
        return subTbale
    end
    subTbale.name =name  --窗体名称
    subTbale.gameobject =nil --窗体 gameobject
    subTbale.transform =nil  --窗体 transform
    subTbale.GetChildByName = function(childName)  -- 查询子类方法
        local child = TransformHelper.GetChild(subTbale.transform,childName)
        return child
    end
    return subTbale
end
