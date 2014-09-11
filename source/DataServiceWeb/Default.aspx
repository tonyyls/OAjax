<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DataServiceWeb._Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Oajax</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link href="Css/bootstrap.css" rel="stylesheet" />
    <!--使用的是cdn-->
    <script src="http://cdn.bootcss.com/jquery/1.10.2/jquery.min.js"></script>
    <script src="http://cdn.bootcss.com/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <!--Oajax依赖于jquery-->
    <script src="Oajax/Oajax_v1.0.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnTest1").click(function () {
                $.Oajax.request("TestLib$HelloService$GetSampleStr", {"str":"Hello,I'm Oajax"},
                    function (res) {
                        alert(JSON.stringify(res));
                    });
                });
                $("#btnTest2").click(function () {
                    $.Oajax.request("TestLib$HelloService$GetUserInfo", { "id":"yulsh","age":26,"sex":true },
                    function (res) {
                        alert(JSON.stringify(res));
                    });
                });
                $("#btnTest3").click(function () {
                    $.Oajax.request("TestLib$HelloService$GetUserList", {},
                    function (res) {
                        alert(JSON.stringify(res));
                    });
                });
        });
    </script>
</head>
<body>
    <div id="header" style="background: green; color: white">
        <div class="container">
            <h1>
                Oajax</h1>
            <h4>
                解决$.ajax(...)与 .NET 后台数据交互的问题</h4>
        </div>
    </div>
    <div id="content">
        <div class="container">
            <h2>
                概述</h2>
            <p>
                在进行.NET与AJAX交互设计的时候，我们通常会用JQuery的ajax函数调用HttpHandler，通过HttpResponse将数据写回给客户端。每次有了新的需求就新建一个.ashx文件，久而久之发现工程里面N多.ashx文件。并且还不亦乐乎，我以前就是这么干的！！</p>
            <pre><code>$.ajax({ type:"post", contentType: "application/json", datatype:"json", url:"dataservice.ashx",
                data:{UID:1}, success:function(data) {alert("ok")} }); </code></pre>
            <p>
                现在，我有了Oajax，ashx文件不用建了，API更简单了！</p>
            <h2>
                安装</h2>
            <p>
                1、在Web工程中导入Oajax.dll、Newtonsoft.Json.dll任意版本,Oajax依赖于Newtonsoft.Json。</p>
            <p>
                2、打开Web.config，配置如下：</p>
            <pre>
&lt;httpHandlers&gt;
  ....
  &lt;add verb="*" path="Oajax.ashx" type="Oajax.OajaxHandler,Oajax"/&gt;
&lt;/httpHandlers&gt;
</pre>
            <p>
                3、引入下面脚本，Oajax是基于JQuery的一款插件。</p>
            <pre>
<code>&lt;script src="http://cdn.bootcss.com/jquery/1.10.2/jquery.min.js"&gt;&lt;/script&gt;
&lt;script src="Oajax/Oajax_v1.0.js" type="text/javascript"&gt;&lt;/script&gt; </code>
</pre>
<h2>说明</h2>
<p>版本：当前版本为v1.0</p>
<p>接口说明:</p>
<p>$.Oajax.request(command,params,callback,options)</p>
<p>参数说明:</p>
<p>command:调用命令，格式为：libname$classname$method 或者 dll名称$服务类名$方法名</p>
<p>params:传入方法的参数，只支持Json对象</p>
<p>callback:回调函数，带一个对象参数</p>
<p>options:Json格式，如{async:true,isLog:true}，async表示是否异步(默认异步)，isLog表示是否将请求日志输出到浏览器控制台(F12)</p>
            <h2>
                使用</h2>
            <p>
                1、简单请求后台方法，后台返回String。</p>
                <pre>
<code>
//js
$.Oajax.request("TestLib$HelloService$GetSampleStr", {"str":"Hello,I'm Oajax"},
                    function (res) {
                        alert(JSON.stringify(res));
                    });
//c#
//返回字符串
        public String GetSampleStr(String str)
        {
            //todo 
            return str;
        }
</code>
</pre>

            <button type="button" id="btnTest1" class="btn btn-success">
                执行</button>
                
                <p>2、请求后台方法，后台返回自定义对象</p>
                                <pre>
<code>
//js
$.Oajax.request("TestLib$HelloService$GetUserInfo", { "id":"yulsh","age":26,"sex":true },
                    function (res) {
                        alert(JSON.stringify(res));
                    });
//c#
        //返回自定义对象
        public UserInfo GetUserInfo(String id, int age, Boolean sex)
        {
            //todo
            return new UserInfo(id,age,sex);
        }
</code>
</pre>
                <button type="button" id="btnTest2" class="btn btn-success"> 执行</button>
                <p>3、请求后台方法，后台返回List&lt;T&gt;数据</p>
                                <pre>
<code>
//js
 $.Oajax.request("TestLib$HelloService$GetUserList", {},
                    function (res) {
                        alert(JSON.stringify(res));
                    });
//c#
        //返回List
        public List&lt;UserInfo&gt; GetUserList()
        {
            var list=new List&lt;UserInfo&gt;();
            var user1 = new UserInfo("yulsh", 26, true);
            var user2 = new UserInfo("menglin", 24, true);
            list.Add(user1);
            list.Add(user2);
            return list;
        } 
</code>
</pre>
                <button type="button" id="btnTest3" class="btn btn-success"> 执行</button>
                <div style="height: 60px;">&nbsp;</div>
        </div>
    </div>
</body>
</html>
