<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickNav.ascx.cs" Inherits="Views_user_QuickNav" %>
<%@ Import Namespace="SystemX.Web"%>
<div class="p-quick clearfix">
	<span>快捷:</span> 
	<span>
		<a class="shortcut" title="最新主题" href="<%=Utils.RelativeWebRoot %>posts.aspx?t=new">新主题</a>
		<a class="shortcut" title="最新评论" href="<%=Utils.RelativeWebRoot %>comments.aspx?t=new">新评论</a> 
		<a class="shortcut" title="热门主题" href="<%=Utils.RelativeWebRoot %>posts.aspx?t=hot">热主题</a> 
		<a class="shortcut" title="收藏夹" href="<%=Utils.RelativeWebRoot %>fav.aspx">收藏夹</a> 
		<a class="shortcut" title="今日推荐" href="<%=Utils.RelativeWebRoot %>recommended.aspx">今日荐</a>
	</span>
	<span>|</span> 
	<span class="p-quick-set">
		<a id="shortcuts_gears" title="快捷导航设置" href="javascript:;" class="gears">快捷导航设置</a>
		<div id="shortcuts_gears_list" style="display: none;" class="gears_list">
			<p class="hint">选中表示显示，最多可显示5个在导航上;</p>
			<form method="post" action="<%=Utils.RelativeWebRoot %>admin/navsettings.aspx">
			<p class="shortcut_item">
				<input type="checkbox" id="type1" value="1" checked="checked" name="nav[]" />
				<input type="hidden" value="1" name="stay[]" />
				<label for="type1">最新主题</label>
				<em class="actions">
					<a target="_blank" title="点击查看" href="<%=Utils.RelativeWebRoot %>posts.aspx?t=new">
						<img width="10" height="10" alt="点击查看" src="/res/img/default/popup.gif" />
					</a> 
					<a class="remove_shortcut" title="删除" href="javascript:;"><img width="10" height="10" alt="删除" src="/res/img/default/remove.gif" /></a>
				</em>
			</p>
			<p class="shortcut_item">
				<input type="checkbox" id="type2" value="2" checked="checked" name="nav[]" />
				<input type="hidden" value="2" name="stay[]" />
				<label for="type2">最新评论</label>
				<em class="actions"><a target="_blank" title="点击查看" href="<%=Utils.RelativeWebRoot %>comments.aspx?t=new">
					<img width="10" height="10" alt="点击查看" src="/res/img/default/popup.gif" /></a> <a
						class="remove_shortcut" title="删除" href="javascript:;">
						<img width="10" height="10" alt="删除" src="/res/img/default/remove.gif" /></a>
				</em>
			</p>
			<p class="shortcut_item">
				<input type="checkbox" id="type3" value="3" checked="checked" name="nav[]" />
				<input type="hidden" value="3" name="stay[]" />
				<label for="type3">热门主题</label>
				<em class="actions"><a target="_blank" title="点击查看" href="<%=Utils.RelativeWebRoot %>posts.aspx?t=hot">
					<img width="10" height="10" alt="点击查看" src="/res/img/default/popup.gif" /></a> <a
						class="remove_shortcut" title="删除" href="javascript:;">
						<img width="10" height="10" alt="删除" src="/res/img/default/remove.gif" /></a>
				</em>
			</p>
			<p class="shortcut_item">
				<input type="checkbox" id="type4" value="4" checked="checked" name="nav[]" />
				<input type="hidden" value="4" name="stay[]" />
				<label for="type4">
					收藏夹</label>
				<em class="actions"><a target="_blank" title="点击查看" href="<%=Utils.RelativeWebRoot %>fav.aspx">
					<img width="10" height="10" alt="点击查看" src="/res/img/default/popup.gif" /></a> <a
						class="remove_shortcut" title="删除" href="javascript:;">
						<img width="10" height="10" alt="删除" src="/res/img/default/remove.gif" /></a>
				</em>
			</p>
			<p class="shortcut_item">
				<input type="checkbox" id="type5" value="5" checked="checked" name="nav[]" />
				<input type="hidden" value="5" name="stay[]" />
				<label for="type5">
					今日推荐</label>
				<em class="actions"><a target="_blank" title="点击查看" href="<%=Utils.RelativeWebRoot %>recommended.aspx">
					<img width="10" height="10" alt="点击查看" src="/res/img/default/popup.gif" /></a> <a
						class="remove_shortcut" title="删除" href="javascript:;">
						<img width="10" height="10" alt="删除" src="/res/img/default/remove.gif" /></a>
				</em>
			</p>
			<p class="acts">
				<input type="submit" value="保 存" name="submit" class="bt_sub2" />
			</p>
			</form>
		</div>
	</span><span>|</span><span class="p-quick-set"><a id="add2quick" title="" href="javascript:;"
		class="add2quick">添加的快捷导航</a>
		<div id="add2quick_tips" style="display: none;" class="add2quick_tips ">
			点击添加本页到快捷导航<a title="" href="#">(使用方法)</a></div>
	</span>
</div>
