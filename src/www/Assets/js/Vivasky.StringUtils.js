﻿/**
 ** Created By Levin Van 20070911
 ** StringUtils is a javascript class.
 ** For detail pls refer to http://jsjava.sourceforge.net
 ** Revision:080307
 **/

function StringUtils(){
	this.jsjava_class="jsorg.apache.commons.lang.StringUtils";	
}
StringUtils.abbreviate=function(str,offset,maxWidth){
	if (str == null) {
        return null;
    }
    if (maxWidth < 4) {
        maxWidth==4;
    }
    if (str.length <= maxWidth) {
        return str;
    }
    if (offset > str.length) {
        offset = str.length;
    }
    if ((str.length - offset) < (maxWidth - 3)) {
        offset = str.length - (maxWidth - 3);
    }
    if (offset <= 4) {
        return str.substring(0, maxWidth - 3) + "...";
    }
    if (maxWidth < 7) {
        maxWidth=7;
    }
    if ((offset + (maxWidth - 3)) < str.length) {
        return "..." + StringUtils.abbreviate(str.substring(offset),0, maxWidth - 3);
    }
    return "..." + str.substring(str.length - (maxWidth - 3));
};
StringUtils.capitalize=function(str){
	if(str==null||str==""){
		return str;
	}
	if(str.length==1){
		return str.toUpperCase();
	}
	var nstr=str.charAt(0).toUpperCase()+str.substring(1);
	return nstr;
};
StringUtils.isEmpty=function(str){
	if(str==undefined||str==""||str==null){
		return true;
	}
	return false;
};
StringUtils.isNotEmpty=function(str){
	return !StringUtils.isEmpty(str);
};
StringUtils.isBlank=function(str){
	if(str==undefined||str==""||str==null||/^\s*$/.test(str)){
		return true;
	}
	return false;
};
StringUtils.isNotBlank=function(str){
	return !StringUtils.isBlank(str);
};
StringUtils.isAlpha=function(str){
	return /^[A-Za-z]+$/.test(str);
};
StringUtils.isAlphanumeric=function(str){
	return /^[A-Za-z0-9]+$/.test(str);
};
StringUtils.isAlphanumericSpace=function(str){
	if(str==""){
		return true;
	}
	return /^[A-Za-z0-9\s]+$/.test(str);
};
StringUtils.isAlphaSpace=function(str){
	if(str==""){
		return true;
	}
	return /^[A-Za-z\s]+$/.test(str);
};
StringUtils.isAsciiPrintable=function(str){
	if (str == null) {
        return false;
    }
    var length = str.length();
    for (var i = 0; i < length; i++) {
        var ch=str.charAt(i);
        ch=ch.charCodeAt(0);
        if(ch < 32 && ch >= 127){
        	return false;
        }
    }
    return true;
};
StringUtils.isNumeric=function(str){
	return /^[0-9]+$/.test(str);
};
StringUtils.isNumericSpace=function(str){
	return /^[0-9\s]+$/.test(str);
};
StringUtils.isWhitespace=function(str){
	return /^[\s]+$/.test(str);
};
StringUtils.joinArray=function(arr,separator){
	if(StringUtils.isEmpty(separator)){
		return arr.join("");
	}
	return arr.join(separator);
};
StringUtils.joinList=function(list,separator){
	if(!(list instanceof Collection)){
		return null;
	}
	if(StringUtils.isEmpty(separator)){
		separator="";
	}
	var arr=list.toArray();
	return arr.join(separator);
};
StringUtils.joinIterator=function(iterator,separator){
	if(!iterator||!iterator.jsjava_class||iterator.jsjava_class!="jsjava.util.Iterator"){
		return null;
	}
	if(StringUtils.isEmpty(separator)){
		separator="";
	}
	iterator.moveTo(0);
	var list=new ArrayList();
	while(iterator.hasNext()){
		list.add(iterator.next());
	}
	var arr=list.toArray();
	return arr.join(separator);
};
StringUtils.trim=function(str){
	if(StringUtils.isEmpty(str)){
		return str;
	}
	return str.replace(/(^\s*)|(\s*$)/g, ""); 
};
StringUtils.charLength=function(str){
	var nstr=str.replace(/[^x00-xff]/g,"JJ");
	return nstr.length;

};
/*Methods below are defined in JsJava's ChinaValidatorUtils class*/
StringUtils.isChinese=function(value){
	var regx=/^[\u4e00-\u9fa5]+$/;
    return regx.test(value);
};
StringUtils.isIDCard=function(value){
	var regx=/^(\d{15}|\d{17}[\dx])$/;
    return regx.test(value);
};
StringUtils.isMobile=function(value){
	var regx=/^13[0-9]\d{8}$/;
	var regx1=/^15[0-9]\d{8}$/;	
  return (regx.test(value)||regx1.test(value));
};
StringUtils.isOicq=function(value){
	var regx=/^[1-9][0-9]{4,}$/;
    return regx.test(value);
};
StringUtils.isPhone=function(value){
	var regx=/^\d{3}-\d{8}|\d{4}-\d{7}$/;
    return regx.test(value);
};
StringUtils.isZip=function(value){
	var regx=/^\d{6}$/;
    return regx.test(value);

};
/*Methods below are defined by myself*/
StringUtils.isPlusFloat = function(str) {
	var regx = /^(?:[1-9][0-9]*(?:\.[0-9]+)?|0\.[0-9]+)$/;
	return regx.test(str);
};
StringUtils.isMinusFloat = function(str) {
	var regx = /^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$/;
	return regx.test(str);
};
StringUtils.isFloat = function(str) {
	var regx = /^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$/;
	return regx.test(str);
};
StringUtils.isPlusInt = function(str) {
	var regx = /^[1-9]\d*$/;
	return regx.test(str);
};
StringUtils.isMinusInt = function(str) {
	var regx = /^-[1-9]\d*$/;
	return regx.test(str);
};
StringUtils.isInt=function(str){
	var regx=/^-?\d+$/;
	return regx.test(str);
};
/**
 ** Detect whether a string value is a valid format date string.
 ** Param1:str,must be a date value or a date input object
 ** Param2:separator,a char that separate day,month or year part of the date string.
 */
StringUtils.isDateStr = function(dateStr, separator) {
	var retVal = {};
	retVal.isValid = false;
	retVal.date = null;
	try {
		dateStr = (typeof dateStr == 'object') ? dateStr.value : dateStr;
	} catch (e) {
		retVal.msg = "StringUtils.isDateStr的str参数无效！";
		return retVal;
	}
	var dArray = dateStr.split(separator);
	if (dArray.length != 3) {
		retVal.msg = "日期格式错误,请参考YYYY-MM-DD格式输入有效的日期";
		return retVal;
	}
	var Y = dArray[0];
	var M = dArray[1];
	M = (M.indexOf("0") == 0) ? M.substring(1) : M;
	var D = dArray[2];
	D = (D.indexOf("0") == 0) ? D.substring(1) : D;
	var tempDateStr = M + "/" + D + "/" + Y;
	try {
		var date = new Date(tempDateStr);
		if ((date.getDate() == D) && (date.getMonth() == (M - 1)) && (date.getFullYear() == Y)) {
			retVal.isValid = true;
			retVal.date = date;
		} else {
			retVal.msg = "日期无效，请参考YYYY-MM-DD格式输入有效的日期";
		}
	} catch (e) {
		retVal.msg = "日期有误，请参考YYYY-MM-DD格式输入有效的日期";
	}
	return retVal;
};