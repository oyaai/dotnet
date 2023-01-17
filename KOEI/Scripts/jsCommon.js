// Komsan L. (Validate client)
function IsDecimals(evt, txt) {
    var charCode = (evt.which) ? evt.which : window.event.keyCode;
    var count = document.getElementById(txt).value.split('.').length - 1;
    var atPointCount = document.getElementById(txt).value.split('.')[1];
    var keyChar = String.fromCharCode(charCode);
    var re;
    if (count == 0) {
        re = /[0123456789.,]/;
    }
    else {
        if (atPointCount.length < 2) {
            re = /[0123456789,]/;
        } else {       
            re = /[0123456789,]/;
        }
    }

    if (charCode <= 13) {
        return true;
    }
    else {
        return re.test(keyChar);
    }
    
}

function setNumberPoint(strNum) {
    var String = strNum.toString();
    var strRemComma = String.replace(/,/g,"");
    if (strRemComma == "" || strRemComma == ".") {
        return "";
    } else {
        var CToString = strRemComma;
        var numSplit = CToString.split(".");
        if (numSplit.length == 2) {
            if (numSplit[0] == ""){
                numSplit[0] = "0";
            }
            switch (numSplit[1].length) {
                case 0:
                    return SetNumberWithComma(numSplit[0]) + "." + "00";
                    break;
                case 1:
                    return SetNumberWithComma(numSplit[0]) + "." + numSplit[1] + "0";
                    break;
                default:
                    return SetNumberWithComma(numSplit[0]) + "." + numSplit[1];
                    break;
            }    
        } else {
            return SetNumberWithComma(CToString) + ".00";
        }
    }
}

function setCurrencyPoint(strNum) {
    var String = strNum.toString();
    var strRemComma = String.replace(/,/g,"");
    if (strRemComma == "" || strRemComma == ".") {
        return "";
    } else {
        var CToString = (Math.round(parseFloat(strRemComma) * 100) / 100).toString();
        var numSplit = CToString.split(".");
        if (numSplit.length == 2) {
            if (numSplit[0] == ""){
                numSplit[0] = "0";
            }
            switch (numSplit[1].length) {
                case 0:
                    return SetNumberWithComma(numSplit[0] + "." + "00");
                    break;
                case 1:
                    return SetNumberWithComma(numSplit[0] + "." + numSplit[1] + "0");
                    break;
                case 2:
                    return SetNumberWithComma(numSplit[0] + "." + numSplit[1]);
                    break;
                default:
                    var getValue = numSplit[0] + "." + numSplit[1];
                    var valueTmp = (Math.round(parseFloat(getValue) * 100) / 100).toString() + "0000";
                    valueToReturn = valueTmp.split(".")[0] + "." + valueTmp.split(".")[1].substring(0, 2);
                    return SetNumberWithComma(valueToReturn);
            }    
        } else {
            return SetNumberWithComma(CToString + ".00");
        }
    }
}

function SetNumberWithComma(num) {
    var numRet = num.toString();
    numRet = numRet.replace(/,/g, "");
    var re = /(-?\d+)(\d{3})/;
    while (re.test(numRet)) {
        numRet = numRet.replace(re, "$1,$2");
    }
    return numRet;
}