<%@ Control Language="C#" Inherits="CoyoEden.UI.Views.CommentFormView" %>
<div class="commentform" id="CommentForm1">
    <div class="content">
        <h2>
            <%=Resources.labels.addComment %></h2>
        <div class="fl">
            <div class="pic"><%=Avatar %></div>
            <div class="flag"><img alt="Country tag" src="<%=Utils.AbsoluteWebRoot %>assets/img/pixel.gif" id="imgFlag" width="16" height="11" /></div>
            <div class="rss">
            <input type="checkbox" id="cbNotify" value="1" title="Notifys me when having new comments!"/>RSS
            <input type="hidden" id="hidCaptcha" name="Captcha" />
            </div>
        </div>
        <div class="ml_100">
        <fieldset class="message">
            <div>
                <input type="text" onclick="this.value='';" class="wp48" value="*Your Name" id="author" name="Name"/>
                <%=RenderDropDownList("ddlCountry", "Country","input1", Resources.labels.regionState, "TwoLetterISORegionName", "EnglishName", SystemX.Infrastructure.CountryData.Countries.Cast<object>())%>
            </div>
            <div>
                <input type="text" onclick="this.value='';" value="*Your Email" id="email" name="Email"/>
            </div>
            <div>
                <input type="text" onclick="this.value='';" value="Your Website" id="url" name="Website"/>
            </div>
            <div class="textarea">
                <textarea rows="" cols="" id="comment" name="comment">Your Comments</textarea>
            </div>
            <div class="submit fl">
                <a href="" id="submit" title="<%=Resources.labels.saveComment %>"><%=Resources.labels.saveComment %></a>
            </div>
            <div class="notice clearfix">
                * Name, Email, Comment are Required</div>
        </fieldset>
        </div>
    </div>
    <!--/content -->
</div>
