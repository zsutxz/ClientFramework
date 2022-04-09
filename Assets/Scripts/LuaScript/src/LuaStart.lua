--- lua入口
--print('测试lua入口')
require("Define")
require("HotFix")
require("Main")
HotFix.HotFixUpdate()
Main.Init()