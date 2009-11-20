; (function($) {
	$.fn.iDropMenu = function(settings) {
		var defaults = {
			menuDiv: '#personal_hover',
			dropdownArrow: '#personal_menu_down',
			myUid: 0,
			webRoot:''
		};

		$.extend(defaults, settings);

		var $menuDiv, menuDiv_html;
		$menuDiv = $(defaults.menuDiv);
		menuDiv_html = $menuDiv.html(); //保存原始的html        
		$menuDiv.bind('mouseleave', function() { $(this).hide(); });

		var wasQueryed = false, friends;
		var setFriendHtml = function(uid, $obj) {
			$obj.next().remove();
			if (uid == defaults.myUid) {
				$obj.html('<strong>你自己</strong>');
			} else if (!friends) {
				$obj.after('<span class="be_stranger"><a href="javascript:;" onclick="friends(' + uid + ');">是否要加为好友?</a></span>');
			} else if (friends['f_' + uid] && friends['f_' + uid] == "1") {
				$obj.after('<span class="be_stranger">是好友 <a href="'+defaults.webRoot+'friends.aspx?d=' + uid + '" onclick="return confirm(\'确定要删除吗？\');">删除?</a></span>');
			} else if (friends['f_' + uid] && friends['f_' + uid] == "2") {
				$obj.after('<span class="be_stranger">已关注 <a href="'+defaults.webRoot+'attention.aspx?u' + uid + '&a=delete" onclick="return confirm(\'确定要取消吗？\');">取消关注</a></span>');
			} else {
				$obj.after('<span class="be_stranger"><a href="javascript:;" onclick="friends(' + uid + ');">是否要加为好友?</a></span>');
			}
		};

		return this.each(function() {
			var $this;
			$this = $(this);
			$this.mouseover(function() {
				var params = {
					uid: $this.attr('title'),
					avatar: $this.find('img').attr('src'),
					nick_name: $this.attr('title')
				};

				var html = menuDiv_html.parseTpl(params);
				var rects = $this[0].getBoundingClientRect();
				var bodyRects = $('body')[0].getBoundingClientRect();
				var left = rects.left - bodyRects.left - 6;
				var top = rects.top - bodyRects.top - 6;
				$menuDiv.css({ 'top': top, 'left': left });
				$menuDiv.html(html).show();
				$menuDiv.find(defaults.dropdownArrow).click(function() {
					var $this = $(this);
					var $next = $this.next();
					$this.toggleClass('active')
					$next.toggle();
					if ($next.css('display') != 'block') return;
					$relation = $next.find('.be_contacts');
					if (params.uid == defaults.myUid) { setFriendHtml(params.uid, $relation); return; }
					if (wasQueryed) { setFriendHtml(params.uid, $relation); return; }
					$relation.after($loading2);
					$.getJSON('/member/get-friends', function(data) {
						try {
							$relation.next().remove();
							if (data.status == 'failed') { alert(data.msg); return false; }
							wasQueryed = true;
							friends = data.friends;
							setFriendHtml(params.uid, $relation);
						} catch (e) { alert('Warning:something wrong! Please retry!'); return; }
					});
				});
			});
		});
	};
})(jQuery);
