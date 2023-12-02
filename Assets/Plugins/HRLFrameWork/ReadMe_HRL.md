HRL is a C# framework for quick unity develop.

Quick Start 

Framework:
    Config:
        Config is based on odin inspector.
        To quick start config, paste the folder into script path, change the class about [language] to another class, like [level]
        Example Folder: Assets\Plugins\HRLFrameWork\Framework\Language\LanguageConfig


Need Plugins:

Odin Inspector
Dotween

More Recommend Plugins:
[Cinemachine]: Need [Post Processing]


代码规范：
    变量：
        公有变量：驼峰命名
        私有变量：小写+下划线
    函数：
        公有函数：驼峰命名
        私有函数：前缀加下划线，驼峰命名
    参数：
        小写+下划线
        如果与私有变量重名，则前缀+下划线
其他：
    Manager 管理类均继承单例，只能存在一个，后缀统一为 Manager，不可为Mgr
    Controller 为每个GameObject都可能拥有。

////////////////////////////////////////////////////////////////////
//                          _ooOoo_                               //
//                         o8888888o                              //
//                         88" . "88                              //
//                         (| ^_^ |)                              //
//                         O\  =  /O                              //
//                      ____/`---'\____                           //
//                    .'  \\|     |//  `.                         //
//                   /  \\|||  :  |||//  \                        //
//                  /  _||||| -:- |||||-  \                       //
//                  |   | \\\  -  /// |   |                       //
//                  | \_|  ''\---/''  |   |                       //
//                  \  .-\__  `-`  ___/-. /                       //
//                ___`. .'  /--.--\  `. . ___                     //
//              ."" '<  `.___\_<|>_/___.'  >'"".                  //
//            | | :  `- \`.;`\ _ /`;.`/ - ` : | |                 //
//            \  \ `-.   \_ __\ /__ _/   .-` /  /                 //
//      ========`-.____`-.___\_____/___.-`____.-'========         //
//                           `=---='                              //
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //
//         佛祖保佑       永无BUG     永不修改                       //
////////////////////////////////////////////////////////////////////