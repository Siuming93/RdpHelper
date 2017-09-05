using System;
using System.Collections.Generic;
using System.Net;

namespace RdpHelper.Business.Data
{
    class Proxy
    {
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

        private Action _onRequestComplete;
        public void RequestDataFromRemote(Action action = null)
        {
            //todo 去服务器拿数据
            //DoRequest();
            _onRequestComplete = action;
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

        private const string URI = "";
        private IAsyncResult DoRequest()
        {
            var webRequest = HttpWebRequest.CreateHttp(URI);
            return webRequest.BeginGetResponse(OnRespnse, webRequest);
        }

        private void OnRespnse(IAsyncResult ar)
        {
            _list.Clear();
            var webRequest = ar.AsyncState as HttpWebRequest;
            var response = webRequest.EndGetResponse(ar);
            foreach (var element in response.Headers)
            {
                _list.Add(new RdpWrapVO(element));
            }

            if (_onRequestComplete != null)
                _onRequestComplete.Invoke();
            _onRequestComplete = null;
        }
    }
}
