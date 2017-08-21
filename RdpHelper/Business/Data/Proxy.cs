using System;
using System.Collections.Generic;

namespace RdpHelper.Business.Data
{
    class Proxy
    {
        public void RequestDataFromRemote()
        {
            //todo 
            //去服务器拿数据
            FakeVOList();
        }

        private void FakeVOList()
        {
            _list.Clear();
            for (int i = 0; i < 10; i++)
            {
                _list.Add(new RdpWrapVO() { server = "192.168.0." + i.ToString(), password = "s", userName = "user1" });
            }
        }
        private static Proxy _proxy;
        public static Proxy Instance {
            get
            {
                if(_proxy == null)
                    _proxy = new Proxy();
                return _proxy;
            }
        }

        private List<RdpWrapVO> _list;

        private Proxy()
        {
            _list = new List<RdpWrapVO>();
        }

        public List<RdpWrapVO> GetList()
        {
            return new List<RdpWrapVO>(_list);
        }


    }
}
