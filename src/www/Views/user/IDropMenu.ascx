<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.ViewBase"%>
<%@ Import Namespace="SystemX" %>
<div id="personal_hover" style="display:none;">
    <div id="personal_hover_shadow" class="lightShadow"></div>
    <div id="personal_hover_inner">
    <a id="personal_hover_link" href="<%=AbsoluteWebRoot%>profile.aspx?u=%uid%" title="">
		<img id="personal_hover_img" src="%avatar%" alt=""/>
	</a>
    <img class="hidden" id="personal_hover_pulser_img" src="<%=ThemeRoot%>img/loading50x50.gif" width="50" height="50" alt="" />
    <div id="personal_trigger_bar">
    <a class="personal_drop" id="personal_menu_down" href="javascript:;" title="">更多内容</a>
    <div id="personal_menu_show" class="personal_buddy_menus" style="display:none;">
    <span class="blank_block"></span>
    <p id="personal_relationship_g"><span class="be_contacts"><strong>%uid%</strong></span></p>
    <div id="personal_menu_other_div">
        <a id="personal_o_index" class="blocks" href="<%=AbsoluteWebRoot%>profile.aspx?u=%uid%" title="%uid%的个人主页">%uid%的个人主页</a>
		<a id="personal_o_minifeed"  class="blocks" href="<%=AbsoluteWebRoot%>feed.aspx?u=%uid%" title="近况">近况</a>
        <a id="personal_o_contact"  class="blocks" href="<%=AbsoluteWebRoot%>guys.aspx?u=%uid%" title="好友">好友</a>
        <a id="personal_o_topic"  class="blocks" href="<%=AbsoluteWebRoot%>topics.aspx?u=%uid%" title="参与的话题">参与的话题</a>
    </div>
    <a id="personal_msgsned" class="blocks_out" href="<%=AbsoluteWebRoot%>admin/msg.aspx?u=%uid%" title="发站内信">发站内信</a>
    <div id="person_menu_self_div"></div>
    </div>
    </div>
    </div>
</div>
