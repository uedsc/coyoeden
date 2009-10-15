<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoyoEden.UI.Views.ViewBase" %>
<input type="hidden" id="ASPSESSID" value="<%=Session.SessionID %>" />
<div id="swfu_container">
	<div class="buttonContainer blue">
        <span id="spanButtonPlaceholder"></span>
	</div>
	<div id="divFileProgressContainer"></div>
	<div id="swfutip" class="s1">
		<ul id="txttip"></ul>
		<div id="thumbnails"></div>
	</div>
</div>