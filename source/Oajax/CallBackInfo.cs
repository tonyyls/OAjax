using System;

namespace Oajax
{
    public class CallBackInfo
    {
        public int Code { get; set; }
        public String ReturnValue { get; set; }
        public String Message { get; set; }

        public CallBackInfo(int code, String rv, String msg)
        {
            Code = code;
            ReturnValue = rv;
            Message = msg;
        }
    }
}
