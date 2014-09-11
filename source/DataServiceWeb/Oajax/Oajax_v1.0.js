/*
*Name:Oajax
*Author:Yu LongSheng
*Version:v0.1
*Time:2014.1.3
*/
;(function ($) {
    $.Oajax = {
        defaultOptions: {
             async:true,
             isLog:true
        },
        getContextPath: function() {
            var fullPath=window.document.location.href;
            var pathName=window.document.location.pathname;
            var pos;
            var contextPath;
            if (pathName=="/") {
                contextPath = fullPath;
            } else {
                pos=fullPath.indexOf(pathName);
                var prePath=fullPath.substring(0,pos);
                var virtualPath=pathName.substring(0,pathName.substr(1).indexOf('/')+1);
                contextPath = prePath + virtualPath+"/";
            }
            return contextPath;
        },
        log: function(tag,content) {
            if (this.defaultOptions.isLog) {
                tag == "" ? console.log(content) : console.log(tag + ":" + content);
            }
        },
        checkParamsIsLegal:function(params) {
            return typeof params == "object" ? true : false;
        },
        request: function (command,params,callback,options) {
           this.log("", "prepare request.");
           if (!this.checkParamsIsLegal(params)) {
               this.log("Error","Invalid parameter.");
               return;
           }
           this.log("params",JSON.stringify(params));
           if (options) {
                $.extend(this.defaultOptions, options);
           }
           var finalOpts = this.defaultOptions;
           var finalUrl =this.getContextPath()+"oajax.ashx?command="+command;
           this.log("url",finalUrl);
           var finalParams = {};
           for (var p in params) {
               finalParams["_"+p] = params[p];
           }
           this.log("", "start "+(finalOpts.async?"async":"sync")+" request.");
           $.ajax({  
                    url:finalUrl,
                    async:finalOpts.async,
                    contentType:finalOpts.contentType,
                    data:finalParams,
                    type:finalOpts.type||"POST",
                    error: function(xhr, ts, errorThown) {
                        alert("request error.");
                        $.Oajax.log("", "request error.");
                    },
                    success: function(data) {
                        $.Oajax.log("", "request finished.");
                        var result = eval('(' + data + ')');
                        callback(result);
                    }
           });
        }
    };
})(jQuery);

