var popUpObj;
function showModalPopUp(Link)
{
    popUpObj=window.open(Link, "ModalPopUp",
    "toolbar=no," +
    "scrollbars=no," +
    "location=no," +
    "statusbar=no," +
    "menubar=no," +
    "resizable=0," +
    "width=500," +
    "height=500," +
    "left = 500," +
    "top=30"
    );
    popUpObj.focus();
    LoadModalDiv();
}
//<input id="Button1" type="button" value="button" onclick="showModalPopUp()" />
function LoadModalDiv()
{
    var bcgDiv = document.getElementById("divParrentBackground");
    bcgDiv.style.display="block";
}

function HideModalDiv() 
{
    //alert("HideModalDiv");
    var bcgDiv = document.getElementById("divParrentBackground");
    bcgDiv.style.display = "none";
}

function OnClose() 
{
    if(window.opener != null && !window.opener.closed) 
    {
        //alert("OnClose");
        window.opener.HideModalDiv();
    }
}
function OnCloseFromParent()
{
    if (popUpObj.window.opener != null && !popUpObj.window.closed)
    {
        //alert("OnCloseFromParent");
        popUpObj.window.HideModalDiv();
    }
}


