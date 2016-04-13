# 背景 #

> 代码不是写给自己看的～

> 本文档由QeeUe.com成员共同商讨决定并不断修订，旨在明确一份大家均认可的编码规范，提高团队代码的可读性、维护性，降低维护成本。同时为QeeUE的新同学降低学习曲线。

> 本规范执行通过团员间的交叉codereview进行。

> js编码的基本原则：**模块化组织代码、避免暴露过多的全局变量或函数**


## 命名规范 ##

> ### 文件名 ###

> 文件名、命名空间应能体现代码功能，可读性要强

> 举个例子

  * sitexx.js 表示网站sitexx通用的js代码

  * sitexx.home.js 表示网站sitexx首页的js代码

  * sitexx.login.js 表示网站sitexx的登录组件代码

> ### 函数名 ###

  * 公有函数-驼峰式命名
```
  var functionName=function(){

  };
```

  * 私有函数-下划线+驼峰式命名
```
  var _functionName=function(){

  };
```

> 特别说明：使用Module Pattern组织代码时，你会发现所有的私有变量或者函数都挂在变量p下面，所有的公有变量或者函数都挂在pub下面。

> 例如
```
  var ModuleA=(function(){
    var p={},pub={};

    /* 私有区域 */

    //p用来引用私有的变量或函数或私有的子模块
    p.var1=1;
    p.sayHi=function(){
      alert("hi");
    };
    p.subModuleB={
      init:function(){
        //子模块B的初始化逻辑
      }
    };

    /* 公有接口 */
    pub.varX=2;
    pub.init=function(){
      //模块A的初始化逻辑
    };
    pub.methodA=function(){
      //methodA的逻辑
    };
    return pub;
  })();
```

> 详见[JSModulePattern](JSModulePattern.md)

> ### 变量名 ###

  * 驼峰式(推荐)
```
  var varXyz="xyz";
```

  * 下划线+小写
```
  var var_xyz="xyz";
```

## 编码规范 ##

  * 语句结束必须以分号结束！（否则使用yui等压缩工具时压出来的代码可能不能用）

  * 函数声明
> 统一使用
```
  var functionName=function(){
  }; 
```
> 而非
```
  function functionName(){
  };
```

  * 排版缩进

  1. 缩进为一个tab或4个ASCII空格。推荐4个空格。不同编辑器或者操作系统的tab宽度是不一样的
  1. 语句体（比如函数体、if语句体）的第一个大括号不要换行

> 函数体要这样：
```
  var functionName=function(){
    //函数体
  };
```

> 不要这样：
```
  var functionName=function()
  {
    //函数体
  };
```


> if语句体要这样：
```
  if(xxx){
    //xxx
  }else{
    //yyy
  };
```

> 不要这样：
```
  if(xxx)
  {
    //xxx
  }
  else
  {
    //yyy
  };
```

  * 避免原型扩展原生的JS对象

> 不要写这样的代码：
```
  //扩展原生Array对象。。。结局可能很悲剧的...
  Array.prototype.each=function(){

  };
```

## 注释规范 ##

> 参考aptana中js编辑器的注释方式

  * 模块或原型对象的注释
```
  /**
   * ModuleA的功能描述
   * @param {String} arg1 参数1的说明
   * @version 版本号
   * @author 作者
   * @requires 依赖性说明
   */
  var ModuleA=function(arg1){/*xxx*/};
```


  * 公有函数的注释

> 参考上面模块的注释方式。

  * 语句或私有函数的注释

```
  //语句注释方式1

  /`*` 注释方式2 -私有函数建议用方式2 `*`/
```

  * 区域注释

> 代码行数很多的时候可能会用到区域注释方式。
```
  /* 区域A
   ----------------------*/


  /* 区域B
   ----------------------*/
```




## 使用规范 ##

  * 外联引用js文件，避免页面出现js代码（除非特殊需要）
  * Script标签尽量放在页面底部,建议在` </body> `之前

## 编码组织方式-Module pattern简介 ##

  * 请参考文档[JSModulePattern](JSModulePattern.md)

## 推荐的JS资料 ##

  1. [Mozilla的JS指南](https://developer.mozilla.org/en/JavaScript/Guide)
  1. [Google的js编码风格-英文版](http://google-styleguide.googlecode.com/svn/trunk/javascriptguide.xml)