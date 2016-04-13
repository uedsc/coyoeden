# 简介 #

> SEO优化的3列布局

> 拿一个wordpress主题[evidens-white](http://designdisease.com/preview/evidens-white)重构做的实验

> 自适应等高测试通过过的浏览器：ff4.0 chrome11 safari5 opera11 ie8~9

> 至于ie6不做测试。。。

# 特点 #

  * SEO优化：主体列最先显示
  * 可以很方便支持3列自适应“等高”（没什么新意，就是结合了“正负补丁”和“隐藏溢出”）

# 示例布局代码 #

## 主体列在左边（g-col3-ml） ##

> HTML代码

```
<!--#page-->
<section id="page" class="g-col3-ml g-equalh">
    <section id="main" class="col-m">
	<div class="col-m-inner">
		#main
	</div>
    </section>            
    <section id="aside-x" class="col-aside col-aside-x">
	#left<br/>
		#left<br/>
		#left<br/>
	</section>            
    <section id="aside-y" class="col-aside col-aside-y">
	#right<br/>
		#right<br/>
		#right<br/>
		#right<br/>
		#right<br/>
		#right<br/>
		#right<br/>
		#right<br/>
		#right<br/>
	</section>   			
</section>
```

> CSS代码

```
/* 3 columns fluid layout
 -------------------------------------------------------------------------------------------------*/
.g-col3-ml{ 
	/* note: g-col3-ml means the layout that of which main column lies on the left side! */
	overflow:hidden;
	zoom:1;
}
.g-col3-ml .col-m{
	/* margin-computed by aside-x ,aside-y and gutters */
	margin-right:515px; /* 160 +25+300+30 */
}
.g-col3-ml .col-m-inner{
	float: left; 
	width: 100%; 
}
.g-col3-ml .col-aside,.g-col3-ml .col-m-inner{
	min-height:50px;
	height:auto !important;
	height:50px;
}
.g-col3-ml .col-aside{
	float:right;	
}
.g-col3-ml .col-aside-y{
	/* default width - you can override it in your page stylesheet */
	width:160px;
	margin-right:25px;
}
.g-col3-ml .col-aside-x{
	/* default width - you can override it in your page stylesheet */
	width:300px;
}

/* equal height column layout
 ------------------------------------------------------------------------------------------------*/
.g-equalh .col-aside,.g-equalh .col-m{
	padding-bottom: 32767px;
    margin-bottom: -32767px;
}
```

> 请猛击[这里查看线上demo](http://lab.coyoeden.googlecode.com/hg/css_grids/fluidLayout-3col.html)

## 主体列在右边（g-col3-mr） ##

> HTML代码和g-col3-ml保持一致，样式稍微有点区别

> 请猛击[这里查看线上demo](http://lab.coyoeden.googlecode.com/hg/css_grids/fluidLayout-3col-mr.html)

## 主体列在中间（g-col3-mm） ##

> HTML代码和g-col3-ml保持一致，样式稍微有点区别

> 请猛击[这里查看线上demo](http://lab.coyoeden.googlecode.com/hg/css_grids/fluidLayout-3col-mm.html)