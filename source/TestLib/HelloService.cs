using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestLib
{
    public  class HelloService
    {
        //返回字符串
        public String GetSampleStr(String str)
        {
            //todo 
            return str;
        }

        //返回自定义对象
        public UserInfo GetUserInfo(String id, int age, Boolean sex)
        {
            //todo
            return new UserInfo(id,age,sex);
        }

        //返回List
        public List<UserInfo> GetUserList()
        {
            var list=new List<UserInfo>();
            var user1 = new UserInfo("yulsh", 26, true);
            var user2 = new UserInfo("menglin", 24, true);
            list.Add(user1);
            list.Add(user2);
            return list;
        } 
    }
}
