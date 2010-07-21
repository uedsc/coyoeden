
/***************Rules**********************/
var myrules = {
   
    '#error' : function(el){
	el.onclick = function(){
	    new Effect.Fade('error');
	}
    }, 
    '#tip11' : function(el){
        el.onclick = function(){
            Effect.Fade('tip11');
        }
    }
    
};

/***************Register Rules*************/
Behaviour.register(myrules);


/********Resgister additional events*******/
Behaviour.apply();

