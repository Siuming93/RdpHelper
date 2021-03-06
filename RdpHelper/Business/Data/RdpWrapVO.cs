﻿namespace RdpHelper.Business.Data
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

        public RdpWrapVO(object element)
        {
            //todo 初始化
        }

        public RdpWrapVO()
        {
        }

        public static RdpWrapVO FakeVO(string id)
        {
            return new RdpWrapVO() { id = id};
        }
    }
}
