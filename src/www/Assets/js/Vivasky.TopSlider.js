$(document).ready(function() {
	// Expand Panel
	$("#open").click(function() {
		$("div#toppanel").slideDown("slow"); return false;
	});
	// Collapse Panel
	$("#close").click(function() {
		$("div#toppanel").slideUp("slow"); return false;
	});
	// Switch buttons from "Log In | Register" to "Close Panel" on click
	$("#toggle a").click(function() {
		$("#toggle a").toggle(); return false;
	});
});