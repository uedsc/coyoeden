/**
 * 获取用户选中的页面对象
 * @author levin
 */
sohu.diySelection=function(){
	var p={},pub={};
	/**
	 * 封装iframe编辑器的用户选择对象selection
	 * @param {Object} opts
	 */
	p.selection=function(opts){
		this._sel=opts.sel;
		this._range=opts.range;
		if($.browser.msie){
			this.text=this._range.text;
			this.isEmpty=(this.text=="");
			this.$parent=$(this._range.parentElement());
			this.$selected=this._range.item?this._range.item(0):this.$parent;
		}else{
			this.isEmpty=(this._sel.rangeCount==0);
			this.text=this.isEmpty?"":this._range.toString();
			this.$selected=$(this._sel.anchorNode);
			this.$parent=this.$selected.parent();
			
		}
	};
	p.selection.prototype.select=function(){
		if(this.isEmpty) return this;
		if($.browser.msie)
			this._range.select();
			
	};
	p.selection.prototype.selectAndRelease=function(){
		if(this.isEmpty) return this;
		if($.browser.msie){
			this._range.select();
			this._range='';
		}else{
			//do nothing...
		}
	};
	p.selection.prototype.getA=function(){
		var a={};
		if (this.$selected.is("a")) {
			a.$obj=this.$selected;
		}else if(this.$selected.find("a").length>0){
			a.$obj=this.$selected.find("a").eq(0);
		}else if (this.$parent.is("a")) {
			a.$obj=this.$parent;
		}else{
			a.$obj=this.$parent.parents("a").eq(0);
		}
		a.isNull=(a.$obj.length==0);
		if(!a.isNull){
			a.title=a.$obj.attr("title");
			a.href=a.$obj.attr("href");
			a.target=a.$obj.attr("target");
			a.target=a.target!="_self"?"_blank":"_self";
		};
		return a;
	};
	/**
	 * Current selection is in a anchor tag
	 */
	p.selection.prototype.inA=function(){
		var retVal=(this.$parent.is("a")||this.$parent.parents("a").length>0);
		return retVal;
	};
	/**
	 * make a snap for the current selection of specified document
	 * @param {document} doc document for snapping
	 * @param {Object} obj 	a dom object to be selected
	 */
	pub.snap=function(doc,obj){
		var o={};
		if($.browser.msie){
			if(obj){
				doc.selection.empty();
				var cr=doc.body.createControlRange();
				cr.addElement(obj);
				cr.select();
			};
			o.sel=doc.selection;
			o.range=o.sel.createRange();
			
		}else{
			var win=doc.parentWindow||doc.defaultView;
			if(obj){
				var cr1=doc.createRange();
				cr1.selectNode(obj);
				var sel=win.getSelection();
				sel.removeAllRanges();
				sel.addRange( cr1 );
			}
			o.sel=win.getSelection();
			o.range=o.sel.getRangeAt(0);
		}
		pub.CurSelection=new p.selection(o);
		sohu.diyConsole.DocSelection=pub.CurSelection;
	};
	return pub;
}();
