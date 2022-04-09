--- lua框架启动入口

require("FormInit")
require("UICtrlMgr")

Main={}
local this = Main

function Main.Init()
    FormInit.Init()
    UICtrlMgr.Init()
end
