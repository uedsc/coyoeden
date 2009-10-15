<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoyoEden.UI.Views.ViewBase" %>
<!--jqModalLayout-->
<div class="<%=this.CssClass %>" id="<%=this.ID %>">
    <div class="dialog_main">
        <h3 class="dialog_title">%Title%</h3>
        <div class="dialog_content">%Body%</div>
        <p class="dialog_acts">
        <input class="bt_sub2 dialog_btn_ok" value="%txtOk%" type="button"/>
        <input class="bt_cancle2 dialog_btn_close jqmClose" value="%txtClose%" type="button"/>		
        </p>
        <p class="close"><a title="" href="#" class="jqmClose">关闭</a></p>
    </div>
    <div class="dialog_sharp"></div>
    <div class="jqDrag"></div>
</div>
<!--!jqModalLayout-->
