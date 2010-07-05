/**
 * @author levin
 * @desc	1,命名空间声明2,横切分栏dom模板
 */
/*=命名空间定义=*/
//sohu
var sohu={};
//sohu编辑器
sohu.diy={};
//sohu编辑器模板
sohu.diyTp={};
/*=/命名空间定义=*/


/*=横切模板区域=*/
//空横切
sohu.diyTp["w0"]='<div class="area"><div class="col w950"><div class="sec"></div></div></div>';
//2栏横切
sohu.diyTp["w270_670"]='<div class="area clear"><div class="col w270 left"><div class="sec"></div></div><div class="col w670 right"><div class="sec"></div></div></div>';
sohu.diyTp["w430_510"]='<div class="area clear"><div class="col w430 left"><div class="sec"></div></div><div class="col w510 right"><div class="sec"></div></div></div>';
sohu.diyTp["w470_470"]='<div class="area clear"><div class="col w470 left"><div class="sec"></div></div><div class="col w470 right"><div class="sec"></div></div></div>';
sohu.diyTp["w670_270"]='<div class="area clear"><div class="col w670 left"><div class="sec"></div></div><div class="col w270 right"><div class="sec"></div></div></div>';
sohu.diyTp["w510_430"]='<div class="area clear"><div class="col w510 left"><div class="sec"></div></div><div class="col w430 right"><div class="sec"></div></div></div>';
//3栏横切
sohu.diyTp["w190_270_470"]='<div class="area clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w270 center"><div class="sec"></div></div><div class="col w470 right"><div class="sec"></div></div></div>';
sohu.diyTp["w310_390_230"]='<div class="area clear"><div class="col w310 left"><div class="sec"></div></div><div class="col w390 center"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["w470_270_190"]='<div class="area clear"><div class="col w470 left"><div class="sec"></div></div><div class="col w270 center"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
sohu.diyTp["w270_390_270"]='<div class="area clear"><div class="col w270 left"><div class="sec"></div></div><div class="col w390 center"><div class="sec"></div></div><div class="col w270 right"><div class="sec"></div></div></div>';

/*=/横切模板区域=*/

/*=分栏模板区域=*/
//390
sohu.diyTp["sw190_190"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
//430
sohu.diyTp["sw210_210"]='<div class="subsec clear"><div class="col w210 left"><div class="sec"></div></div><div class="col w210 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_230"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw230_190"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
//470
sohu.diyTp["sw230_230"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw270_190"]='<div class="subsec clear"><div class="col w270 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_270"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w270 right"><div class="sec"></div></div></div>';

//510
sohu.diyTp["sw250_250"]='<div class="subsec clear"><div class="col w250 left"><div class="sec"></div></div><div class="col w250 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_310"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w310 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw310_190"]='<div class="subsec clear"><div class="col w310 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';

//670
sohu.diyTp["sw330_330"]='<div class="subsec clear"><div class="col w330 left"><div class="sec"></div></div><div class="col w330 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw190_470"]='<div class="subsec clear"><div class="col w190 left"><div class="sec"></div></div><div class="col w470 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw470_190"]='<div class="subsec clear"><div class="col w470 left"><div class="sec"></div></div><div class="col w190 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw230_190_230"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w190 center"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';

//950
sohu.diyTp["sw310_310_310"]='<div class="subsec clear"><div class="col w310 left"><div class="sec"></div></div><div class="col w310 center"><div class="sec"></div></div><div class="col w310 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw230_230_230_230"]='<div class="subsec clear"><div class="col w230 left"><div class="sec"></div></div><div class="col w230 center"><div class="sec"></div></div><div class="col w230 center"><div class="sec"></div></div><div class="col w230 right"><div class="sec"></div></div></div>';
sohu.diyTp["sw182_182_182_182_182"]='<div class="subsec clear"><div class="col w182 left"><div class="sec"></div></div><div class="col w182 center"><div class="sec"></div></div><div class="col w182 center"><div class="sec"></div></div><div class="col w182 center"><div class="sec"></div></div><div class="col w182 right"><div class="sec"></div></div></div>';

/*=/分栏模板区域=*/

/*=内容模板区域=*/
sohu.diyTp["ctEmptyLine"]='<div class="vspace"><hr/></div>';
/*=/内容模板区域=*/

