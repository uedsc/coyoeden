# 背景 #

在基于Html的应用中，js的全局变量比较邪恶，一来容易与别人的js代码或js框架产生冲突，二来影响页面性能。

两年前曾经看到过[Douglas Crockford](http://www.crockford.com/)提到的module pattern of javascript，利用这种代码模式可以最低限度的减少全局变量污染dom上下文，后来Yahoo的js框架YUI便采用了这种模式，当然还有现在流行的jquery中也有module pattern的影子。

关于Module pattern的基本介绍，推荐查阅文档

[yui module pattern](http://yuiblog.com/blog/2007/06/12/module-pattern/)

[module pattern](http://ajaxian.com/archives/a-javascript-module-pattern)


## QeeUE的Module Pattern模板 ##

> TODO:很忙呢，晚点补充