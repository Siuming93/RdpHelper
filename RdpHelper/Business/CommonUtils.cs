using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Crypt32;
using Les.Util;
using RdpHelper.Business.Data;

namespace RdpHelper.Business
{
    static class  CommonUtils
    {
        public const string RDP_FILE_TEMPLATE = "screen mode id:i:{0}\r\ndesktopwidth:i:{1}\r\ndesktopheight:i:{2}\r\nsession bpp:i:16\r\nfull address:s:{3}\r\nconnect to console:i:{4}\r\ncompression:i:1\r\nkeyboardhook:i:2\r\naudiomode:i:2\r\nredirectdrives:i:1\r\nredirectprinters:i:0\r\nredirectcomports:i:0\r\nredirectsmartcards:i:0\r\ndisplayconnectionbar:i:1\r\nautoreconnection enabled:i:1\r\ndomain:s:{5}\r\nusername:s:{6}\r\npassword 51:b:{7}\r\nauthentication level:i:0\r\nprompt for credentials:i:0\r\ndisable wallpaper:i:0\r\ndisable full window drag:i:1\r\ndisable themes:i:0\r\ndisable cursor setting:i:1\r\nbitmapcachepersistenable:i:1";
     
        public static void StartRdp(RdpWrapVO vo)
        {
            if (vo == null)
            {
                return;
            }
            var fullName = FileUtil.AssembleDirName + @"/rdp/" + vo.server + ".rdp";
            if (File.Exists(fullName))
            {
                Process process = new Process();
                process.StartInfo.FileName = fullName;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            else
            {
                MessageBox.Show(string.Format("rdp目录下文件{0}不存在, 请刷新重试", fullName), "警告");
            }
        }
        /// <summary>
        /// 创建Rdp文件,已存在的自动覆盖
        /// </summary>
        /// <param name="vo"></param>
        public static void CreateRdpFile(RdpWrapVO vo)
        {
            if (vo == null)
            {
                return;
            }
            var path = FileUtil.AssembleDirName + @"/rdp/";
            var fullName = path + vo.server + ".rdp";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
            }
            FileUtil.WriteToTxtFile(fullName, EncodeRdpProto(vo));
        }

        /// <summary>
        /// 拼装Rdp协议
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        public static string EncodeRdpProto(RdpWrapVO vo)
        {
            string screenMode = vo.screenMode;
            string desktopWidth = vo.desktopWidth;
            string desktopHeight = vo.desktopHeight;
            string server = vo.server;
            string consoleSession = vo.consoleSession;
            string domain = vo.domain;
            string userName = vo.userName;
            //密码项需要加密
            string password = DPAPI.Encrypt(vo.password).Replace("-", "");

            //todo 其他内容的拼装

            return string.Format(RDP_FILE_TEMPLATE, screenMode, desktopWidth, desktopHeight, server,
                 consoleSession, domain, userName, password);
        }
    }
}
