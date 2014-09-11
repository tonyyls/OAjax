using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestLib
{
    public  class UserInfo
    {
        public string Id { get; set; }

        public int Age { get; set; }

        public bool Sex { get; set; }


        public UserInfo(string id, int age, bool sex)
        {
            this.Id = id;
            this.Age = age;
            this.Sex = sex;
        }
    }
}
