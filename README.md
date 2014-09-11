<h2>Oajax</h2>
<h4>解决$.ajax(...)与 .NET 后台数据交互的问题</h4>
<p>在进行.NET与AJAX交互设计的时候，我们通常会用JQuery的ajax函数调用HttpHandler，通过HttpResponse将数据写回给客户端。每次有了新的需求就新建一个.ashx文件。
这样做根本不用动脑子，而且还很开心，不亦乐乎，很多代码都可以复制黏贴，相信很多.Net开发人员都有类似的体会！但慢慢的就会暴露出很多问题，
比如：ashx路径改变,前端js得随着变；工程里边ashx文件越来越多维护越来越困难！下面的结构大家应该很熟悉了！</p>
<pre>
<code>
$.ajax({ type:"post", contentType: "application/json", datatype:"json", url:"dataservice.ashx",
                data:{UID:1}, success:function(data) {alert("ok")} });
</code>
</pre>
<p>现在，有了Oajax，再也不用建立ashx文件了，再也不怕路径改变了，Js的API使用也更加简单了!</p>
<h3>安装</h3>
<p>1、在Web工程中导入Oajax.dll、Newtonsoft.Json.dll任意版本,Oajax依赖于Newtonsoft.Json。</p>
<p>2、打开Web.config，配置如下：</p>
<pre>
<code>
&lt;httpHandlers&gt;
  ....
  &lt;add verb="*" path="Oajax.ashx" type="Oajax.OajaxHandler,Oajax"/&gt;
&lt;/httpHandlers&gt;
</code>
</pre>
<p>3、引入下面脚本，Oajax是一款JQuery插件。</p>
<pre>
<code>
&lt;script src="http://cdn.bootcss.com/jquery/1.10.2/jquery.min.js"&gt;&lt;/script&gt;
&lt;script src="Oajax/Oajax_v1.0.js" type="text/javascript"&gt;&lt;/script&gt;
</code>
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
<h2>使用</h2>
<p>具体使用示例，请下载源码，运行Default.aspx即可。</p>