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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (5)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (56)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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

Sub SendRequest4()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (20)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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

Sub SendRequest5()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (4)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (36)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (201)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (29)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (12)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (7)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (6)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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

Sub SendRequest13()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (8)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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

Sub SendRequest14()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (8)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (3081)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (299)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (115)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (7700)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv"
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
    If fEnableDelays = True then Test.Sleep (581)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin"
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
    If fEnableDelays = True then Test.Sleep (176)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (116)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (7)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (10)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (11)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest26()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (19)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest27()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (43)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest28()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (13998)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/skins/centered1/activeArrowRight.gif"
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/skins/centered1/activeArrowRight.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest29()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (2411)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Default.aspx"+"?pageindex=3&pageid=1"
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Default.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest30()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (342)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/Default.aspx?pageindex=3&pageid=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (2120)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumView.aspx"+"?ItemID=1&mid=2&pageindex=3&pageid=1"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/Default.aspx?pageindex=3&pageid=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumView.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest32()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (827)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumView.aspx?ItemID=1&mid=2&pageindex=3&pageid=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest33()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (169)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/SiteImages/thread.gif"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumView.aspx?ItemID=1&mid=2&pageindex=3&pageid=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/SiteImages/thread.gif"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest34()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (3325)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumPostEdit.aspx"+"?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumView.aspx?ItemID=1&mid=2&pageindex=3&pageid=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumPostEdit.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest35()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (270)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest36()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (82)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest37()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (24)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest38()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (136)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest39()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (100)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest40()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (61)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest41()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (166)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest42()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (70)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest43()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (128)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest44()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (30)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest45()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (111)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest46()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (29)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (16)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest48()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (14)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest49()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (59)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (163)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (16)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (137)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (163)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (175)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (111)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (1536)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (15)
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
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (13371)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumPostEdit.aspx"+"?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oRequest.Verb = "POST"
        oRequest.HTTPVersion = "HTTP/1.0"
        oRequest.EncodeBody = False
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "Content-Type", "application/x-www-form-urlencoded"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "Content-Length", "(automatic)" 
        oRequest.Body = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEP"
        oRequest.Body = oRequest.Body + "DwUJODk1NTk5NDE1DxYCHgtVcmxSZWZlcnJlcgVHaHR0cDovL2"
        oRequest.Body = oRequest.Body + "xvYWR0ZXN0L21vam8vRm9ydW1WaWV3LmFzcHg%2FSXRlbUlEPT"
        oRequest.Body = oRequest.Body + "EmbWlkPTImcGFnZWluZGV4PTMmcGFnZWlkPTEWAmYPZBYCAgMP"
        oRequest.Body = oRequest.Body + "ZBYIAgUPDxYCHgdWaXNpYmxlaGRkAgsPZBYCAgIPDxYCHgRUZX"
        oRequest.Body = oRequest.Body + "h0BQZTZWFyY2hkZAIODw8WBB4IQ3NzQ2xhc3MFEWNlbnRlci1s"
        oRequest.Body = oRequest.Body + "ZWZ0bWFyZ2luHgRfIVNCAgJkFgICBQ9kFg4CAQ8PFgIfAgUMU2"
        oRequest.Body = oRequest.Body + "FtcGxlIEZvcnVtZGQCAw8WBh4IQmFzZVBhdGgFEC9tb2pvL0ZD"
        oRequest.Body = oRequest.Body + "S2VkaXRvci8eBkNvbmZpZzLJBgABAAAA%2F%2F%2F%2F%2FwEA"
        oRequest.Body = oRequest.Body + "AAAAAAAADAIAAABQRnJlZENLLkZDS2VkaXRvclYyLCBWZXJzaW"
        oRequest.Body = oRequest.Body + "9uPTIuMi4yMzk1LjMxMTY3LCBDdWx0dXJlPW5ldXRyYWwsIFB1"
        oRequest.Body = oRequest.Body + "YmxpY0tleVRva2VuPW51bGwFAQAAACpGcmVkQ0suRkNLZWRpdG"
        oRequest.Body = oRequest.Body + "9yVjIuRkNLZWRpdG9yQ29uZmlndXJhdGlvbnMBAAAAC0NvbmZp"
        oRequest.Body = oRequest.Body + "Z1RhYmxlAxxTeXN0ZW0uQ29sbGVjdGlvbnMuSGFzaHRhYmxlAg"
        oRequest.Body = oRequest.Body + "AAAAkDAAAABAMAAAAcU3lzdGVtLkNvbGxlY3Rpb25zLkhhc2h0"
        oRequest.Body = oRequest.Body + "YWJsZQcAAAAKTG9hZEZhY3RvcgdWZXJzaW9uCENvbXBhcmVyEE"
        oRequest.Body = oRequest.Body + "hhc2hDb2RlUHJvdmlkZXIISGFzaFNpemUES2V5cwZWYWx1ZXMA"
        oRequest.Body = oRequest.Body + "AAMDAAUFCwgcU3lzdGVtLkNvbGxlY3Rpb25zLklDb21wYXJlci"
        oRequest.Body = oRequest.Body + "RTeXN0ZW0uQ29sbGVjdGlvbnMuSUhhc2hDb2RlUHJvdmlkZXII"
        oRequest.Body = oRequest.Body + "7FE4PwQAAAAKCgsAAAAJBAAAAAkFAAAAEAQAAAAEAAAABgYAAA"
        oRequest.Body = oRequest.Body + "APSW1hZ2VCcm93c2VyVVJMBgcAAAANRWRpdG9yQXJlYUNTUwYI"
        oRequest.Body = oRequest.Body + "AAAADkxpbmtCcm93c2VyVVJMBgkAAAAIU2tpblBhdGgQBQAAAA"
        oRequest.Body = oRequest.Body + "QAAAAGCgAAAHMvbW9qby9GQ0tlZGl0b3IvZWRpdG9yL2ZpbGVt"
        oRequest.Body = oRequest.Body + "YW5hZ2VyL2Jyb3dzZXIvZGVmYXVsdC9icm93c2VyLmh0bWw%2F"
        oRequest.Body = oRequest.Body + "VHlwZT1JbWFnZSZDb25uZWN0b3I9Y29ubmVjdG9ycy9hc3B4L2"
        oRequest.Body = oRequest.Body + "Nvbm5lY3Rvci5hc3B4BgsAAAA7aHR0cDovL2xvYWR0ZXN0L21v"
        oRequest.Body = oRequest.Body + "am8vRGF0YS9TaXRlcy8xL3NraW5zL2NlbnRlcmVkMS9zdHlsZS"
        oRequest.Body = oRequest.Body + "5jc3MGDAAAAGgvbW9qby9GQ0tlZGl0b3IvZWRpdG9yL2ZpbGVt"
        oRequest.Body = oRequest.Body + "YW5hZ2VyL2Jyb3dzZXIvZGVmYXVsdC9icm93c2VyLmh0bWw%2F"
        oRequest.Body = oRequest.Body + "Q29ubmVjdG9yPWNvbm5lY3RvcnMvYXNweC9jb25uZWN0b3IuYX"
        oRequest.Body = oRequest.Body + "NweAYNAAAAJC9tb2pvL0ZDS2VkaXRvci9lZGl0b3Ivc2tpbnMv"
        oRequest.Body = oRequest.Body + "bm9ybWFsLwseClRvb2xiYXJTZXQFBEZ1bGxkAg0PDxYCHgxFcn"
        oRequest.Body = oRequest.Body + "Jvck1lc3NhZ2UFGFlvdSBNdXN0IEVudGVyIGEgU3ViamVjdGRk"
        oRequest.Body = oRequest.Body + "Ag8PDxYEHwIFDFBvc3QgW0FsdCsxXR4JQWNjZXNzS2V5BQExZG"
        oRequest.Body = oRequest.Body + "QCEQ8PFgQfAgUOQ2FuY2VsIFtBbHQrMl0fCQUBMmRkAhMPDxYG"
        oRequest.Body = oRequest.Body + "HwFoHwIFGERlbGV0ZSB0aGlzIGl0ZW0gW0FsdCszXR8JBQEzZG"
        oRequest.Body = oRequest.Body + "QCFQ9kFgJmDxYGHgRocmVmBSkvbW9qby9IZWxwLmFzcHg%2FaG"
        oRequest.Body = oRequest.Body + "VscGtleT1mb3J1bXBvc3RlZGl0aGVscB4DcmVsBRlpYm94Jmhl"
        oRequest.Body = oRequest.Body + "aWdodD0zNTAmd2lkdGg9NDAwHgV0aXRsZWQWAmYPFgYeA3NyYw"
        oRequest.Body = oRequest.Body + "UrL21vam8vRGF0YS9TaXRlSW1hZ2VzL0ZlYXR1cmVJY29ucy9o"
        oRequest.Body = oRequest.Body + "ZWxwLmdpZh4DYWx0ZB4GYm9yZGVyBQEwZAIPDw8WAh8BaGRkGA"
        oRequest.Body = oRequest.Body + "IFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBSJj"
        oRequest.Body = oRequest.Body + "dGwwMCRtYWluQ29udGVudCRjaGtOb3RpZnlPblJlcGx5BRVjdG"
        oRequest.Body = oRequest.Body + "wwMCRTaXRlTWVudTEkY3RsMDAPD2QFCE5ldyBQYWdlZOIkQK%2"
        oRequest.Body = oRequest.Body + "FnwxHjjzIKdLmjs7LcUgF%2B&ctl00%24SearchInput1%24tx"
        oRequest.Body = oRequest.Body + "tSearch=enter+search+terms&ctl00%24mainContent%24F"
        oRequest.Body = oRequest.Body + "CKTextBox0=this+is+a+topic+for+you&ctl00%24mainCon"
        oRequest.Body = oRequest.Body + "tent%24txtSubject=topic+1&ctl00%24mainContent%24bt"
        oRequest.Body = oRequest.Body + "nUpdate=Post+%5BAlt%2B1%5D&__PREVIOUSPAGE=XU73QLdx"
        oRequest.Body = oRequest.Body + "zBy9TXxzA_CEMSyqdwZyGc7uNtE1-08TsJQ1&__EVENTVALIDA"
        oRequest.Body = oRequest.Body + "TION=%2FwEWBwKn%2BfSWDQLs%2FMSMCAK4gcXfBgKshPfCCAL"
        oRequest.Body = oRequest.Body + "nwNvPCgKgvuvCAgL0%2FaO7B5%2FCjP8WNcpyMLCtK0tpAcRQG"
        oRequest.Body = oRequest.Body + "Ahw"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumPostEdit.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest59()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (1257)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumThreadView.aspx"+"?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?forumid=1&pageid=1&threadid=0&postid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumThreadView.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest60()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (668)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumThreadView.aspx?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    If fEnableDelays = True then Test.Sleep (198)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/avatars/"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumThreadView.aspx?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/avatars/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest62()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (5981)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumPostEdit.aspx"+"?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumThreadView.aspx?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumPostEdit.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest63()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (194)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest64()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (114)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest65()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (76)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest66()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (65)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/avatars/"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/avatars/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest67()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (10107)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumPostEdit.aspx"+"?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oRequest.Verb = "POST"
        oRequest.HTTPVersion = "HTTP/1.0"
        oRequest.EncodeBody = False
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "Content-Type", "application/x-www-form-urlencoded"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        oHeaders.Add "Content-Length", "(automatic)" 
        oRequest.Body = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEP"
        oRequest.Body = oRequest.Body + "DwUJODk1NTk5NDE1DxYCHgtVcmxSZWZlcnJlcgVVaHR0cDovL2"
        oRequest.Body = oRequest.Body + "xvYWR0ZXN0L21vam8vRm9ydW1UaHJlYWRWaWV3LmFzcHg%2FdG"
        oRequest.Body = oRequest.Body + "hyZWFkPTImZm9ydW1pZD0xJnBhZ2VpbmRleD0zJnBhZ2VudW1i"
        oRequest.Body = oRequest.Body + "ZXI9MRYCZg9kFgICAw9kFggCBQ8PFgIeB1Zpc2libGVoZGQCCw"
        oRequest.Body = oRequest.Body + "9kFgICAg8PFgIeBFRleHQFBlNlYXJjaGRkAg4PDxYEHghDc3ND"
        oRequest.Body = oRequest.Body + "bGFzcwURY2VudGVyLWxlZnRtYXJnaW4eBF8hU0ICAmQWAgIFD2"
        oRequest.Body = oRequest.Body + "QWDAIDDxYIHgVWYWx1ZWUeCEJhc2VQYXRoBRAvbW9qby9GQ0tl"
        oRequest.Body = oRequest.Body + "ZGl0b3IvHgZDb25maWcyyQYAAQAAAP%2F%2F%2F%2F8BAAAAAA"
        oRequest.Body = oRequest.Body + "AAAAwCAAAAUEZyZWRDSy5GQ0tlZGl0b3JWMiwgVmVyc2lvbj0y"
        oRequest.Body = oRequest.Body + "LjIuMjM5NS4zMTE2NywgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaW"
        oRequest.Body = oRequest.Body + "NLZXlUb2tlbj1udWxsBQEAAAAqRnJlZENLLkZDS2VkaXRvclYy"
        oRequest.Body = oRequest.Body + "LkZDS2VkaXRvckNvbmZpZ3VyYXRpb25zAQAAAAtDb25maWdUYW"
        oRequest.Body = oRequest.Body + "JsZQMcU3lzdGVtLkNvbGxlY3Rpb25zLkhhc2h0YWJsZQIAAAAJ"
        oRequest.Body = oRequest.Body + "AwAAAAQDAAAAHFN5c3RlbS5Db2xsZWN0aW9ucy5IYXNodGFibG"
        oRequest.Body = oRequest.Body + "UHAAAACkxvYWRGYWN0b3IHVmVyc2lvbghDb21wYXJlchBIYXNo"
        oRequest.Body = oRequest.Body + "Q29kZVByb3ZpZGVyCEhhc2hTaXplBEtleXMGVmFsdWVzAAADAw"
        oRequest.Body = oRequest.Body + "AFBQsIHFN5c3RlbS5Db2xsZWN0aW9ucy5JQ29tcGFyZXIkU3lz"
        oRequest.Body = oRequest.Body + "dGVtLkNvbGxlY3Rpb25zLklIYXNoQ29kZVByb3ZpZGVyCOxROD"
        oRequest.Body = oRequest.Body + "8EAAAACgoLAAAACQQAAAAJBQAAABAEAAAABAAAAAYGAAAAD0lt"
        oRequest.Body = oRequest.Body + "YWdlQnJvd3NlclVSTAYHAAAADUVkaXRvckFyZWFDU1MGCAAAAA"
        oRequest.Body = oRequest.Body + "5MaW5rQnJvd3NlclVSTAYJAAAACFNraW5QYXRoEAUAAAAEAAAA"
        oRequest.Body = oRequest.Body + "BgoAAABzL21vam8vRkNLZWRpdG9yL2VkaXRvci9maWxlbWFuYW"
        oRequest.Body = oRequest.Body + "dlci9icm93c2VyL2RlZmF1bHQvYnJvd3Nlci5odG1sP1R5cGU9"
        oRequest.Body = oRequest.Body + "SW1hZ2UmQ29ubmVjdG9yPWNvbm5lY3RvcnMvYXNweC9jb25uZW"
        oRequest.Body = oRequest.Body + "N0b3IuYXNweAYLAAAAO2h0dHA6Ly9sb2FkdGVzdC9tb2pvL0Rh"
        oRequest.Body = oRequest.Body + "dGEvU2l0ZXMvMS9za2lucy9jZW50ZXJlZDEvc3R5bGUuY3NzBg"
        oRequest.Body = oRequest.Body + "wAAABoL21vam8vRkNLZWRpdG9yL2VkaXRvci9maWxlbWFuYWdl"
        oRequest.Body = oRequest.Body + "ci9icm93c2VyL2RlZmF1bHQvYnJvd3Nlci5odG1sP0Nvbm5lY3"
        oRequest.Body = oRequest.Body + "Rvcj1jb25uZWN0b3JzL2FzcHgvY29ubmVjdG9yLmFzcHgGDQAA"
        oRequest.Body = oRequest.Body + "ACQvbW9qby9GQ0tlZGl0b3IvZWRpdG9yL3NraW5zL25vcm1hbC"
        oRequest.Body = oRequest.Body + "8LHgpUb29sYmFyU2V0BQRGdWxsZAINDw8WAh4MRXJyb3JNZXNz"
        oRequest.Body = oRequest.Body + "YWdlBRhZb3UgTXVzdCBFbnRlciBhIFN1YmplY3RkZAIPDw8WBB"
        oRequest.Body = oRequest.Body + "8CBQxQb3N0IFtBbHQrMV0eCUFjY2Vzc0tleQUBMWRkAhEPDxYE"
        oRequest.Body = oRequest.Body + "HwIFDkNhbmNlbCBbQWx0KzJdHwoFATJkZAITDw8WBh8BaB8CBR"
        oRequest.Body = oRequest.Body + "hEZWxldGUgdGhpcyBpdGVtIFtBbHQrM10fCgUBM2RkAhUPZBYC"
        oRequest.Body = oRequest.Body + "Zg8WBh4EaHJlZgUpL21vam8vSGVscC5hc3B4P2hlbHBrZXk9Zm"
        oRequest.Body = oRequest.Body + "9ydW1wb3N0ZWRpdGhlbHAeA3JlbAUZaWJveCZoZWlnaHQ9MzUw"
        oRequest.Body = oRequest.Body + "JndpZHRoPTQwMB4FdGl0bGVkFgJmDxYGHgNzcmMFKy9tb2pvL0"
        oRequest.Body = oRequest.Body + "RhdGEvU2l0ZUltYWdlcy9GZWF0dXJlSWNvbnMvaGVscC5naWYe"
        oRequest.Body = oRequest.Body + "A2FsdGQeBmJvcmRlcgUBMGQCDw8PFgIfAWhkZBgCBR5fX0Nvbn"
        oRequest.Body = oRequest.Body + "Ryb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAQUiY3RsMDAkbWFp"
        oRequest.Body = oRequest.Body + "bkNvbnRlbnQkY2hrTm90aWZ5T25SZXBseQUVY3RsMDAkU2l0ZU"
        oRequest.Body = oRequest.Body + "1lbnUxJGN0bDAwDw9kBQRIb21lZI2Nr4Va8uLh7tWsM7eJz1wh"
        oRequest.Body = oRequest.Body + "La4S&ctl00%24SearchInput1%24txtSearch=enter+search"
        oRequest.Body = oRequest.Body + "+terms&ctl00%24mainContent%24FCKTextBox0=this+is+a"
        oRequest.Body = oRequest.Body + "+reply+for+ya&ctl00%24mainContent%24txtSubject=Re%"
        oRequest.Body = oRequest.Body + "3A+topic+1&ctl00%24mainContent%24btnUpdate=Post+%5"
        oRequest.Body = oRequest.Body + "BAlt%2B1%5D&__PREVIOUSPAGE=XU73QLdxzBy9TXxzA_CEMSy"
        oRequest.Body = oRequest.Body + "qdwZyGc7uNtE1-08TsJQ1&__EVENTVALIDATION=%2FwEWBwKh"
        oRequest.Body = oRequest.Body + "taeoDwLs%2FMSMCAK4gcXfBgKshPfCCALnwNvPCgKgvuvCAgL0"
        oRequest.Body = oRequest.Body + "%2FaO7Bzas6t%2BGSWmgMRoZMw6TUHVKBS3T"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumPostEdit.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest68()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (262)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumThreadView.aspx"+"?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumPostEdit.aspx?thread=2&pageid=0&postid=0&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        oHeaders.Add "Pragma", "no-cache"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumThreadView.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest69()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (87)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumThreadView.aspx?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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

