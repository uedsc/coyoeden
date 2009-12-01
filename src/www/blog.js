// global object
CoyoEden = {
    $: function(id) {
        return document.getElementById(id);
    }
	,
    webRoot: '',
    // internationalization (injected into the <head> by SiteBasePage)
    i18n: {
        hasRated: '',
        savingTheComment: '',
        comments: '',
        commentWasSaved: '',
        commentWaitingModeration: '',
        cancel: '',
        filter: '',
        apmlDescription: '',
        beTheFirstToRate: '',
        currentlyRated: '',
        ratingHasBeenRegistered: '',
        rateThisXStars: ''
    }
	,
    setFlag: function(iso) {
        if (iso.length > 0)
            CoyoEden.comments.flagImage.src = CoyoEden.webRoot + "pics/flags/" + iso + ".png";
        else
            CoyoEden.comments.flagImage.src = CoyoEden.webRoot + "pics/pixel.gif";
    }
	,

    // Shows the preview of the comment
    showCommentPreview: function() {
        var oPreview = this.$('preview');
        var oCompose = this.$('compose');

        if (oPreview) oPreview.className = 'selected';
        if (oCompose) oCompose.className = '';
        this.$('commentCompose').style.display = 'none';
        this.$('commentPreview').style.display = 'block';
        this.$('commentPreview').innerHTML = '<img src="' + CoyoEden.webRoot + 'pics/ajax-loader.gif" alt="Loading" />';
        var argument = this.$('commentPreview').innerHTML;
        this.addComment(true);
    }
	,
    composeComment: function() {
        var oPreview = this.$('preview');
        var oCompose = this.$('compose');

        if (oPreview) oPreview.className = '';
        if (oCompose) oCompose.className = 'selected';
        this.$('commentPreview').style.display = 'none';
        this.$('commentCompose').style.display = 'block';
    }
	,
    endShowPreview: function(arg, context) {
        CoyoEden.$('commentPreview').innerHTML = arg;
    }
	,
    toggleCommentSavingIndicators: function(bSaving) {
        CoyoEden.$("btnSaveAjax").disabled = bSaving;
        CoyoEden.$("ajaxLoader").style.display = bSaving ? "inline" : "none";
        CoyoEden.$("status").className = "";
        CoyoEden.$("status").innerHTML = "";
        if (!bSaving) {
            CoyoEden.$('commentPreview').innerHTML = "";
            CoyoEden.composeComment();
        }
    }
    ,
    onCommentError: function(error, context) {
        CoyoEden.toggleCommentSavingIndicators(false);
        error = error || "Unknown error occurred.";
        var iDelimiterPos = error.indexOf("|");
        if (iDelimiterPos > 0) {
            error = error.substr(0, iDelimiterPos);
            // Remove numbers from end of error message.
            while (error.length > 0 && error.substr(error.length - 1, 1).match(/\d/)) {
                error = error.substr(0, error.length - 1);
            }
        }
        alert("Sorry, the following error occurred while processing your comment:\n\n" + error);
    }
	,
    addComment: function(preview) {
        var isPreview = preview == true;
        if (!isPreview) {
            CoyoEden.toggleCommentSavingIndicators(true);
            this.$("status").innerHTML = CoyoEden.i18n.savingTheComment;
        }

        var author = CoyoEden.comments.nameBox.value;
        var email = CoyoEden.comments.emailBox.value;
        var website = CoyoEden.comments.websiteBox.value;
        var country = CoyoEden.comments.countryDropDown ? CoyoEden.comments.countryDropDown.value : "";
        var content = CoyoEden.comments.contentBox.value;
        var notify = CoyoEden.$("cbNotify").checked;
        var captcha = CoyoEden.comments.captchaField.value;
        var replyToId = CoyoEden.comments.replyToId ? CoyoEden.comments.replyToId.value : "";

        var callback = isPreview ? CoyoEden.endShowPreview : CoyoEden.appendComment;
        var argument = author + "-|-" + email + "-|-" + website + "-|-" + country + "-|-" + content + "-|-" + notify + "-|-" + isPreview + "-|-" + captcha + "-|-" + replyToId;

        WebForm_DoCallback(CoyoEden.comments.controlId, argument, callback, 'comment', CoyoEden.onCommentError, false);

        if (!isPreview && typeof (OnComment) != "undefined")
            OnComment(author, email, website, country, content);
    }
    ,
    cancelReply: function() {
        this.replyToComment('');
    }
	,
    replyToComment: function(id) {

        // set hidden value
        CoyoEden.comments.replyToId.value = id;

        // move comment form into position
        var commentForm = CoyoEden.$('comment-form');
        if (!id || id == '' || id == null || id == '00000000-0000-0000-0000-000000000000') {
            // move to after comment list
            var base = CoyoEden.$("commentlist");
            base.appendChild(commentForm);
            // hide cancel button
            CoyoEden.$('cancelReply').style.display = 'none';
        } else {
            // show cancel
            CoyoEden.$('cancelReply').style.display = '';

            // move to nested position
            var parentComment = CoyoEden.$('id_' + id);
            var replies = CoyoEden.$('replies_' + id);

            // add if necessary
            if (replies == null) {
                replies = document.createElement('div');
                replies.className = 'comment-replies';
                replies.setAttribute('id') = 'replies_' + id;
                parentComment.appendChild(replies);
            }
            replies.style.display = '';
            replies.appendChild(commentForm);
        }

        CoyoEden.comments.nameBox.focus();
    }
	,
    appendComment: function(args, context) {
        if (context == "comment") {
            var commentList = CoyoEden.$("commentlist");
            if (commentList.innerHTML.length < 10)
                commentList.innerHTML = "<h1 id='comment'>" + CoyoEden.i18n.comments + "</h1>"

            // add comment html to the right place
            var id = CoyoEden.comments.replyToId ? CoyoEden.comments.replyToId.value : '';

            if (id != '') {
                var replies = CoyoEden.$('replies_' + id);
                replies.innerHTML += args;
            } else {
                commentList.innerHTML += args;
                commentList.style.display = 'block';
            }

            // reset form values
            CoyoEden.comments.contentBox.value = "";
            CoyoEden.comments.contentBox = CoyoEden.$(CoyoEden.comments.contentBox.id);
            CoyoEden.toggleCommentSavingIndicators(false);
            CoyoEden.$("status").className = "success";

            if (!CoyoEden.comments.moderation)
                CoyoEden.$("status").innerHTML = CoyoEden.i18n.commentWasSaved;
            else
                CoyoEden.$("status").innerHTML = CoyoEden.i18n.commentWaitingModeration;

            // move form back to bottom
            var commentForm = CoyoEden.$('comment-form');
            commentList.appendChild(commentForm);
            // reset reply to
            if (CoyoEden.comments.replyToId) CoyoEden.comments.replyToId.value = '';
            if (CoyoEden.$('cancelReply')) CoyoEden.$('cancelReply').style.display = 'none';
        }

        CoyoEden.$("btnSaveAjax").disabled = false;
    }
	,
    checkAuthorName: function(sender, args) {
        args.IsValid = true;

        if (CoyoEden.comments.checkName) {
            var author = CoyoEden.comments.postAuthor;
            var visitor = CoyoEden.comments.nameBox.value;
            args.IsValid = !this.equal(author, visitor);
        }
    }
	,
    addBbCode: function(v) {
        try {
            var contentBox = CoyoEden.comments.contentBox;
            if (contentBox.selectionStart) // firefox
            {
                var pretxt = contentBox.value.substring(0, contentBox.selectionStart);
                var therest = contentBox.value.substr(contentBox.selectionEnd);
                var sel = contentBox.value.substring(contentBox.selectionStart, contentBox.selectionEnd);
                contentBox.value = pretxt + "[" + v + "]" + sel + "[/" + v + "]" + therest;
                contentBox.focus();
            }
            else if (document.selection && document.selection.createRange) // IE
            {
                var str = document.selection.createRange().text;
                contentBox.focus();
                var sel = document.selection.createRange();
                sel.text = "[" + v + "]" + str + "[/" + v + "]";
            }
        }
        catch (ex) { }

        return;
    }
	,
    // Searches the blog based on the entered text and
    // searches comments as well if chosen.
    search: function(root) {
        var input = this.$("searchfield");
        var check = this.$("searchcomments");

        var search = "search.aspx?q=" + encodeURIComponent(input.value);
        if (check != null && check.checked)
            search += "&comment=true";

        top.location.href = root + search;

        return false;
    }
	,
    // Clears the search fields on focus.
    searchClear: function(defaultText) {
        var input = this.$("searchfield");
        if (input.value == defaultText)
            input.value = "";
        else if (input.value == "")
            input.value = defaultText;
    }
	,
    rate: function(id, rating) {
        this.createCallback("rating.axd?id=" + id + "&rating=" + rating, CoyoEden.ratingCallback);
    }
	,
    ratingCallback: function(response) {
        var rating = response.substring(0, 1);
        var status = response.substring(1);

        if (status == "OK") {
            if (typeof OnRating != "undefined")
                OnRating(rating);

            alert(CoyoEden.i18n.ratingHasBeenRegistered);
        }
        else if (status == "HASRATED") {
            alert(CoyoEden.i18n.hasRated);
        }
        else {
            alert("An error occured while registering your rating. Please try again");
        }
    }
	,
    /// <summary>
    /// Creates a client callback back to the requesting page
    /// and calls the callback method with the response as parameter.
    /// </summary>
    createCallback: function(url, callback) {
        var http = CoyoEden.getHttpObject();
        http.open("GET", url, true);

        http.onreadystatechange = function() {
            if (http.readyState == 4) {
                if (http.responseText.length > 0 && callback != null)
                    callback(http.responseText);
            }
        };

        http.send(null);
    }
	,
    /// <summary>
    /// Creates a XmlHttpRequest object.
    /// </summary>
    getHttpObject: function() {
        if (typeof XMLHttpRequest != 'undefined')
            return new XMLHttpRequest();

        try {
            return new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                return new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e) { }
        }

        return false;
    }
	,
    // Updates the calendar from client-callback
    updateCalendar: function(args, context) {
        var cal = CoyoEden.$('calendarContainer');
        cal.innerHTML = args;
        CoyoEden.Calendar.months[context] = args;
    }
	,
    toggleMonth: function(year) {
        var monthList = CoyoEden.$("monthList");
        var years = monthList.getElementsByTagName("ul");
        for (i = 0; i < years.length; i++) {
            if (years[i].id == year) {
                var state = years[i].className == "open" ? "" : "open";
                years[i].className = state;
                break;
            }
        }
    }
	,
    // Adds a trim method to all strings.
    equal: function(first, second) {
        var f = first.toLowerCase().replace(new RegExp(' ', 'gi'), '');
        var s = second.toLowerCase().replace(new RegExp(' ', 'gi'), '');
        return f == s;
    }
	,
    /*-----------------------------------------------------------------------------
    XFN HIGHLIGHTER
    -----------------------------------------------------------------------------*/
    xfnRelationships: ['friend', 'acquaintance', 'contact', 'met'
										, 'co-worker', 'colleague', 'co-resident'
										, 'neighbor', 'child', 'parent', 'sibling'
										, 'spouse', 'kin', 'muse', 'crush', 'date'
										, 'sweetheart', 'me']
	,
    // Applies the XFN tags of a link to the title tag
    hightLightXfn: function() {
        var content = CoyoEden.$('content');
        if (content == null)
            return;

        var links = content.getElementsByTagName('a');
        for (i = 0; i < links.length; i++) {
            var link = links[i];
            var rel = link.getAttribute('rel');
            if (rel && rel != "nofollow") {
                for (j = 0; j < CoyoEden.xfnRelationships.length; j++) {
                    if (rel.indexOf(CoyoEden.xfnRelationships[j]) > -1) {
                        link.title = 'XFN relationship: ' + rel;
                        break;
                    }
                }
            }
        }
    }
	,

    showRating: function(container, id, raters, rating) {
        var div = document.createElement('div');
        div.className = 'rating';

        var p = document.createElement('p');
        div.appendChild(p);
        if (raters == 0) {
            p.innerHTML = CoyoEden.i18n.beTheFirstToRate;
        }
        else {
            p.innerHTML = CoyoEden.i18n.currentlyRated.replace('{0}', new Number(rating).toFixed(1)).replace('{1}', raters);
        }

        var ul = document.createElement('ul');
        ul.className = 'star-rating small-star';
        div.appendChild(ul);

        var li = document.createElement('li');
        li.className = 'current-rating';
        li.style.width = Math.round(rating * 20) + '%';
        li.innerHTML = 'Currently ' + rating + '/5 Stars.';
        ul.appendChild(li);

        for (var i = 1; i <= 5; i++) {
            var l = document.createElement('li');
            var a = document.createElement('a');
            a.innerHTML = i;
            a.href = 'rate/' + i;
            a.className = this.englishNumber(i);
            a.title = CoyoEden.i18n.rateThisXStars.replace('{0}', i.toString()).replace('{1}', i == 1 ? '' : 's');
            a.onclick = function() {
                CoyoEden.rate(id, this.innerHTML);
                return false;
            };

            l.appendChild(a);
            ul.appendChild(l);
        }

        container.innerHTML = '';
        container.appendChild(div);
        container.style.visibility = 'visible';
    }
	,

    applyRatings: function() {
        var divs = document.getElementsByTagName('div');
        for (var i = 0; i < divs.length; i++) {
            if (divs[i].className == 'ratingcontainer') {
                var args = divs[i].innerHTML.split('|');
                CoyoEden.showRating(divs[i], args[0], args[1], args[2]);
            }
        }
    },

    englishNumber: function(number) {
        if (number == 1)
            return 'one-star';

        if (number == 2)
            return 'two-stars';

        if (number == 3)
            return 'three-stars';

        if (number == 4)
            return 'four-stars';

        return 'five-stars';
    }
	,
    // Adds event to window.onload without overwriting currently assigned onload functions.
    // Function found at Simon Willison's weblog - http://simon.incutio.com/
    addLoadEvent: function(func) {
        var oldonload = window.onload;
        if (typeof window.onload != 'function') {
            window.onload = func;
        }
        else {
            window.onload = function() {
                oldonload();
                func();
            }
        }
    }
	,
    filterByAPML: function() {
        var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
        var height = document.documentElement.clientHeight + document.documentElement.scrollTop;
        document.body.style.position = 'static';

        var layer = document.createElement('div');
        layer.style.zIndex = 2;
        layer.id = 'layer';
        layer.style.position = 'absolute';
        layer.style.top = '0px';
        layer.style.left = '0px';
        layer.style.height = document.documentElement.scrollHeight + 'px';
        layer.style.width = width + 'px';
        layer.style.backgroundColor = 'black';
        layer.style.opacity = '.6';
        layer.style.filter += ("progid:DXImageTransform.Microsoft.Alpha(opacity=60)");
        document.body.appendChild(layer);

        var div = document.createElement('div');
        div.style.zIndex = 3;
        div.id = 'apmlfilter';
        div.style.position = (navigator.userAgent.indexOf('MSIE 6') > -1) ? 'absolute' : 'fixed';
        div.style.top = '200px';
        div.style.left = (width / 2) - (400 / 2) + 'px';
        div.style.height = '50px';
        div.style.width = '400px';
        div.style.backgroundColor = 'white';
        div.style.border = '2px solid silver';
        div.style.padding = '20px';
        document.body.appendChild(div);

        var p = document.createElement('p');
        p.innerHTML = CoyoEden.i18n.apmlDescription;
        p.style.margin = '0px';
        div.appendChild(p);

        var form = document.createElement('form');
        form.method = 'get';
        form.style.display = 'inline';
        form.action = CoyoEden.webRoot;
        div.appendChild(form);

        var textbox = document.createElement('input');
        textbox.type = 'text';
        textbox.value = CoyoEden.getCookieValue('url') || 'http://';
        textbox.style.width = '320px';
        textbox.id = 'txtapml';
        textbox.name = 'apml';
        textbox.style.background = 'url(' + CoyoEden.webRoot + 'pics/apml.png) no-repeat 2px center';
        textbox.style.paddingLeft = '16px';
        form.appendChild(textbox);
        textbox.focus();

        var button = document.createElement('input');
        button.type = 'submit';
        button.value = CoyoEden.i18n.filter;
        button.onclick = function() { location.href = CoyoEden.webRoot + '?apml=' + encodeURIComponent(CoyoEden.$('txtapml').value) };
        form.appendChild(button);

        var br = document.createElement('br');
        div.appendChild(br);

        var a = document.createElement('a');
        a.innerHTML = CoyoEden.i18n.cancel;
        a.href = 'javascript:void(0)';
        a.onclick = function() { document.body.removeChild(CoyoEden.$('layer')); document.body.removeChild(CoyoEden.$('apmlfilter')); document.body.style.position = ''; };
        div.appendChild(a);
    }
	,
    getCookieValue: function(name) {
        var cookie = new String(document.cookie);

        if (cookie != null && cookie.indexOf('comment=') > -1) {
            var start = cookie.indexOf(name + '=') + name.length + 1;
            var end = cookie.indexOf('&', start);
            if (end > start && start > -1)
                return cookie.substring(start, end);
        }

        return null;
    }
	,
    comments: {
        flagImage: null,
        contentBox: null,
        moderation: null,
        checkName: null,
        postAuthor: null,
        nameBox: null,
        emailBox: null,
        websiteBox: null,
        countryDropDown: null,
        captchaField: null,
        controlId: null,
        replyToId: null
    }
};

CoyoEden.addLoadEvent(CoyoEden.hightLightXfn);

// add this to global if it doesn't exist yet
if (typeof ($) == 'undefined')
    window.$ = CoyoEden.$;

if (typeof (registerCommentBox) != 'undefined')
    CoyoEden.addLoadEvent(registerCommentBox);
if (typeof (registerVariables) != 'undefined')
    CoyoEden.addLoadEvent(registerVariables);
if (typeof (setupCoyoEdenCalendar) != 'undefined')
    CoyoEden.addLoadEvent(setupCoyoEdenCalendar);

// apply ratings after registerVariables.
CoyoEden.addLoadEvent(CoyoEden.applyRatings);
