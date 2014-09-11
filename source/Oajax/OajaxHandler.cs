using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;

namespace Oajax
{
    public class OajaxHandler : IHttpHandler
    {
        private CallBackInfo _callBackInfo;
        private const int ErrorCode = 500;
        private const String InvalidCommand = "Invalid command. eg:libname$classname$method";
        private const String InvalidParameter = "Invalid parameters.";

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse response = context.Response;
            String commandName = req.QueryString["command"];
            String[] commands = commandName.Split('$');

            //检查命令格式
            if (commands.Length < 3)
            {
                _callBackInfo = new CallBackInfo(ErrorCode, "", InvalidCommand);
                DoResonse(response);
                return;
            }

            //命令格式:libname$classname$method
            String assemablyName = commands[0];
            String className = commands[1];
            String methodName = commands[2];
            //检查命令是否合法
            if (String.IsNullOrEmpty(assemablyName) || String.IsNullOrEmpty(className) || String.IsNullOrEmpty(methodName))
            {
                _callBackInfo = new CallBackInfo(ErrorCode, "", InvalidCommand);
                DoResonse(response);
                return;
            }

            //反射拿到对应的类、方法、参数
            String fullName = assemablyName + "." + className;
            MethodInfo method;
            Object callObj;
            ParameterInfo[] parameterInfos;
            try
            {
                Assembly assembly = Assembly.Load(assemablyName);
                method = assembly.GetType(fullName).GetMethod(methodName);
                callObj = assembly.CreateInstance(fullName);
                parameterInfos = method.GetParameters();
            }
            catch (Exception ex)
            {
                _callBackInfo=new CallBackInfo(ErrorCode,"",ex.Message);
                DoResonse(response);
                return;
            }

            //处理js传入的参数
            var keys = req.Params.Keys;
            var paramsDict = new Dictionary<string, object>();
            for (int i = 0, length = keys.Count; i < length; i++)
            {
                var key = keys.Get(i);
                if (key.StartsWith("_"))
                {
                    paramsDict.Add(key, req.Params[key]);
                }
            }

            //组装成数组，为了按顺序传入方法
            var methodObj = new object[paramsDict.Count];
            var j = 0;
            foreach (var p in paramsDict)
            {
                methodObj[j] = paramsDict[p.Key];
                j++;
            }

            //检查参数个数
            if (parameterInfos.Length != methodObj.Length)
            {
                _callBackInfo=new CallBackInfo(ErrorCode,"",InvalidParameter);
                DoResonse(response);
                return;
            }

            //循环检查参数的合法性
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                var param = methodObj[i];
                Type valueType = param.GetType();
                Type parameterType = parameterInfos[i].ParameterType;
                //如果类型一样不用处理
                if (parameterType == valueType)
                {
                    continue;
                }
                //如果类型不一样就尝试转换,转换不成功就抛出错误
                if (parameterType == typeof(Boolean))
                {
                    methodObj[i] = Boolean.Parse(param.ToString());
                }
                else if (parameterType == typeof(Int32))
                {
                    methodObj[i] = Int32.Parse(param.ToString());
                }
                else
                {
                    _callBackInfo = new CallBackInfo(ErrorCode, "", InvalidParameter);
                    DoResonse(response);
                    return;
                }
            }
            //方法执行返回结果
            Object result = method.Invoke(callObj, methodObj);
            HandlerResult(response, result, result.GetType());
        }

        //处理返回结果
        private void HandlerResult(HttpResponse response, Object result, Type type)
        {
            //基础类型：Boolean,Int32等，注意:String不属于基础类型
            //其他类型：Dictionary<String,Object>,List<T>
            String ret;
            if (type.IsPrimitive || type == typeof(String))
            {
                ret = result.ToString();
                _callBackInfo = new CallBackInfo(response.StatusCode, ret, response.StatusDescription);
            }
            else
            {
                try
                {
                    ret = JsonConvert.SerializeObject(result);
                    _callBackInfo=new CallBackInfo(response.StatusCode,ret,response.StatusDescription);
                }
                catch (JsonSerializationException ex)
                {
                    _callBackInfo=new CallBackInfo(ErrorCode,"",ex.Message);
                }
            }
            DoResonse(response);
        }

        //执行返回
        private void DoResonse(HttpResponse response)
        {
            response.Write(JsonConvert.SerializeObject(_callBackInfo));
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