Sub SendRequest70()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (48)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/Data/Sites/1/avatars/"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "*/*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumThreadView.aspx?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/Data/Sites/1/avatars/"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest71()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (19114)
    Set oConnection = Test.CreateConnection("loadtest", 80, false)
    If (oConnection is Nothing) Then
        Test.Trace "Error: Unable to create connection to loadtest"
    Else
        Set oRequest = Test.CreateRequest
        oRequest.Path = "/mojo/ForumView.aspx"+"?ItemID=1&mid=0&pageindex=3"
        oRequest.Verb = "GET"
        oRequest.HTTPVersion = "HTTP/1.0"
        set oHeaders = oRequest.Headers
        oHeaders.RemoveAll
        oHeaders.Add "Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumThreadView.aspx?thread=2&forumid=1&pageindex=3&pagenumber=1"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
        oHeaders.Add "Cookie", "(automatic)"
        Set oResponse = oConnection.Send(oRequest)
        If (oResponse is Nothing) Then
            Test.Trace "Error: Failed to receive response for URL to " + "/mojo/ForumView.aspx"
        Else
            strStatusCode = oResponse.ResultCode
        End If
        oConnection.Close
    End If
End Sub

Sub SendRequest72()
    Dim oConnection, oRequest, oResponse, oHeaders, strStatusCode
    If fEnableDelays = True then Test.Sleep (114)
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
        oHeaders.Add "Referer", "http://loadtest/mojo/ForumView.aspx?ItemID=1&mid=0&pageindex=3"
        oHeaders.Add "Accept-Language", "en-us,it;q=0.91,de;q=0.82,nl;q=0.73,pt-br;q=0.64,ru;q=0.55,cs;q=0.45,ar-iq;q=0.36,es-mx;q=0.27,tr;q=0.18,es;q=0.09"
        oHeaders.Add "User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; InfoPath.1)"
        'oHeaders.Add "Host", "loadtest"
        oHeaders.Add "Host", "(automatic)"
        'oHeaders.Add "Cookie", ".ASPXANONYMOUS=dUmUfBSxeiO_EtIWMXzRWET0nRDty2ioKDqski_fx-S08gJv594gqK32By859JPPxUbCzgDNo-xM33FZhrVD5apxJN75oMxAU6VNKO67NSs1; ASP.NET_SessionId=u2jbpmabs2m5mr55l0t0slnv; .ASPXAUTH=E42A2585CF2685EE139CE3FC630EDB04A1D54CAF922249B25908029620C2959E37C950CC17E3C39C8FA9BE9ADE8131FFF53B851AF8971C5167725176B014DFCB15D1E54ACEE0EA69785C479878ACB54E; DisplayName=Admin; portalroles=A293F743C8DB5502DD1212CF3DF4F59FCC3E1B1B6FD16DB01BB87F5AE6C236CD291EFE09A9CB2A26EAFA6C57CB223A3A1C15D561A4AA597C6F6F888B17BB89C70AC46A314D0BF0C1B1D0D8641A4BBC8FDE6512A069220712A64A0F86EBA3448BAD541BB94929D15545AB417546110042E115AA0FE014AC7EC8C17E7F7088E531"
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
    call SendRequest68()
    call SendRequest69()
    call SendRequest70()
    call SendRequest71()
    call SendRequest72()
End Sub
Main
