var this$ = function() {
    var p = {}, pub = {};
    //custom private methods
    p.showSearches = function() {
        p._search_cat.fadeIn("fast");
        p._search_icon.removeClass("arrow-up").addClass("arrow-down");
    };
    p.hideSearches = function(evt) {
        p._search_cat.fadeOut("fast");
        p._search_icon.removeClass("arrow-down").addClass("arrow-up");
    };
    p.toggleSearchC = function(flag) {
        $("#frmC input.sub-cat").each(function(i, e) {
            $(this).attr("checked", flag == 0 ? false : true).attr("disabled", flag == 0 ? false : true);
        });
    };
    p.onSubmitSearch = function() {
        if (p._txtSearch.val() === "Start Searching ..." || p._txtSearch.val() === "") {
            p.showSearches(); return false;
        };
        return true;
    };
    p.initNav = function() {
        $("#navbar li.submenu").hover(function(evt) { $(this).find(".inner-boundary").show(); }, function(evt) { $(this).find(".inner-boundary").hide(); });
        $("#categories").hover(function(evt) { $(this).children("ul").show(); }, function(evt) { $(this).children("ul").hide(); });
    };
    //default private methods
    p.initVar = function(opts) {
        p._search_cat = $("#search_cat");
        p._search_icon = $("#search_icon");
        p._txtSearch = $("#txtSearch");
    };
    p.onLoaded = function() {
        p._txtSearch.preInput({ val: 'Start Searching ...', afterfocus: p.showSearches }).click(p.showSearches);
        p.initNav();
    };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
        $("#searchfrm").submit(p.onSubmitSearch);
        $("#frmC").mouseleave(p.hideSearches);
        $("#cbxscall").click(function(evt) {
            if ($(this).is(":checked")) { p.toggleSearchC(1); } else { p.toggleSearchC(0); };
        });
        $("#home_slide").imgSlide({});
    };

    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} ();