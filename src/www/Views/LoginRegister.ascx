<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.ViewBase"%>
<div class="left">
	<form method="post" action="" class="clearfix" id="fmLogin">
		<h1 class="padlock">Member Login</h1>
		<label for="txtUserID" class="grey">Username:</label>
		<input type="text" size="23" value="" id="txtUserID" name="UserID" class="field"/>
		<label for="txtPwd" class="grey">Password:</label>
		<input type="password" size="23" id="txtPwd" name="Password" class="field"/>
    	<label><input type="checkbox" value="forever" checked="checked" id="rememberme" name="RememberMe"/>  Remember me</label>
		<input type="submit" class="bt_silver" value="Login" name="submit"/>
		<a href="#" class="lost-pwd">Lost your password?</a>
	</form>
</div>
<div class="left right">
	<form method="post" action="" id="fmRegister">
		<h1>Not a member yet? Sign Up!</h1>				
		<label for="txtNewUserID" class="grey">Username:</label>
		<input type="text" size="23" value="" id="txtNewUserID" name="UserID" class="field"/>
		<label for="txtEmail" class="grey">Email:</label>
		<input type="text" size="23" id="txtEmail" name="Email" class="field"/>
		<label>A password will be e-mailed to you.</label>
		<input type="submit" class="bt_blue" value="Register" name="submit"/>
	</form>
</div>