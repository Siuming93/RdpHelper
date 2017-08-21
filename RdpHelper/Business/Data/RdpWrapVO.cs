using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpHelper.Business.Data
{
    class RdpWrapVO
    {
        public string id;

        public string screenMode = "2";
        public string desktopWidth;
        public string desktopHeight;
        public string server;
        public string consoleSession = "0";
        public string domain = "0";
        public string userName;
        public string password;

        /// <summary>
        /// 追加到尾部的内容
        /// </summary>
        public string tail;

        public static RdpWrapVO FakeVO(string id)
        {
            return new RdpWrapVO() { id = id};
        }
    }
}
