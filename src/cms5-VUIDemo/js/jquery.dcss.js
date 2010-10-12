;(function($) {
    // Private functions.
    var p = {};
	p.ID="dcss_camp";
    p.M = function() {
		this.$d=null;
		this._css=null;
		this._rules={}; 
	};
	p.M.prototype={
		init:function(){
			//init layout
			this.$d=$("#"+p.ID);
			if(this.$d.length==0){
				this.$d=$("<style/>",{
					id:p.ID,
					type:"text/css"
				}).appendTo("head");
			};
			this._css=$.trim(this.$d.html());
			//load css
			if(this._css.length>0){
				var rows=this._css.split("}"),cssJson,a0,a1,a2;
				for(var i=0;i<rows.length;i++){
					if(rows[i].length==0) break;
					cssJson={};
					a0=rows[i].split("{");
					a1=$.trim(a0[1]).split(";");
					for(var j=0;j<a1.length;j++){
						if(a1[j].length==0) break;
						a2=$.trim(a1[j]).split(":");
						cssJson[$.trim(a2[0])]=$.trim(a2[1]);
					};//for2
					this._rules[$.trim(a0[0])]=cssJson;					
				};//for1
			};//if
		},
		update:function(ruleName,val){
			var old=this._rules[ruleName];
			if(old){
				val=$.extend(old,val);
			};
			
			//如果css属性值为Null,则移除该属性
			for(var c in val){
				if(!c) break;
				if(val[c]===null)
					delete val[c];
			};
			
			this._rules[ruleName]=val;
			return this;
		},
		remove:function(ruleName){
			var old=this._rules[ruleName];
			if(old){
				delete this._rules[ruleName];
			};	
			return this;
		},
		refresh:function(){
			if($.isEmptyObject(this._rules)) return;
			this._css="";
			var cssJson;
			for(var c in this._rules){
				if(!c) break;
				this._css+=c+"{";
				cssJson=this._rules[c];
				for(var c1 in cssJson){
					if(!c1) break;
					this._css+=c1+":"+cssJson[c1]+";"
				};//for2
				this._css+="}\r\n";
			};//for1
			this.$d.html(this._css);
		}
	};
	p._instance=new p.M();
    // Public functions.
    $.DCSS=p._instance;
})(jQuery);  