using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace csutils
{
    /// <summary>
    /// C#实现阻止关闭显示器和系统待机
    /// https://www.cnblogs.com/TianFang/archive/2012/10/12/2721883.html
    /// 要实现下载时阻止程序休眠，则有两种实现方式：
    /// 1,下载期间起计时器定期执行ResetSleepTimer函数
    /// 2,下载开始时执行PreventSleep函数，下载结束后执行ResotreSleep函数。
    /// 
    /// 阻止屏幕保护程序的运行:
    /// https://blog.csdn.net/zjerryj/article/details/4618485
    /// 调用Windows API函数SystemParametersInfo()来关闭屏幕保护程序，
    /// 播放结束后再重新打开。
    /// </summary>
    public class SystemSleepManagement
    {
        //定义API函数
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(ExecutionFlag flags);

        [Flags]
        enum ExecutionFlag : uint
        {
            System = 0x00000001,
            Display = 0x00000002,
            Continus = 0x80000000,
        }

        /// <summary>
        ///阻止系统休眠，直到线程结束恢复休眠策略
        /// </summary>
        /// <param name="includeDisplay">是否阻止关闭显示器</param>
        public static void PreventSleep(bool includeDisplay = false)
        {
            if (includeDisplay)
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display | ExecutionFlag.Continus);
            else
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Continus);
        }

        /// <summary>
        ///恢复系统休眠策略
        /// </summary>
        public static void ResotreSleep()
        {
            SetThreadExecutionState(ExecutionFlag.Continus);
        }

        /// <summary>
        ///重置系统休眠计时器
        /// </summary>
        /// <param name="includeDisplay">是否阻止关闭显示器</param>
        public static void ResetSleepTimer(bool includeDisplay = false)
        {
            if (includeDisplay)
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display);
            else
                SetThreadExecutionState(ExecutionFlag.System);
        }
    }
}
