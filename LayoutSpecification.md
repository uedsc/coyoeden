# 背景 #

> 页面的布局方式是页面构成的主骨架，其重要性你们懂的～

> 拿到一个PSD，一般情况下你会：

> 看整体布局方式，是列式布局还是通栏式布局？
> 列式布局是2列还是3列？
> 通栏式布局一共有多少个通栏？每个通栏有多少列？

> 总的来说，看PSD就要像看MM，先要知道她PP大还是MM大。。。

> 所以在切页面前，先要写页面的主要布局的代码，和建楼房先搭地基的道理是如出一辙滴。布局代码确定后，再开始在布局上做“填空”游戏。

> 布局对于设计师来说也很重要，当开发员和设计师都遵循统一的布局规范时，可以大大的降低两者的沟通成本。同时保证页面风格的统一性。

  * v1.0
  * 适用期限 2011至2013年

## 常用栅格布局方案 ##

> 下面介绍的布局方案的示例代码使用[gridsystemgenerator](http://www.gridsystemgenerator.com/gs01.php) 参照[960gs框架](http://960.gs)自动生成。

> [css\_grids示例](http://lab.coyoeden.googlecode.com/hg/css_grids/)

> ### 页宽1000px ###

  * 示例1-[1000\_20\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/1000_20_5_5.html)

> 20列的组合(每列40px)，10像素间隔，实际宽度990px

  * 示例2-[1000\_20\_10\_10.html](http://lab.coyoeden.googlecode.com/hg/css_grids/1000_20_10_10.html)

> 20列的组合(每列30px)，20像素间隔，实际宽度980px

  * 示例3-[1000\_25\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/1000_25_5_5.html)

> 25列的组合(每列30px)，10像素间隔，实际宽度990px

  * 示例4-[1000\_25\_10\_10.html](http://lab.coyoeden.googlecode.com/hg/css_grids/1000_25_10_10.html)

> 25列的组合(每列20px)，20像素间隔，实际宽度980px


> ### 页宽990px ###

  * 示例1-[990\_11\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/990_11_5_5.html)
> ` 11列的组合(每列80px)，10像素间隔，实际宽度980px `

  * 示例2-[990\_18\_10\_10.html](http://lab.coyoeden.googlecode.com/hg/css_grids/990_18_10_10.html)
> ` 18列的组合(每列35px)，20像素间隔，实际宽度970px `

  * 示例3-[990\_18\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/990_18_5_5.html)
> 25列的组合(每列45px)，10像素间隔，实际宽度980px

  * 示例4-[990\_33\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/990_33_5_5.html)
> 33列的组合(每列20px)，10像素间隔，实际宽度980px

> ### 页宽980px ###

  * 示例1-[980\_14\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/980_14_5_5.html)
> ` 14列的组合(每列60px)，10像素间隔，实际宽度970px `

  * 示例2-[980\_14\_10\_10.html](http://lab.coyoeden.googlecode.com/hg/css_grids/980_14_10_10.html)
> ` 14列的组合(每列50px)，20像素间隔，实际宽度960px `

  * 示例3-[980\_28\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/980_28_5_5.html)
> 28列的组合(每列25px)，10像素间隔，实际宽度970px


> ### 页宽960px ###


  * 示例1-[960\_12\_5\_5.html](http://lab.coyoeden.googlecode.com/hg/css_grids/960_12_5_5.html)
> ` 12列的组合(每列70px)，10像素间隔，实际宽度950px `

  * 示例2-[960\_12AND16\_10\_10.html](http://lab.coyoeden.googlecode.com/hg/css_grids/960_12AND16_10_10.html)
> ` 12列或16列的组合(每列60px或40px)，20像素间隔，实际宽度940px `

## 1000/990/980...天哪这种数字究竟有神马用！！！！ ##

> 我那990\_33\_5\_5的栅格布局来解说吧～～

> ### 990px和33列的关系 ###

> 很简单的一个数学公式：
  * 90=33(列)**20(像素/列)+33\*2\*5(每列左右边距5像素)**

> ### 神奇的列组合 ###

  * 3可以拆成X个整数相加，每种加法可以构成总宽990px的布局！！

> 例如,33=21+12，可以构成两列布局，其中右边一列的宽度为21x20+21\*2\*5=630px，右列的宽度为12\*20+12\*10=360px

```
  <div class="w-33">
    <div class="g-21"></div>
    <div class="g-12"></div>
  </div>
```

> 其中w-33表示你采用33列的布局！！g-21表示grid-21，即一个宽度630px的栅格；

## 规范代码 ##

> ### 空白HTML页 ###

```
  <!DOCTYPE html>
  <html>
      <head>
          <meta charset="utf-8" />
          <title>Untitled Document</title>
          <!--[if lt IE 9]>
          <script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
          <![endif]-->
          <!--样式文件-->
          <link rel="stylesheet" type="text/css" href="css/reset.css" />
          <link rel="stylesheet" type="text/css" href="css/text.css" />
          <link rel="stylesheet" type="text/css" href="css/layout.css" />
          <link rel="stylesheet" type="text/css" href="css/site.xxx.css" />

          <!--JQuery-->
          <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>
      </head>
      <body>

          <!--页面交互的js文件-->
          <script type="text/javascript" src="js/AppFac.js"></script>
          <script type="text/javascript" src="js/site.xx.js"></script>
          <script type="text/javascript">
               AppFac.init({
                   uid:'xxx'
               });
          </script>
      </body>
  </html>
```

> [\*查看demo\*](http://lab.coyoeden.googlecode.com/hg/css_grids/layout_empty.html)

> ### 两列布局示例 ###

> 代码比较多。。

> 这个还是直接看[demo](http://lab.coyoeden.googlecode.com/hg/css_grids/GridsLayoutDemo.html)吧～

> 直接在wiki上看下代码，请猛击[GridsLayoutDemo](GridsLayoutDemo.md)

> ### 三列布局示例 ###

> 代码比较多。。

> 这个还是直接看[demo](http://lab.coyoeden.googlecode.com/hg/css_grids/GridsLayoutDemo.html)吧～

> 直接在wiki上看下代码，请猛击[GridsLayoutDemo](GridsLayoutDemo.md)