<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Page Language="c#" AutoEventWireup="false" Inherits="Brettle.Web.NeatUpload.ProgressPage" %>
<%@ Register TagPrefix="Upload" Namespace="Brettle.Web.NeatUpload" Assembly="Brettle.Web.NeatUpload" %>
<%--
NeatUpload - an HttpModule and User Controls for uploading large files
Copyright (C) 2005  Dean Brettle
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.
This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.
You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
--%>
<html>
<head id="Head1" runat="server">
    <title>
        <%= Resources.Resource.UploadProgressLabel %></title>
    <link rel="stylesheet" type="text/css" title="default" href="default.css" />
    <style type="text/css">
        body, form, table, tr, td
        {
            margin: 0px;
            border: 0px none;
            padding: 0px;
        }
        html, body, form, #progressDisplayCenterer
        {
            width: 100%;
            height: 100%;
        }
        #progressDisplayCenterer
        {
            vertical-align: middle;
            margin: 0 auto;
        }
        #progressDisplay
        {
            vertical-align: middle;
            width: 100%;
        }
        #barTd
        {
            width: 100%;
        }
        #statusDiv
        {
            border-width: 1px;
            border-style: solid;
            padding: 0px;
            position: relative;
            width: 100%;
            text-align: center;
            z-index: 1;
        }
        #barDiv, #barDetailsDiv
        {
            border: 0px none;
            margin: 0px;
            padding: 0px;
            position: absolute;
            top: 0pt;
            left: 0pt;
            z-index: -1;
            height: 100%;
            width: 75%;
        }
    </style>
</head>
<body>
    <form id="dummyForm" runat="server">
    <table id="progressDisplayCenterer">
        <tr>
            <td>
                <table id="progressDisplay" class="ProgressDisplay">
                    <tr>
                        <td>
                            <span id="label" runat="server" class="Label">Upload&#160;Status:</span>
                        </td>
                        <td id="barTd">
                            <div id="statusDiv" runat="server" class="StatusMessage">
                                &#160;
                                <Upload:DetailsSpan ID="normalInProgress" runat="server" WhenStatus="NormalInProgress"
                                    Style="font-weight: normal; white-space: nowrap;">
                                    <%# String.Format(Resources.Resource.UploadNormalInProgressFormat,
										FormatCount(BytesRead), FormatCount(BytesTotal), CountUnits, FractionComplete,
										FormatRate(BytesPerSec), FormatTimeSpan(TimeRemaining), FormatTimeSpan(TimeElapsed)) %>
                                </Upload:DetailsSpan>
                                <Upload:DetailsSpan ID="chunkedInProgress" runat="server" WhenStatus="ChunkedInProgress"
                                    Style="font-weight: normal; white-space: nowrap;">
                                    <%# String.Format(Resources.Resource.UploadChunkedInProgressFormat,
										FormatCount(BytesRead), FormatCount(BytesTotal), CountUnits, FractionComplete,
										FormatRate(BytesPerSec), FormatTimeSpan(TimeRemaining), FormatTimeSpan(TimeElapsed)) %>
                                </Upload:DetailsSpan>
                                <Upload:DetailsSpan ID="processing" runat="server" WhenStatus="ProcessingInProgress ProcessingCompleted"
                                    Style="font-weight: normal; white-space: nowrap;">
                                    <%# ProcessingHtml %>
                                </Upload:DetailsSpan>
                                <Upload:DetailsSpan ID="completed" runat="server" WhenStatus="Completed">
                                    <%# String.Format(Resources.Resource.UploadCompleteFormat,
										FormatCount(BytesRead), FormatCount(BytesTotal), CountUnits, FractionComplete,
										FormatRate(BytesPerSec), FormatTimeSpan(TimeRemaining), FormatTimeSpan(TimeElapsed)) %>
                                </Upload:DetailsSpan>
                                <Upload:DetailsSpan ID="cancelled" runat="server" WhenStatus="Cancelled">
                                    <cy:SiteLabel ID="lblUploadCancelled" runat="server" ConfigKey="UploadCancelledLabel"
                                        UseLabelTag="false" />
                                </Upload:DetailsSpan>
                                <Upload:DetailsSpan ID="rejected" runat="server" WhenStatus="Rejected">
                                    <cy:SiteLabel ID="lblUploadRejected" runat="server" ConfigKey="UploadRejectedLabel"
                                        UseLabelTag="false" />
                                    <%# Rejection != null ? Rejection.Message : "" %>
                                </Upload:DetailsSpan>
                                <Upload:DetailsSpan ID="error" runat="server" WhenStatus="Failed">
                                    <cy:SiteLabel ID="lblUploadFailed" runat="server" ConfigKey="UploadFailedLabel" />
                                    <%# Failure != null ? Failure.Message : "" %>
                                </Upload:DetailsSpan>
                                <Upload:DetailsDiv ID="barDetailsDiv" runat="server" UseHtml4="true" Width='<%# Unit.Percentage(Math.Floor(100*FractionComplete)) %>'
                                    CssClass="ProgressBar">
                                </Upload:DetailsDiv>
                            </div>
                        </td>
                        <td>
                            <asp:HyperLink ID="cancel" runat="server" Visible='<%# CancelVisible %>' NavigateUrl='<%# CancelUrl %>'
                                title='<%# Resources.Resource.CancelUploadAltText %>' class="ImageButton">
                                <img id="cancelImage" runat="server" src='<%# Cynthia.Web.Framework.WebUtils.GetSiteRoot() + "/Data/SiteImages/cancel.png" %>'
                                    alt='<%# Resources.Resource.CancelUploadAltText %>' /></asp:HyperLink>
                            <asp:HyperLink ID="refresh" runat="server" Visible='<%# StartRefreshVisible %>' NavigateUrl='<%# StartRefreshUrl %>'
                                title='<%# Resources.Resource.RefreshAltText %>' class="ImageButton">
                                <img id="refreshImage" runat="server" src='<%# Cynthia.Web.Framework.WebUtils.GetSiteRoot() + "/Data/SiteImages/refresh2.png"  %>'
                                    alt='<%# Resources.Resource.RefreshAltText %>' /></asp:HyperLink>
                            <asp:HyperLink ID="stopRefresh" runat="server" Visible='<%# StopRefreshVisible %>'
                                NavigateUrl='<%# StopRefreshUrl %>' title='<%# Resources.Resource.StopRefreshAltText %>'
                                class="ImageButton">
                                <img id="stopRefreshImage" runat="server" src='<%# Cynthia.Web.Framework.WebUtils.GetSiteRoot() + "/Data/SiteImages/stop_refresh.png" %>'
                                    alt='<%# Resources.Resource.StopRefreshAltText %>' /></asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
