    Position.Window = {
        //extended prototypes position to return
        //the scrolled window deltas
        getDeltas: function() {
            var deltaX =  window.pageXOffset
                || document.documentElement.scrollLeft
                || document.body.scrollLeft
                || 0;
            var deltaY =  window.pageYOffset
                || document.documentElement.scrollTop
                || document.body.scrollTop
                || 0;
            return [deltaX, deltaY];
        },
        //extended prototypes position to
        //return working window's size, 
        //copied this code from the 
        size: function() {
            var winWidth, winHeight, d=document;
            if (typeof window.innerWidth!='undefined') {
                winWidth = window.innerWidth;
                winHeight = window.innerHeight;
            } else {
                if (d.documentElement && typeof d.documentElement.clientWidth!='undefined' && d.documentElement.clientWidth!=0) {
                    winWidth = d.documentElement.clientWidth
                    winHeight = d.documentElement.clientHeight
                } else {
                    if (d.body && typeof d.body.clientWidth!='undefined') {
                        winWidth = d.body.clientWidth
                        winHeight = d.body.clientHeight
                    }
                }
            }
            return [winWidth, winHeight];
        }
    }
    //my own custom effect that basically
    //calls the Effect.Move Scriptaculous
    //effect with the correct window offsets
    function keepFixed(element, offsetx, offsety) {
        var _scroll = Position.Window.getDeltas();
        var _window = Position.Window.size();
        var elementDimensions = Element.getDimensions(element);
        var eWidth = elementDimensions.width;
        var eHeight = elementDimensions.height;
        var moveX = _scroll[0] + offsetx;
        var moveY = _scroll[1] + offsety;
        return new Effect.Move(element, { x: moveX, y: moveY, mode: 'absolute' });
    }
    //the function that calls the KeepFixed effect.
    //It accepts the id, and offsets from bottom left
    function feedback() {
        keepFixed('designs_show', 20, 20);
    }
