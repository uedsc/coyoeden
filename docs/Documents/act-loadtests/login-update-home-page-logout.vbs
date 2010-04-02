Option Explicit
Dim fEnableDelays
fEnableDelays = False

Sub SendRequest1()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (0)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest2()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (6)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest3()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (55)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/skins/centered1/IESpecific.css"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/skins/centered1/IESpecific.css"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest4()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (4)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/skins/centered1/style.css"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/skins/centered1/style.css"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest5()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (16)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/App_Themes/default/theme.css"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/App_Themes/default/theme.css"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest6()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (49)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=wzYMextq5JuPmaa3AykFmQ2&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest7()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (120)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ClientScript/AdapterUtils.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ClientScript/AdapterUtils.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest8()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (14)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ClientScript/MenuAdapter.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ClientScript/MenuAdapter.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest9()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (11)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/logos/mojotonguesmall.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/logos/mojotonguesmall.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest10()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (3)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/FeatureIcons/home.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/FeatureIcons/home.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest11()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (4)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/FeatureIcons/SmallCalendar.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/FeatureIcons/SmallCalendar.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest12()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (3)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/valid-xhtml10.png"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/valid-xhtml10.png"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest13()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (4)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/poweredbymojoportal3.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/poweredbymojoportal3.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest14()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (0)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/vcss.png"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/vcss.png"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest15()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (1375)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Secure/Login.aspx"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Secure/Login.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest16()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (53)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=wzYMextq5JuPmaa3AykFmQ2&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/Secure/Login.aspx"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest17()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (53)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=WhBw9bXZBREMcU_-HYI69g2&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/Secure/Login.aspx"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest18()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (6612)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Secure/Login.aspx"
        oRequest.Verb = "POST"
        oRequest.HTTPVersion = "HTTP/1.0"
        oRequest.EncodeBody = False
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/Secure/Login.aspx"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "Content-Type", "application/x-www-form-urlencoded"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "Content-Length", "(automatic)" 
        oRequest.Body = "__LASTFOCUS=&__VIEWSTATE=%2FwEPDwULLTE2Njc2ODUyMjE"
        oRequest.Body = oRequest.Body + "PFgQeC1VybFJlZmVycmVyBRVodHRwOi8vbG9hZHRlc3QvbW9qb"
        oRequest.Body = oRequest.Body + "y8eD0xvZ2luRXJyb3JDb3VudGYWAmYPZBYCAgMPZBYKAgUPDxY"
        oRequest.Body = oRequest.Body + "CHgdWaXNpYmxlaGRkAgsPZBYEZg8PFgIfAmhkZAICDw8WAh8Ca"
        oRequest.Body = oRequest.Body + "GRkAg0PDxYCHwJoZBYCAgEPDxYCHwJoZGQCDg9kFgICBQ9kFgI"
        oRequest.Body = oRequest.Body + "CAw88KwAKAQAPFgweEkRlc3RpbmF0aW9uUGFnZVVybAUVaHR0c"
        oRequest.Body = oRequest.Body + "DovL2xvYWR0ZXN0L21vam8vHhRQYXNzd29yZFJlY292ZXJ5VGV"
        oRequest.Body = oRequest.Body + "4dAUQUmVjb3ZlciBQYXNzd29yZB4TUGFzc3dvcmRSZWNvdmVye"
        oRequest.Body = oRequest.Body + "VVybAUwaHR0cDovL2xvYWR0ZXN0L21vam8vU2VjdXJlL1JlY29"
        oRequest.Body = oRequest.Body + "2ZXJQYXNzd29yZC5hc3B4HgtGYWlsdXJlVGV4dAUPTG9naW4gR"
        oRequest.Body = oRequest.Body + "mFpbGVkIQ0KHg5DcmVhdGVVc2VyVGV4dAUIcmVnaXN0ZXIeDUN"
        oRequest.Body = oRequest.Body + "yZWF0ZVVzZXJVcmwFKWh0dHA6Ly9sb2FkdGVzdC9tb2pvL1NlY"
        oRequest.Body = oRequest.Body + "3VyZS9SZWdpc3Rlci5hc3B4ZBYCZg9kFggCAQ8PFgIfAmhkZAI"
        oRequest.Body = oRequest.Body + "LDxAPFgQeB0NoZWNrZWRoHgRUZXh0BQ5SZW1lbWJlciBMb2dpb"
        oRequest.Body = oRequest.Body + "mRkZGQCDQ8PFgQeCUFjY2Vzc0tleQUBMR8KBQ5Mb2cgSW4gW0F"
        oRequest.Body = oRequest.Body + "sdCsxXWRkAhMPDxYEHwoFEFJlY292ZXIgUGFzc3dvcmQeC05hd"
        oRequest.Body = oRequest.Body + "mlnYXRlVXJsBTBodHRwOi8vbG9hZHRlc3QvbW9qby9TZWN1cmU"
        oRequest.Body = oRequest.Body + "vUmVjb3ZlclBhc3N3b3JkLmFzcHhkZAIPDw8WAh8CaGRkGAEFH"
        oRequest.Body = oRequest.Body + "l9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBSZjdGw"
        oRequest.Body = oRequest.Body + "wMCRtYWluQ29udGVudCRMb2dpbkN0cmwkUmVtZW1iZXJNZRuZi"
        oRequest.Body = oRequest.Body + "LEY1y0vp%2FoB9BbzapTOZ9P2&ctl00%24mainContent%24Lo"
        oRequest.Body = oRequest.Body + "ginCtrl%24UserName=admin&ctl00%24mainContent%24Log"
        oRequest.Body = oRequest.Body + "inCtrl%24Password=admin&ctl00%24mainContent%24Logi"
        oRequest.Body = oRequest.Body + "nCtrl%24Login=Log+In+%5BAlt%2B1%5D&__EVENTTARGET=&"
        oRequest.Body = oRequest.Body + "__EVENTARGUMENT=&__EVENTVALIDATION=%2FwEWBQKjk%2BX"
        oRequest.Body = oRequest.Body + "lAwL7m%2B%2BpDgKg4ou3CAKBx%2FiPCAKG5vsBVCHLL0SQUu%"
        oRequest.Body = oRequest.Body + "2B062yVYE71UbHlGU0%3D"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Secure/Login.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest19()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (51)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/Secure/Login.aspx"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest20()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (110)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=wzYMextq5JuPmaa3AykFmQ2&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest21()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (59)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/edit.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/edit.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest22()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (8)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/pencil.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/pencil.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest23()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (9)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/FeatureIcons/file.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/FeatureIcons/file.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest24()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (8)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/FeatureIcons/gears.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/FeatureIcons/gears.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest25()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (13)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/skins/centered1/arrowRight.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/skins/centered1/arrowRight.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest26()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (10)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/FeatureIcons/online.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/FeatureIcons/online.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest27()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (2)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/FeatureIcons/image.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/FeatureIcons/image.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest28()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (2118)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/HtmlEdit.aspx"+"?mid=0&pageindex=&pageid=0"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/HtmlEdit.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest29()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (142)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=wzYMextq5JuPmaa3AykFmQ2&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest30()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (47)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ClientScript/ibox.js.aspx"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ClientScript/ibox.js.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest31()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (7)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/skins/centered1/ibox.css"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/skins/centered1/ibox.css"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest32()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (75)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ClientScript/calendar.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ClientScript/calendar.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest33()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (5)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/style/CalendarMojo.css"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/style/CalendarMojo.css"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest34()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (85)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ClientScript/calendar-en.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ClientScript/calendar-en.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest35()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (32)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ClientScript/calendar-setup.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ClientScript/calendar-setup.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest36()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (36)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=5SgJEeoemGRl-V7QL79q_1aXKWciAlmV7-8md8sPZYg1&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest37()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (47)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/fckeditor.html"+"?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/fckeditor.html"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest38()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (1)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/FeatureIcons/help.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/FeatureIcons/help.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest39()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (44)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/js/fckeditorcode_ie.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/js/fckeditorcode_ie.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest40()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (12)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/fckconfig.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/fckconfig.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest41()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (99)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/fck_editor.css"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/fck_editor.css"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest42()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (5)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/lang/en.js"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/lang/en.js"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest43()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (74)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/images/spacer.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/images/spacer.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest44()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (19)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/default/images/toolbar.start.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/default/images/toolbar.start.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest45()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (18)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/default/images/toolbar.buttonarrow.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/default/images/toolbar.buttonarrow.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest46()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (9)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/fck_strip.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/fck_strip.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest47()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (28)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/ibox-indicator.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/ibox-indicator.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest48()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (38)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.expand.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.expand.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest49()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (14)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.collapse.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.collapse.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest50()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (100)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/fckstyles.xml"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/fckstyles.xml"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest51()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (151)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.buttonarrow.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.buttonarrow.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest52()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (12)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.buttonarrow.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.buttonarrow.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest53()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (130)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.start.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.start.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest54()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (1)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.start.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.start.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest55()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (132)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/css/fck_internal.css"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/FCKeditor/editor/fckeditor.html?InstanceName=ctl00_mainContent_FCKTextBox0&Toolbar=Full"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/css/fck_internal.css"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest56()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (2779)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.arrowright.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.arrowright.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest57()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (21)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/FCKeditor/editor/skins/normal/images/toolbar.arrowright.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/FCKeditor/editor/skins/normal/images/toolbar.arrowright.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest58()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (3518)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/HtmlEdit.aspx"+"?mid=0&pageindex=&pageid=0"
        oRequest.Verb = "POST"
        oRequest.HTTPVersion = "HTTP/1.0"
        oRequest.EncodeBody = False
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "Content-Type", "application/x-www-form-urlencoded"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "Content-Length", "(automatic)" 
        oRequest.Body = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEP"
        oRequest.Body = oRequest.Body + "DwUKMjA3NDQwNDE5NA8WAh4LVXJsUmVmZXJyZXIFFWh0dHA6Ly"
        oRequest.Body = oRequest.Body + "9sb2FkdGVzdC9tb2pvLxYCZg9kFgICAw9kFggCBQ8PFgIeB1Zp"
        oRequest.Body = oRequest.Body + "c2libGVoZGQCCw9kFgICAg8PFgIeBFRleHQFBlNlYXJjaGRkAg"
        oRequest.Body = oRequest.Body + "4PDxYEHghDc3NDbGFzcwURY2VudGVyLWxlZnRtYXJnaW4eBF8h"
        oRequest.Body = oRequest.Body + "U0ICAmQWAgIFD2QWGAIDDxYKHgZIZWlnaHQbAAAAAADgdUABAA"
        oRequest.Body = oRequest.Body + "AAHgVWYWx1ZQWSBjxwPldlbGNvbWUgdG8gPHN0cm9uZz5tb2pv"
        oRequest.Body = oRequest.Body + "UG9ydGFsPC9zdHJvbmc%2BLCB0aGlzIGlzIHNhbXBsZSBkYXRh"
        oRequest.Body = oRequest.Body + "LCB5b3UgY2FuIGVkaXQgdGhpcyBjb250ZW50IHRvIGdldCBzdG"
        oRequest.Body = oRequest.Body + "FydGVkLiA8YnIgLz4NCjxiciAvPg0KWW91IGNhbiBsb2dpbiB1"
        oRequest.Body = oRequest.Body + "c2luZyBhZG1pbkBhZG1pbi5jb20gYW5kIHRoZSBwYXNzd29yZC"
        oRequest.Body = oRequest.Body + "BhZG1pbi4gPGJyIC8%2BDQo8YnIgLz4NCkJlIHN1cmUgYW5kIG"
        oRequest.Body = oRequest.Body + "NoYW5nZSB0aGUgYWRtaW5pc3RyYXRvciBuYW1lIGFuZCBwYXNz"
        oRequest.Body = oRequest.Body + "d29yZCBvbiB0aGUgcHJvZmlsZSBwYWdlIGFmdGVyIHlvdSBsb2"
        oRequest.Body = oRequest.Body + "dpbi4gPGJyIC8%2BDQo8YnIgLz4NCkFmdGVyIHlvdSBsb2dpbi"
        oRequest.Body = oRequest.Body + "wgeW91IHdpbGwgc2VlIGFuIEFkbWluIG1lbnUgdGhhdCBwcm92"
        oRequest.Body = oRequest.Body + "aWRlcyBmZWF0dXJlcyB0byBjcmVhdGUgbmV3IHBhZ2VzIGFuZC"
        oRequest.Body = oRequest.Body + "BhZGQgY29udGVudCBtb2R1bGVzIHRvIHRoZSBwYWdlcy4gWW91"
        oRequest.Body = oRequest.Body + "IGNhbiBjcmVhdGUgbmV3IHJvbGVzIGFuZCBkZXRlcm1pbmUgd2"
        oRequest.Body = oRequest.Body + "hpY2ggcm9sZXMgY2FuIGVkaXQgY29udGVudCBmb3IgYW55IGNv"
        oRequest.Body = oRequest.Body + "bnRlbnQgbW9kdWxlLiA8YnIgLz4NCjxiciAvPg0KVGhpcyBpcy"
        oRequest.Body = oRequest.Body + "B0aGUgSHRtbCBNb2R1bGUgd2hpY2ggaXMgdGhlIGJhc2ljIGNv"
        oRequest.Body = oRequest.Body + "bnRlbnQgbWFuYWdtZW50IHRvb2wgZm9yIHRoZSBzaXRlLiA8Yn"
        oRequest.Body = oRequest.Body + "IgLz4NCjxiciAvPg0KRm9yIG1vcmUgaW5mbyBvbiB1c2luZyBt"
        oRequest.Body = oRequest.Body + "b2pvUG9ydGFsLCBwbGVhc2UgdmlzaXQgPGEgdGFyZ2V0PSJfYm"
        oRequest.Body = oRequest.Body + "xhbmsiIGhyZWY9Imh0dHA6Ly93d3cubW9qb3BvcnRhbC5jb20i"
        oRequest.Body = oRequest.Body + "Pnd3dy5tb2pvcG9ydGFsLmNvbTwvYT48L3A%2BDQo8cD7CoDwv"
        oRequest.Body = oRequest.Body + "cD4NCjxwPm1tbW1tPC9wPh4IQmFzZVBhdGgFEC9tb2pvL0ZDS2"
        oRequest.Body = oRequest.Body + "VkaXRvci8eBkNvbmZpZzLJBgABAAAA%2F%2F%2F%2F%2FwEAAA"
        oRequest.Body = oRequest.Body + "AAAAAADAIAAABQRnJlZENLLkZDS2VkaXRvclYyLCBWZXJzaW9u"
        oRequest.Body = oRequest.Body + "PTIuMi4yMzk1LjMxMTY3LCBDdWx0dXJlPW5ldXRyYWwsIFB1Ym"
        oRequest.Body = oRequest.Body + "xpY0tleVRva2VuPW51bGwFAQAAACpGcmVkQ0suRkNLZWRpdG9y"
        oRequest.Body = oRequest.Body + "VjIuRkNLZWRpdG9yQ29uZmlndXJhdGlvbnMBAAAAC0NvbmZpZ1"
        oRequest.Body = oRequest.Body + "RhYmxlAxxTeXN0ZW0uQ29sbGVjdGlvbnMuSGFzaHRhYmxlAgAA"
        oRequest.Body = oRequest.Body + "AAkDAAAABAMAAAAcU3lzdGVtLkNvbGxlY3Rpb25zLkhhc2h0YW"
        oRequest.Body = oRequest.Body + "JsZQcAAAAKTG9hZEZhY3RvcgdWZXJzaW9uCENvbXBhcmVyEEhh"
        oRequest.Body = oRequest.Body + "c2hDb2RlUHJvdmlkZXIISGFzaFNpemUES2V5cwZWYWx1ZXMAAA"
        oRequest.Body = oRequest.Body + "MDAAUFCwgcU3lzdGVtLkNvbGxlY3Rpb25zLklDb21wYXJlciRT"
        oRequest.Body = oRequest.Body + "eXN0ZW0uQ29sbGVjdGlvbnMuSUhhc2hDb2RlUHJvdmlkZXII7F"
        oRequest.Body = oRequest.Body + "E4PwQAAAAKCgsAAAAJBAAAAAkFAAAAEAQAAAAEAAAABgYAAAAP"
        oRequest.Body = oRequest.Body + "SW1hZ2VCcm93c2VyVVJMBgcAAAANRWRpdG9yQXJlYUNTUwYIAA"
        oRequest.Body = oRequest.Body + "AADkxpbmtCcm93c2VyVVJMBgkAAAAIU2tpblBhdGgQBQAAAAQA"
        oRequest.Body = oRequest.Body + "AAAGCgAAAHMvbW9qby9GQ0tlZGl0b3IvZWRpdG9yL2ZpbGVtYW"
        oRequest.Body = oRequest.Body + "5hZ2VyL2Jyb3dzZXIvZGVmYXVsdC9icm93c2VyLmh0bWw%2FVH"
        oRequest.Body = oRequest.Body + "lwZT1JbWFnZSZDb25uZWN0b3I9Y29ubmVjdG9ycy9hc3B4L2Nv"
        oRequest.Body = oRequest.Body + "bm5lY3Rvci5hc3B4BgsAAAA7aHR0cDovL2xvYWR0ZXN0L21vam"
        oRequest.Body = oRequest.Body + "8vRGF0YS9TaXRlcy8xL3NraW5zL2NlbnRlcmVkMS9zdHlsZS5j"
        oRequest.Body = oRequest.Body + "c3MGDAAAAGgvbW9qby9GQ0tlZGl0b3IvZWRpdG9yL2ZpbGVtYW"
        oRequest.Body = oRequest.Body + "5hZ2VyL2Jyb3dzZXIvZGVmYXVsdC9icm93c2VyLmh0bWw%2FQ2"
        oRequest.Body = oRequest.Body + "9ubmVjdG9yPWNvbm5lY3RvcnMvYXNweC9jb25uZWN0b3IuYXNw"
        oRequest.Body = oRequest.Body + "eAYNAAAAJC9tb2pvL0ZDS2VkaXRvci9lZGl0b3Ivc2tpbnMvbm"
        oRequest.Body = oRequest.Body + "9ybWFsLwseClRvb2xiYXJTZXQFBEZ1bGxkAgcPDxYIHgpDbG9j"
        oRequest.Body = oRequest.Body + "a0hvdXJzBQIxMh4ITGFuZ3VhZ2UFAmVuHgxMYW5ndWFnZUZpbG"
        oRequest.Body = oRequest.Body + "UFDmNhbGVuZGFyLWVuLmpzHgxGb3JtYXRTdHJpbmcFESVtLyVk"
        oRequest.Body = oRequest.Body + "LyVZICVJOiVNICVwZGQCCw8PFggfCgUCMTIfCwUCZW4fDAUOY2"
        oRequest.Body = oRequest.Body + "FsZW5kYXItZW4uanMfDQURJW0vJWQvJVkgJUk6JU0gJXBkZAIN"
        oRequest.Body = oRequest.Body + "DxYCHwFoZAIPDxYCHwFoZAIRDxYCHwFoZAIZDw8WAh4MRXJyb3"
        oRequest.Body = oRequest.Body + "JNZXNzYWdlBSFZb3UgTXVzdCBFbnRlciBhIFZhbGlkIEJlZ2lu"
        oRequest.Body = oRequest.Body + "IERhdGVkZAIbDw8WAh8OBR9Zb3UgTXVzdCBFbnRlciBhIFZhbG"
        oRequest.Body = oRequest.Body + "lkIEVuZCBEYXRlZGQCHw8PFgQfAgUOVXBkYXRlIFtBbHQrMV0e"
        oRequest.Body = oRequest.Body + "CUFjY2Vzc0tleQUBMWRkAiEPDxYEHw8FATIfAgUOQ2FuY2VsIF"
        oRequest.Body = oRequest.Body + "tBbHQrMl1kZAIjDw8WBh8PBQEzHwFoHwIFDkRlbGV0ZSBbQWx0"
        oRequest.Body = oRequest.Body + "KzNdZGQCJQ9kFgJmDxYGHgRocmVmBSsvbW9qby9IZWxwLmFzcH"
        oRequest.Body = oRequest.Body + "g%2FaGVscGtleT1odG1sY29udGVudGVkaXRoZWxwHgNyZWwFGW"
        oRequest.Body = oRequest.Body + "lib3gmaGVpZ2h0PTM1MCZ3aWR0aD00MDAeBXRpdGxlZBYCZg8W"
        oRequest.Body = oRequest.Body + "Bh4Dc3JjBSsvbW9qby9EYXRhL1NpdGVJbWFnZXMvRmVhdHVyZU"
        oRequest.Body = oRequest.Body + "ljb25zL2hlbHAuZ2lmHgNhbHRkHgZib3JkZXIFATBkAg8PDxYC"
        oRequest.Body = oRequest.Body + "HwFoZGQYAQUVY3RsMDAkU2l0ZU1lbnUxJGN0bDAwDw9kBQRIb2"
        oRequest.Body = oRequest.Body + "1lZJ5ZZChcN1hCP4CoSvpAvR%2F9sDn1&ctl00%24SearchInp"
        oRequest.Body = oRequest.Body + "ut1%24txtSearch=enter+search+terms&ctl00%24mainCon"
        oRequest.Body = oRequest.Body + "tent%24FCKTextBox0=%3Cp%3EWelcome+to+%3Cstrong%3Em"
        oRequest.Body = oRequest.Body + "ojoPortal%3C%2Fstrong%3E%2C+this+is+sample+data%2C"
        oRequest.Body = oRequest.Body + "+you+can+edit+this+content+to+get+started.+%3Cbr+%"
        oRequest.Body = oRequest.Body + "2F%3E%0D%0A%3Cbr+%2F%3E%0D%0AYou+can+login+using+a"
        oRequest.Body = oRequest.Body + "dmin@admin.com+and+the+password+admin.+%3Cbr+%2F%3"
        oRequest.Body = oRequest.Body + "E%0D%0A%3Cbr+%2F%3E%0D%0ABe+sure+and+change+the+ad"
        oRequest.Body = oRequest.Body + "ministrator+name+and+password+on+the+profile+page+"
        oRequest.Body = oRequest.Body + "after+you+login.+%3Cbr+%2F%3E%0D%0A%3Cbr+%2F%3E%0D"
        oRequest.Body = oRequest.Body + "%0AAfter+you+login%2C+you+will+see+an+Admin+menu+t"
        oRequest.Body = oRequest.Body + "hat+provides+features+to+create+new+pages+and+add+"
        oRequest.Body = oRequest.Body + "content+modules+to+the+pages.+You+can+create+new+r"
        oRequest.Body = oRequest.Body + "oles+and+determine+which+roles+can+edit+content+fo"
        oRequest.Body = oRequest.Body + "r+any+content+module.+%3Cbr+%2F%3E%0D%0A%3Cbr+%2F%"
        oRequest.Body = oRequest.Body + "3E%0D%0AThis+is+the+Html+Module+which+is+the+basic"
        oRequest.Body = oRequest.Body + "+content+managment+tool+for+the+site.+%3Cbr+%2F%3E"
        oRequest.Body = oRequest.Body + "%0D%0A%3Cbr+%2F%3E%0D%0AFor+more+info+on+using+moj"
        oRequest.Body = oRequest.Body + "oPortal%2C+please+visit+%3Ca+target%3D%22_blank%22"
        oRequest.Body = oRequest.Body + "+href%3D%22http%3A%2F%2Fwww.mojoportal.com%22%3Eww"
        oRequest.Body = oRequest.Body + "w.mojoportal.com%3C%2Fa%3E%3C%2Fp%3E%0D%0A%3Cp%3E%"
        oRequest.Body = oRequest.Body + "26nbsp%3B%3C%2Fp%3E%0D%0A%3Cp%3Emmmmm%3C%2Fp%3E%0D"
        oRequest.Body = oRequest.Body + "%0A%3Cp%3Emmm%3C%2Fp%3E&ctl00%24mainContent%24dpBe"
        oRequest.Body = oRequest.Body + "ginDate%24ctl00=7%2F22%2F2006+3%3A51%3A48+PM&ctl00"
        oRequest.Body = oRequest.Body + "%24mainContent%24dpEndDate%24ctl00=7%2F22%2F2056+4"
        oRequest.Body = oRequest.Body + "%3A11%3A48+PM&ctl00%24mainContent%24txtExcerpt=&ct"
        oRequest.Body = oRequest.Body + "l00%24mainContent%24btnUpdate=Update+%5BAlt%2B1%5D"
        oRequest.Body = oRequest.Body + "&__PREVIOUSPAGE=HObk24Sfe39-RZD1xKPEiujVFlrqTG6dUy"
        oRequest.Body = oRequest.Body + "sTomCnIy41&__EVENTVALIDATION=%2FwEWCAKmtP2VAQLs%2F"
        oRequest.Body = oRequest.Body + "MSMCAK4gcXfBgLO%2BKKbBgKdwID%2FCgKb1%2BXnAgKgvuvCA"
        oRequest.Body = oRequest.Body + "gL0%2FaO7BwZnPtbS7diVqvFcCI0SqlOUvA1z"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/HtmlEdit.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest59()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (519)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/HtmlEdit.aspx?mid=0&pageindex=&pageid=0"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest60()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (109)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=wzYMextq5JuPmaa3AykFmQ2&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest61()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (1429)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Logoff.aspx"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=Admin; portalroles=E5887D3EB76897556DE381FDF3D93CFBD64ADA327970F0ABE8D7546EE87030A0BD85AA1858CABB7BBB61FD6FCF3570B2848E727A0B1832C549A5EC8920A1347C4953876CE833C798EDBE23151E462790EEC756227BA4EA66B617E1DE8DC1C519A4317DF4A632FB34A8122E17E7CB5FBCF31BC5F84EB5BB2E813AD67EDBF56AD8; .ASPXAUTH=50BE9BFBEC9B7F226F453FB0F6240D9A17F35F7E10955299BD59EBF451C07B64D432A3FBF326A4EBA1E60104F05022581D7885D9A32D5E38753E548D1946E05280632BECF5C1D6F25C2623DA64DBB454"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Logoff.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest62()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (42)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest63()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (14)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest64()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (84)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/WebResource.axd"+"?d=wzYMextq5JuPmaa3AykFmQ2&t=632883361864062500"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; DisplayName=; portalroles="
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/WebResource.axd"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub




Sub Main()
    call SendRequest1()
    call SendRequest2()
    call SendRequest3()
    call SendRequest4()
    call SendRequest5()
    call SendRequest6()
    call SendRequest7()
    call SendRequest8()
    call SendRequest9()
    call SendRequest10()
    call SendRequest11()
    call SendRequest12()
    call SendRequest13()
    call SendRequest14()
    call SendRequest15()
    call SendRequest16()
    call SendRequest17()
    call SendRequest18()
    call SendRequest19()
    call SendRequest20()
    call SendRequest21()
    call SendRequest22()
    call SendRequest23()
    call SendRequest24()
    call SendRequest25()
    call SendRequest26()
    call SendRequest27()
    call SendRequest28()
    call SendRequest29()
    call SendRequest30()
    call SendRequest31()
    call SendRequest32()
    call SendRequest33()
    call SendRequest34()
    call SendRequest35()
    call SendRequest36()
    call SendRequest37()
    call SendRequest38()
    call SendRequest39()
    call SendRequest40()
    call SendRequest41()
    call SendRequest42()
    call SendRequest43()
    call SendRequest44()
    call SendRequest45()
    call SendRequest46()
    call SendRequest47()
    call SendRequest48()
    call SendRequest49()
    call SendRequest50()
    call SendRequest51()
    call SendRequest52()
    call SendRequest53()
    call SendRequest54()
    call SendRequest55()
    call SendRequest56()
    call SendRequest57()
    call SendRequest58()
    call SendRequest59()
    call SendRequest60()
    call SendRequest61()
    call SendRequest62()
    call SendRequest63()
    call SendRequest64()
End Sub
Main
