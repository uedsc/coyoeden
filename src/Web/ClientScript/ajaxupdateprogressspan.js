// based on Sys.UI._UpdateProgress
// modified by Joe Audette to work with soan instead of div
// used in /Controls/UpdateProgressSpan.cs which is a modified version of UpdateProgress from the Mono project
// last modified 2009-10-30

Type.registerNamespace("C");

C._UpdateProgress = function C$_UpdateProgress(element) {
    C._UpdateProgress.initializeBase(this,[element]);
    this._displayAfter = 500;
    this._dynamicLayout = true;
    this._associatedUpdatePanelId = null;
    this._beginRequestHandlerDelegate = null;
    this._startDelegate = null;
    this._endRequestHandlerDelegate = null;
    this._pageRequestManager = null;
    this._timerCookie = null;
}
    function C$_UpdateProgress$get_displayAfter() {
        /// <value type="Number" locid="P:J#C._UpdateProgress.displayAfter"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._displayAfter;
    }
    function C$_UpdateProgress$set_displayAfter(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        this._displayAfter = value;
    }
    function C$_UpdateProgress$get_dynamicLayout() {
        /// <value type="Boolean" locid="P:J#C._UpdateProgress.dynamicLayout"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._dynamicLayout;
    }
    function C$_UpdateProgress$set_dynamicLayout(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._dynamicLayout = value;
    }
    function C$_UpdateProgress$get_associatedUpdatePanelId() {
        /// <value type="String" mayBeNull="true" locid="P:J#C._UpdateProgress.associatedUpdatePanelId"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._associatedUpdatePanelId;
    }
    function C$_UpdateProgress$set_associatedUpdatePanelId(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._associatedUpdatePanelId = value;
    }
    function C$_UpdateProgress$_clearTimeout() {
        if (this._timerCookie) {
            window.clearTimeout(this._timerCookie);
            this._timerCookie = null;
        }
    }
    function C$_UpdateProgress$_handleBeginRequest(sender, arg) {
        var curElem = arg.get_postBackElement();
        var showProgress = !this._associatedUpdatePanelId; 
        while (!showProgress && curElem) {
            if (curElem.id && this._associatedUpdatePanelId === curElem.id) {
                showProgress = true; 
            }
            curElem = curElem.parentNode; 
        } 
        if (showProgress) {
            this._timerCookie = window.setTimeout(this._startDelegate, this._displayAfter);
        }
    }
    function C$_UpdateProgress$_startRequest() {
        if (this._pageRequestManager.get_isInAsyncPostBack()) {
            if (this._dynamicLayout) this.get_element().style.display = 'inline';
            else this.get_element().style.visibility = 'visible';
        }
        this._timerCookie = null;
    }
    function C$_UpdateProgress$_handleEndRequest(sender, arg) {
        if (this._dynamicLayout) this.get_element().style.display = 'none';
        else this.get_element().style.visibility = 'hidden';
        this._clearTimeout();
    }
    function C$_UpdateProgress$dispose() {
        if (this._beginRequestHandlerDelegate !== null) {
            this._pageRequestManager.remove_beginRequest(this._beginRequestHandlerDelegate);
            this._pageRequestManager.remove_endRequest(this._endRequestHandlerDelegate);
            this._beginRequestHandlerDelegate = null;
            this._endRequestHandlerDelegate = null;
        }
        this._clearTimeout();
        C._UpdateProgress.callBaseMethod(this,"dispose");
    }
    function C$_UpdateProgress$initialize() {
        C._UpdateProgress.callBaseMethod(this, 'initialize');
    	this._beginRequestHandlerDelegate = Function.createDelegate(this, this._handleBeginRequest);
    	this._endRequestHandlerDelegate = Function.createDelegate(this, this._handleEndRequest);
    	this._startDelegate = Function.createDelegate(this, this._startRequest);
    	if (Sys.WebForms && Sys.WebForms.PageRequestManager) {
           this._pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
    	}
    	if (this._pageRequestManager !== null ) {
    	    this._pageRequestManager.add_beginRequest(this._beginRequestHandlerDelegate);
    	    this._pageRequestManager.add_endRequest(this._endRequestHandlerDelegate);
    	}
    }
C._UpdateProgress.prototype = {
    get_displayAfter: C$_UpdateProgress$get_displayAfter,
    set_displayAfter: C$_UpdateProgress$set_displayAfter,
    get_dynamicLayout: C$_UpdateProgress$get_dynamicLayout,
    set_dynamicLayout: C$_UpdateProgress$set_dynamicLayout,
    get_associatedUpdatePanelId: C$_UpdateProgress$get_associatedUpdatePanelId,
    set_associatedUpdatePanelId: C$_UpdateProgress$set_associatedUpdatePanelId,
    _clearTimeout: C$_UpdateProgress$_clearTimeout,
    _handleBeginRequest: C$_UpdateProgress$_handleBeginRequest,
    _startRequest: C$_UpdateProgress$_startRequest,
    _handleEndRequest: C$_UpdateProgress$_handleEndRequest,
    dispose: C$_UpdateProgress$dispose,
    initialize: C$_UpdateProgress$initialize
}
C._UpdateProgress.registerClass('C._UpdateProgress', Sys.UI.Control);
