---  窗体初始化
require("UIForm")
FormInit={}
local this =FormInit

function FormInit.Init()
    for i, v in pairs(UIFormNames) do
           require(tostring(v))
    end
end
