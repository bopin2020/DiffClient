using DiffClient.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Utility
{
    internal sealed class Help
    {
        /// <summary>
        /// medium integrity get the spcified process fullpath
        /// </summary>
        internal sealed class QueryProcessFullPath
        {
            const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
            const int SYNCHRONIZE = 0x00100000;

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr OpenProcess([In] int dwDesiredAccess, [In] bool bInheritHandle, [In] int dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] int dwFlags, [Out] StringBuilder lpExeName, ref int lpdwSize);

            public static string GetPath(int pid)
            {
                IntPtr hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION | SYNCHRONIZE, false, pid);
                if (hProcess == IntPtr.Zero)
                {
                    return string.Empty;
                }
                StringBuilder sb = new StringBuilder(4096);
                int size = 4096;
                if(QueryFullProcessImageName(hProcess,0,sb,ref size))
                {
                    return sb.ToString();
                }
                return string.Empty;
            }

            public async Task<string> GetPathAsync(int pid)
            {
                return await Task.Run(async () =>
                {
                    await Task.Delay(100);
                    return GetPath(pid);
                });
            }
        }
    
        internal static List<ProcessEntry> GetProcesses()
        {
            var tmp = new List<ProcessEntry>();
            int index = 0;
            foreach (var item in Process.GetProcesses())
            {
                Func<string> call = () => QueryProcessFullPath.GetPath(item.Id);
                tmp.Add(new ProcessEntry()
                {
                    Index = index,
                    Pid = item.Id,
                    PPid = item.SessionId,
                    Name = item.ProcessName,
                    FullName = call.EnterGuard() == null ? string.Empty : call.EnterGuard()
                });
                index++;
            }
            return tmp;
        }
    }
}
