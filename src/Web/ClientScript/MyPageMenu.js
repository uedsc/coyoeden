var hoverClass = "userpagemenu-Hover";
var topmostClass = "userpagemenu";

function Hover__userpagemenu(element)
{
//    AddClassUpward__CssFriendlyAdapters(element.firstChild /* gets the inner SPAN or A */, topmostClass, hoverClass);
    AddClass__CssFriendlyAdapters(element, hoverClass);
}

function Unhover__userpagemenu(element)
{
//    RemoveClassUpward__CssFriendlyAdapters(element.firstChild /* gets the inner SPAN or A */, topmostClass, hoverClass);
    RemoveClass__CssFriendlyAdapters(element, hoverClass);
}

function SetHover__userpagemenu()
{
    var menus = document.getElementsByTagName("ul");
    for (var i=0; i<menus.length; i++)
    {
        if(menus[i].className == topmostClass)
        {
            var items = menus[i].getElementsByTagName("li");
            for (var k=0; k<items.length; k++)
            {
                items[k].onmouseover = function() { Hover__userpagemenu(this); }
                items[k].onmouseout = function() { Unhover__userpagemenu(this); }
            }
        }
    }
}


window.onload = SetHover__userpagemenu;

