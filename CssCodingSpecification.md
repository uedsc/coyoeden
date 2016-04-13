# 背景 #

> 代码不是写给自己看的～

> 本文档由QeeUe.com成员共同商讨决定并不断修订，旨在明确一份大家均认可的编码规范，提高团队代码的可读性、维护性，降低维护成本。同时为QeeUE的新同学降低学习曲线。

> 本规范执行通过团员间的交叉codereview进行。

> 基本原则:语义化

## CSS\HTML命名规范 ##

> ### id命名 ###

  * 小写，中划线分隔，“对象名-属性名”例如：

```
  <div id="user-name">...</div> 
```

  * 避免超过3级，如确实超过3级，3级之后的内容考虑缩写合并至第三级

> ~~`<div id="user-xxx-yyy-zzz">...</div>`~~

> 可考虑：

```
  <div id="user-xxx-yyyZzz">...</div> 
```

> 注意：3级以后的内容合并至第三级时使用驼峰样式！如上面的\*yyyZzz

  * 按钮功能的id

> 请以"btn-"作为前缀，例如 "btn-submit"。

> 如果按钮是js交互用的事件钩子(event hook)，请以"ebtn-"作为前缀。

  * 显示文本用的标签的id
> 例如label、span等，有时候出于js交互目的需要使用id属性，那么id请以"lbl-"作为前缀，例如"lbl-user-name"

  * icon图标的id

> icon图标如需id属性，请以i-作为前缀，例如
```
  <span id="i-sina"></span> 
```

  * 更多内容期待英雄们补充...

> ### class命名 ###

> 类似id命名

  * 小写，中划线，简短富有含义。避免使用list1、list2等含糊的名称

  * 避免超过3级，如确实超过3级，3级之后的内容考虑缩写合并至第三级

  * 页面级布局(page layout)和组件布局(widget layout)的命名规范请参考文档“LayoutSpecification”

  * 网站小图标(icon)
> 使用css sprite合图作为背景。且使用"icons"做为主类名，使用i-xx作为次类名。

> 例如：新浪微博的图标为 **` <span class="icons i-sina"></span> `**

  * 更多内容期待英雄们补充...

> ### data-xx属性 ###

> data-xx属性是html5标准属性，可用于存放与元素相关的且与展示相关的数据。

> 请小写，且避免出现3级或以上 ~~` data-xxx-yyy `~~

> 例如客户端的分页组件：
```
  <div id="pager" data-total="300" data-rn="20"></div> 
```

## HTML编码规范 ##

> ### YES ###

  1. 标签和属性务必小写
  1. 使用xhtml标准时，标签属性必须有值且用双引号包含。~~` <img alt="" .../> `~~
  1. 内容中的特殊字符建议使用字符实体编码，”<”,”>”,”&”这3个字符必须使用字符实体编码。即` &lt;&gt;&amp; `
  1. 使用定高宽的img标签时，需标明width和height。
  1. 使用flash对象时，标明width和height
  1. 图片属于正式内容的重要部分时，使用img标签且一定要加alt属性。图片属于修饰性内容时尽量通过css设定
  1. 编写页面时考虑减少请求数量，包括图片、css和js。

> ### NO ###

  1. 使用非标准或者已经过时的标签。 例如b、nobr、i等
  1. 标签不闭合。~~` <input type="text" ...> `~~
  1. 文档没有指定charset

## CSS编码规范 ##

  * 尽量使用link标签引入css文件，避免内联样式

> ~~` <style type="text/css">...</style> `~~

> ~~` <div style="..."></div> `~~

  * 针对类名选择，避免针对标签进行选择

> 例如对于

```
  <div class="widget">
    <h3 class="wdg-title">...</h3>
  </div> 
```

> 避免下面的代码选择h3

> ~~` .widget h3 `~~

> 应该这样

> ` .widget .wdg-title `

> 这样也不好,使用id作为选择器没有必要加标签了...

> ~~` div#user `~~

> 这样也很悲剧的写法,不利于重构

> ~~` div.user `~~ 应直接写成.user

  * 选择器少使用3级，避免3级以上的低能选择器

> 例如 ~~` #page .col-left .block-a .user `~~

> 这样的选择器能命中.user但性能低下，可以在.block-a上加一个id，如下

```
  #block-xxx .user
```

  * 每项css样式定义语句，即使是最后一项，也要以分号作为结束

  * 慎用hack，IE的问题可使用注释型hack以及`_`+`*`等方式修复，firefox和safari可使用浏览器专有样式属性

> ### 懒人原则 ###

  * 对于border,margin,padding等样式，注意简炼的缩写，避免上下左右分别写

> ~~` margin-top:1px;margin-bottom:1px;margin-left:2px;margin-right:3px; `~~

> 缩写为

> ` margin:1px 3px 1px 2px; `

  * 对于重复出现的样式，注意利用","写到一起，避免分别写

> ~~` #a{color:#fff;...} #b{color:#fff;...} `~~

> 缩写为

> ` #a,#b{color:#fff;} `

> ### 关于css3 ###

> 圆角效果、阴影效果尽量使用css3实现。

> 由于目前css3和html5标准没有正式普及，故涉及css3的样式代码使用html5.css来定义。