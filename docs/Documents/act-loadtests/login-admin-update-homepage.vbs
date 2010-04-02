Option Explicit
Dim fEnableDelays
fEnableDelays = False

Sub SendRequest1()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (0)
    Set oConnection = Test.CreateConnection("imisvr5", 5440, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to imisvr5"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/CSIS/CSISISAPI.dll/"+"?request?75406f4e-238e-42f6-ab5f-be4e78faa3ca;CCSISSvrCONN%3A%3AwaitForEvent%3B3%3B300%3B3%3B120"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "User-Agent", "CSISHttpReq"
        'oHeaders.Add "Host", "imisvr5:5440"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/CSIS/CSISISAPI.dll/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest2()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (36767)
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

Sub SendRequest3()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (18)
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

Sub SendRequest4()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (7824)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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
    If fEnableDelays = True then Test.Sleep (1)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest6()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (46)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest7()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (42)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest8()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (36)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest9()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (44)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest10()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (37)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest11()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (5)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest12()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (73)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest13()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (37)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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
    If fEnableDelays = True then Test.Sleep (22)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest15()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (55)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest16()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (4563)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest17()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (255)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest19()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (7959)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel"
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

Sub SendRequest20()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (619)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin"
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

Sub SendRequest21()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (261)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest22()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (55)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (1)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest24()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (11)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (68)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest26()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (8)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (3)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (1)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest29()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (8696)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest30()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (378)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest31()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (28)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest32()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (2)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest33()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (85)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (1)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest35()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (72)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest36()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (40)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest37()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (56)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest38()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (67)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (4)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest40()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (43)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest41()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (2)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest42()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (138)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest43()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (1)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest44()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (67)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest45()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (23)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (7)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest47()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (10)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest48()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (7)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest49()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (13)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest50()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (84)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest51()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (10)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest52()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (80)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest53()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (122)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest54()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (151)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    If fEnableDelays = True then Test.Sleep (3)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest56()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (24)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest57()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (25)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest58()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (34)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest59()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (9)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest60()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (37)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest61()
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest62()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (88)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest63()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (2821)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest64()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (37)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest65()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (4410)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "Content-Length", "(automatic)" 
        oRequest.Body = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEP"
        oRequest.Body = oRequest.Body + "DwUKMjA3NDQwNDE5NA8WAh4LVXJsUmVmZXJyZXIFFWh0dHA6Ly"
        oRequest.Body = oRequest.Body + "9sb2FkdGVzdC9tb2pvLxYCZg9kFgICAw9kFggCBQ8PFgIeB1Zp"
        oRequest.Body = oRequest.Body + "c2libGVoZGQCCw9kFgICAg8PFgIeBFRleHQFBlNlYXJjaGRkAg"
        oRequest.Body = oRequest.Body + "4PDxYEHghDc3NDbGFzcwURY2VudGVyLWxlZnRtYXJnaW4eBF8h"
        oRequest.Body = oRequest.Body + "U0ICAmQWAgIFD2QWGAIDDxYKHgZIZWlnaHQbAAAAAADgdUABAA"
        oRequest.Body = oRequest.Body + "AAHgVWYWx1ZQXiBVdlbGNvbWUgdG8gPGI%2BbW9qb1BvcnRhbD"
        oRequest.Body = oRequest.Body + "wvYj4sIHRoaXMgaXMgc2FtcGxlIGRhdGEsIHlvdSBjYW4gZWRp"
        oRequest.Body = oRequest.Body + "dCB0aGlzIGNvbnRlbnQgdG8gZ2V0IHN0YXJ0ZWQuDQo8YnIgLz"
        oRequest.Body = oRequest.Body + "48YnIgLz5Zb3UgY2FuIGxvZ2luIHVzaW5nIGFkbWluQGFkbWlu"
        oRequest.Body = oRequest.Body + "LmNvbSBhbmQgdGhlIHBhc3N3b3JkIGFkbWluLiANCjxiciAvPj"
        oRequest.Body = oRequest.Body + "xiciAvPkJlIHN1cmUgYW5kIGNoYW5nZSB0aGUgYWRtaW5pc3Ry"
        oRequest.Body = oRequest.Body + "YXRvciBuYW1lIGFuZCBwYXNzd29yZCBvbiB0aGUgcHJvZmlsZS"
        oRequest.Body = oRequest.Body + "BwYWdlIGFmdGVyIHlvdSBsb2dpbi4gDQo8YnIgLz48YnIgLz5B"
        oRequest.Body = oRequest.Body + "ZnRlciB5b3UgbG9naW4sIHlvdSB3aWxsIHNlZSBhbiBBZG1pbi"
        oRequest.Body = oRequest.Body + "BtZW51IHRoYXQgcHJvdmlkZXMgZmVhdHVyZXMgdG8gY3JlYXRl"
        oRequest.Body = oRequest.Body + "IG5ldyBwYWdlcyBhbmQgDQphZGQgY29udGVudCBtb2R1bGVzIH"
        oRequest.Body = oRequest.Body + "RvIHRoZSBwYWdlcy4gWW91IGNhbiBjcmVhdGUgbmV3IHJvbGVz"
        oRequest.Body = oRequest.Body + "IGFuZCBkZXRlcm1pbmUgd2hpY2ggcm9sZXMgY2FuIGVkaXQgY2"
        oRequest.Body = oRequest.Body + "9udGVudCANCmZvciBhbnkgY29udGVudCBtb2R1bGUuIA0KPGJy"
        oRequest.Body = oRequest.Body + "IC8%2BPGJyIC8%2BVGhpcyBpcyB0aGUgSHRtbCBNb2R1bGUgd2"
        oRequest.Body = oRequest.Body + "hpY2ggaXMgdGhlIGJhc2ljIGNvbnRlbnQgbWFuYWdtZW50IHRv"
        oRequest.Body = oRequest.Body + "b2wgZm9yIHRoZSBzaXRlLg0KPGJyIC8%2BPGJyIC8%2BRm9yIG"
        oRequest.Body = oRequest.Body + "1vcmUgaW5mbyBvbiB1c2luZyBtb2pvUG9ydGFsLCBwbGVhc2Ug"
        oRequest.Body = oRequest.Body + "dmlzaXQgDQo8YSBocmVmPSJodHRwOi8vd3d3Lm1vam9wb3J0YW"
        oRequest.Body = oRequest.Body + "wuY29tIiB0YXJnZXQ9Il9ibGFuayI%2Bd3d3Lm1vam9wb3J0YW"
        oRequest.Body = oRequest.Body + "wuY29tPC9hPh4IQmFzZVBhdGgFEC9tb2pvL0ZDS2VkaXRvci8e"
        oRequest.Body = oRequest.Body + "BkNvbmZpZzLJBgABAAAA%2F%2F%2F%2F%2FwEAAAAAAAAADAIA"
        oRequest.Body = oRequest.Body + "AABQRnJlZENLLkZDS2VkaXRvclYyLCBWZXJzaW9uPTIuMi4yMz"
        oRequest.Body = oRequest.Body + "k1LjMxMTY3LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRv"
        oRequest.Body = oRequest.Body + "a2VuPW51bGwFAQAAACpGcmVkQ0suRkNLZWRpdG9yVjIuRkNLZW"
        oRequest.Body = oRequest.Body + "RpdG9yQ29uZmlndXJhdGlvbnMBAAAAC0NvbmZpZ1RhYmxlAxxT"
        oRequest.Body = oRequest.Body + "eXN0ZW0uQ29sbGVjdGlvbnMuSGFzaHRhYmxlAgAAAAkDAAAABA"
        oRequest.Body = oRequest.Body + "MAAAAcU3lzdGVtLkNvbGxlY3Rpb25zLkhhc2h0YWJsZQcAAAAK"
        oRequest.Body = oRequest.Body + "TG9hZEZhY3RvcgdWZXJzaW9uCENvbXBhcmVyEEhhc2hDb2RlUH"
        oRequest.Body = oRequest.Body + "JvdmlkZXIISGFzaFNpemUES2V5cwZWYWx1ZXMAAAMDAAUFCwgc"
        oRequest.Body = oRequest.Body + "U3lzdGVtLkNvbGxlY3Rpb25zLklDb21wYXJlciRTeXN0ZW0uQ2"
        oRequest.Body = oRequest.Body + "9sbGVjdGlvbnMuSUhhc2hDb2RlUHJvdmlkZXII7FE4PwQAAAAK"
        oRequest.Body = oRequest.Body + "CgsAAAAJBAAAAAkFAAAAEAQAAAAEAAAABgYAAAAPSW1hZ2VCcm"
        oRequest.Body = oRequest.Body + "93c2VyVVJMBgcAAAANRWRpdG9yQXJlYUNTUwYIAAAADkxpbmtC"
        oRequest.Body = oRequest.Body + "cm93c2VyVVJMBgkAAAAIU2tpblBhdGgQBQAAAAQAAAAGCgAAAH"
        oRequest.Body = oRequest.Body + "MvbW9qby9GQ0tlZGl0b3IvZWRpdG9yL2ZpbGVtYW5hZ2VyL2Jy"
        oRequest.Body = oRequest.Body + "b3dzZXIvZGVmYXVsdC9icm93c2VyLmh0bWw%2FVHlwZT1JbWFn"
        oRequest.Body = oRequest.Body + "ZSZDb25uZWN0b3I9Y29ubmVjdG9ycy9hc3B4L2Nvbm5lY3Rvci"
        oRequest.Body = oRequest.Body + "5hc3B4BgsAAAA7aHR0cDovL2xvYWR0ZXN0L21vam8vRGF0YS9T"
        oRequest.Body = oRequest.Body + "aXRlcy8xL3NraW5zL2NlbnRlcmVkMS9zdHlsZS5jc3MGDAAAAG"
        oRequest.Body = oRequest.Body + "gvbW9qby9GQ0tlZGl0b3IvZWRpdG9yL2ZpbGVtYW5hZ2VyL2Jy"
        oRequest.Body = oRequest.Body + "b3dzZXIvZGVmYXVsdC9icm93c2VyLmh0bWw%2FQ29ubmVjdG9y"
        oRequest.Body = oRequest.Body + "PWNvbm5lY3RvcnMvYXNweC9jb25uZWN0b3IuYXNweAYNAAAAJC"
        oRequest.Body = oRequest.Body + "9tb2pvL0ZDS2VkaXRvci9lZGl0b3Ivc2tpbnMvbm9ybWFsLwse"
        oRequest.Body = oRequest.Body + "ClRvb2xiYXJTZXQFBEZ1bGxkAgcPDxYIHgpDbG9ja0hvdXJzBQ"
        oRequest.Body = oRequest.Body + "IxMh4ITGFuZ3VhZ2UFAmVuHgxMYW5ndWFnZUZpbGUFDmNhbGVu"
        oRequest.Body = oRequest.Body + "ZGFyLWVuLmpzHgxGb3JtYXRTdHJpbmcFESVtLyVkLyVZICVJOi"
        oRequest.Body = oRequest.Body + "VNICVwZGQCCw8PFggfCgUCMTIfCwUCZW4fDAUOY2FsZW5kYXIt"
        oRequest.Body = oRequest.Body + "ZW4uanMfDQURJW0vJWQvJVkgJUk6JU0gJXBkZAINDxYCHwFoZA"
        oRequest.Body = oRequest.Body + "IPDxYCHwFoZAIRDxYCHwFoZAIZDw8WAh4MRXJyb3JNZXNzYWdl"
        oRequest.Body = oRequest.Body + "BSFZb3UgTXVzdCBFbnRlciBhIFZhbGlkIEJlZ2luIERhdGVkZA"
        oRequest.Body = oRequest.Body + "IbDw8WAh8OBR9Zb3UgTXVzdCBFbnRlciBhIFZhbGlkIEVuZCBE"
        oRequest.Body = oRequest.Body + "YXRlZGQCHw8PFgQfAgUOVXBkYXRlIFtBbHQrMV0eCUFjY2Vzc0"
        oRequest.Body = oRequest.Body + "tleQUBMWRkAiEPDxYEHw8FATIfAgUOQ2FuY2VsIFtBbHQrMl1k"
        oRequest.Body = oRequest.Body + "ZAIjDw8WBh8PBQEzHwFoHwIFDkRlbGV0ZSBbQWx0KzNdZGQCJQ"
        oRequest.Body = oRequest.Body + "9kFgJmDxYGHgRocmVmBSsvbW9qby9IZWxwLmFzcHg%2FaGVscG"
        oRequest.Body = oRequest.Body + "tleT1odG1sY29udGVudGVkaXRoZWxwHgNyZWwFGWlib3gmaGVp"
        oRequest.Body = oRequest.Body + "Z2h0PTM1MCZ3aWR0aD00MDAeBXRpdGxlZBYCZg8WBh4Dc3JjBS"
        oRequest.Body = oRequest.Body + "svbW9qby9EYXRhL1NpdGVJbWFnZXMvRmVhdHVyZUljb25zL2hl"
        oRequest.Body = oRequest.Body + "bHAuZ2lmHgNhbHRkHgZib3JkZXIFATBkAg8PDxYCHwFoZGQYAQ"
        oRequest.Body = oRequest.Body + "UVY3RsMDAkU2l0ZU1lbnUxJGN0bDAwDw9kBQRIb21lZDZUCoBh"
        oRequest.Body = oRequest.Body + "JjN1H62PdPZQfJZB7H%2Fp&ctl00%24SearchInput1%24txtS"
        oRequest.Body = oRequest.Body + "earch=enter+search+terms&ctl00%24mainContent%24FCK"
        oRequest.Body = oRequest.Body + "TextBox0=%3Cp%3EWelcome+to+%3Cstrong%3EmojoPortal%"
        oRequest.Body = oRequest.Body + "3C%2Fstrong%3E%2C+this+is+sample+data%2C+you+can+e"
        oRequest.Body = oRequest.Body + "dit+this+content+to+get+started.+%3Cbr+%2F%3E%0D%0"
        oRequest.Body = oRequest.Body + "A%3Cbr+%2F%3E%0D%0AYou+can+login+using+admin@admin"
        oRequest.Body = oRequest.Body + ".com+and+the+password+admin.+%3Cbr+%2F%3E%0D%0A%3C"
        oRequest.Body = oRequest.Body + "br+%2F%3E%0D%0ABe+sure+and+change+the+administrato"
        oRequest.Body = oRequest.Body + "r+name+and+password+on+the+profile+page+after+you+"
        oRequest.Body = oRequest.Body + "login.+%3Cbr+%2F%3E%0D%0A%3Cbr+%2F%3E%0D%0AAfter+y"
        oRequest.Body = oRequest.Body + "ou+login%2C+you+will+see+an+Admin+menu+that+provid"
        oRequest.Body = oRequest.Body + "es+features+to+create+new+pages+and+add+content+mo"
        oRequest.Body = oRequest.Body + "dules+to+the+pages.+You+can+create+new+roles+and+d"
        oRequest.Body = oRequest.Body + "etermine+which+roles+can+edit+content+for+any+cont"
        oRequest.Body = oRequest.Body + "ent+module.+%3Cbr+%2F%3E%0D%0A%3Cbr+%2F%3E%0D%0ATh"
        oRequest.Body = oRequest.Body + "is+is+the+Html+Module+which+is+the+basic+content+m"
        oRequest.Body = oRequest.Body + "anagment+tool+for+the+site.+%3Cbr+%2F%3E%0D%0A%3Cb"
        oRequest.Body = oRequest.Body + "r+%2F%3E%0D%0AFor+more+info+on+using+mojoPortal%2C"
        oRequest.Body = oRequest.Body + "+please+visit+%3Ca+target%3D%22_blank%22+href%3D%2"
        oRequest.Body = oRequest.Body + "2http%3A%2F%2Fwww.mojoportal.com%22%3Ewww.mojoport"
        oRequest.Body = oRequest.Body + "al.com%3C%2Fa%3E%3C%2Fp%3E%0D%0A%3Cp%3E%26nbsp%3B%"
        oRequest.Body = oRequest.Body + "3C%2Fp%3E%0D%0A%3Cp%3Emmmmm%3C%2Fp%3E&ctl00%24main"
        oRequest.Body = oRequest.Body + "Content%24dpBeginDate%24ctl00=7%2F22%2F2006+3%3A51"
        oRequest.Body = oRequest.Body + "%3A48+PM&ctl00%24mainContent%24dpEndDate%24ctl00=7"
        oRequest.Body = oRequest.Body + "%2F22%2F2056+4%3A11%3A48+PM&ctl00%24mainContent%24"
        oRequest.Body = oRequest.Body + "txtExcerpt=&ctl00%24mainContent%24btnUpdate=Update"
        oRequest.Body = oRequest.Body + "+%5BAlt%2B1%5D&__PREVIOUSPAGE=HObk24Sfe39-RZD1xKPE"
        oRequest.Body = oRequest.Body + "iujVFlrqTG6dUysTomCnIy41&__EVENTVALIDATION=%2FwEWC"
        oRequest.Body = oRequest.Body + "ALz54D7BQLs%2FMSMCAK4gcXfBgLO%2BKKbBgKdwID%2FCgKb1"
        oRequest.Body = oRequest.Body + "%2BXnAgKgvuvCAgL0%2FaO7B2LMcz4w%2FzdHqhvTH5%2FJVin"
        oRequest.Body = oRequest.Body + "tMreB"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/HtmlEdit.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest66()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (1438)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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

Sub SendRequest67()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (130)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=xisd4qz152bk54nbon231fel; .ASPXAUTH=9F2956707FA81F2C222AB0DD4787F896816F0CD2927E49DEC8DC87D133CB3009F2E2870601BE8E0DF5D3EEDC67F56D96C9BD0066CC538EA5B94302478D3AC506A473CB1DE3F79C0998735166C74555EB; DisplayName=Admin; portalroles=6D97951A47120372BCB31A485236FF8A604A62BFD1FA68080AB82A8888C929BBB310C222192A214DEB2927D08A1D1E16CD2035353701628B881973E58AFFDDFB5FE59F0F2EF7ACF3C6F12828D343B9BC66C88F1A430433964C658747E7FFC34126B080A5A2EA07CA2F57B38F3E2A5B331FE24FC13F913D2D7899D383A424C5DC"
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
    call SendRequest65()
    call SendRequest66()
    call SendRequest67()
End Sub
Main
