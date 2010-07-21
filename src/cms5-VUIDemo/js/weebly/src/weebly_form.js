Weebly.Form = {
    drawRadioOptions : function(options){
        if(typeof(options) === 'string'){
            options = options.split('||');
        }
        var radio = '';
        var name = 'radio-'+Math.floor(Math.random()*999);
        options.each(function(option){
            radio += '<span class="form-radio-container"><input type="radio" name="'+name+'" /><label>'+option+'</label></span>';
        });
        return radio;
    },

    drawSelectOptions : function(options){
        if(typeof(options) === 'string'){
            options = options.split('||');
        }
        var select = '<select>';
        options.each(function(option){
            select += '<option>'+option+'</option>';
        });
        select += '</select>';
        return select;
    },

    drawCheckboxes : function(options){
        if(typeof(options) === 'string'){
            options = options.split('||');
        }
        var radio = '';
        options.each(function(option){
            radio += '<span class="form-radio-container"><input type="checkbox" /><label>'+option+'</label></span>';
        });
        return radio;
    },

    isOverInputWarningLimit : function(){
        if(Weebly.Restrictions.hasAccess('unlimited_form_inputs')){
            return false;
        }

        var newElements = $$('#secondlist .outside_top');
        if(newElements.size() == 1){
            var newEl = newElements[0];
            var form = newEl.up('.formlist')
            if(form){
                var totalInputs  = form.select('.weebly-form-field').size();
                return totalInputs >= (Weebly.Restrictions.accessValue('free_form_inputs') - 1);
            }
        }
        return false;
    },

    isOverInputLimit : function(){
        if(Weebly.Restrictions.hasAccess('unlimited_form_inputs')){
            return false;
        }

        var newElements = $$('#secondlist .outside_top');
        if(newElements.size() == 1){
            var newEl = newElements[0];
            var form = newEl.up('.formlist')
            if(form){
                var totalInputs  = form.select('.weebly-form-field').size();
                return totalInputs >= (Weebly.Restrictions.accessValue('free_form_inputs'));
            }
        }
        return false;
    },
    
    isNewFormElement : function(){
        var newElements = $$('#secondlist .outside_top');
        if (newElements.size() == 1) {
            if(newElements[0].down('input')){
                var def = newElements[0].down('input').value;
                var id = def.replace(/[^\d]/g, '');
                return Weebly.Form.elements.member(id);
            }
        }
        return false;
    },

    showFieldInstructions : function( msg, pointTo ){
        var image = false;
        var el = new Element( 'div', { 'class':'instructions-container', 'id':pointTo.id+'-instructions' } ).update( msg );
        currentVisibleError = el.identify();
        el.observe( 'click', function(e){ el.hide().remove() } );
        $('scroll_container').insert( {'bottom':el} );
        var dimensions = el.getDimensions();

        var target = $(pointTo);
        var offset = target.cumulativeOffset();
        var targetDimensions = target.getDimensions();
        var top = (offset.top + targetDimensions.height/2 - dimensions.height/2) - 133 + 'px';
        var left = ( offset.left + targetDimensions.width + 20 ) + 'px';

        el.setStyle( {top: top, left: left} );
        //set arrow position
        var imagetop  = Math.floor( dimensions.height / 2 ) - 10;
        var imageleft = '-13';
        el.insert( {'bottom':'<img src="http://www.weebly.com/images/error_arrow_left.gif" style="position: absolute; left:'+imageleft+'px; top: '+imagetop+'px;" />'} );
    },

    removeFieldInstructions : function(event){
        var el = Event.element(event);
        if(!el.up('.weebly-form-field')){
            document.stopObserving('mousemove', Weebly.Form.removeFieldInstructions);
            $$('.instructions-container').invoke('remove');
        }
    },

    fieldInstructionsHandler : function(){
        $$('.weebly-form-instructions').each(function(el){Weebly.Form.setupFieldInstructions(el);});
    },

    setupFieldInstructions : function(el){
        var pointTo = $(el.id.replace('instructions', 'input'));
        //select inputs
        if(!pointTo){
            pointTo = el.up('.weebly-form-field').down('.form-select');
        }
        //radio/checkbox inputs
        if(!pointTo){
            pointTo = el.up('.weebly-form-field').down('.weebly-form-label');
        }
        var container = pointTo.up('.weebly-form-field');
        if(pointTo.up('.weebly-form-input-container') && pointTo.up('.weebly-form-input-container').hasClassName('weebly-form-left')){
            pointTo = pointTo.up('.weebly-form-input-container').next('.weebly-form-right');
        }
        container.stopObserving('mouseover');
        container.observe('mouseover', function(event){
            if(this.hasClassName('weebly-form-field')){
                if(!$(pointTo.id+'-instructions') && !el.innerHTML.empty()){
                    Weebly.Form.showFieldInstructions(el.innerHTML, pointTo);
                }
                document.observe('mousemove', Weebly.Form.removeFieldInstructions);
            }
        });
    },
    
    commonFieldOptions : {
        'Gender' : ['Male', 'Female'],
        'Age' : ['Less than 13','13-18','19-25','26-35','36-50','Over 50','Prefer not to say'],
        'Income' : ['Less than $10,000','$10,001-$25,000','$25,001-$40,000','$70,001-$100,000','> $100,000', 'Prefer not to say'],
        'Marketing Reference' : ['Internet Search','Advertisment','Friend','Other'],
        'Answer' : ['Yes','No','Maybe'],
        'Education' : ['Some High School','Completed High School','Some College','Associate\'s Degree','Bachelor\'s Degree','Master\'s Degree','PhD'],
        'Employment' : ['Unemployed','Part-time','Full-time','Self-employed'],
        'Days of the Week' : ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'],
        'Months of the Year' : ['January','February','March','April','May','June','July','August','September','October','November','December'],
        'U.S. States' : ['Alabama','Alaska','Arizona','Arkansas','California','Colorado','Connecticut','Delaware','Florida','Georgia','Hawaii','Idaho','Illinois','Indiana','Iowa','Kansas','Kentucky','Louisiana','Maine','Maryland','Massachusetts','Michigan','Minnesota','Mississippi','Missouri','Montana','Nebraska','Nevada','New Hampshire','New Jersey','New Mexico','New York','North Carolina','North Dakota','Ohio','Oklahoma','Oregon','Pennsylvania','Rhode Island','South Carolina','South Dakota','Tennessee','Texas','Utah','Vermont','Virginia','Washington','West Virginia','Wisconsin','Wyoming'],
        'How Often' : ['Everyday','Once per week','2 to 3 times per week','Once per month','Less than once per month'],
        'How Long' : ['Less than 1 month','1-6 months','1-3 years','Over 3 years','Never'],
        'Satisfaction' : ['Very Satisfied','Satisfied','Neutral','Unsatisfied','Very Unsatisfied'],
        'Importance' : ['Very Important','Important','Neutral','Somewhat Important','Not at all Important'],
        'Agreement' : ['Strongly Agree','Agree','Neutral','Disagree','Strongly Disagree'],
        'Comparison' : ['Much Better','Somewhat Better','About the Same','Somewhat Worse','Much Worse']
    }  

}
