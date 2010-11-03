/**
 * @author levinhuang
 * 焦点图点击统计代码
 */
;(function($){
	$(function(){
		var trackUrl="http://track.svip.sohu.com/t.php",
		data={id:170,index:0,url:'',ts:''},
		$items=$("[data-track]");
		if($items.length>0){
			var $if=$('<iframe id="fi_tracker" height="0" width="0" border="0" style="position:absolute;top:-100px;left:-100px;"/>').appendTo("body");
			$items.each(function(i,o){
				o=$(o);
				(function($t,j){
					$t.click(function(){
						data.id=$t.attr("data-track");
						data.index=$t.index("[collection='Y']");
						data.url=($t.data("curData")||{l:''}).l;
						data.ts=new Date().getTime();
						$if.attr("src",trackUrl+"?"+$.param(data));	
					});					
				})(o,i);

			});			
		};

	});	
})(jQuery);
