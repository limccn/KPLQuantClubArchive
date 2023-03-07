// IE Polyfill
if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (search, pos) {
        return this.substr(!pos || pos < 0 ? 0 : +pos, search.length) === search
    }
}

function ellipsisFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    return value.length > 30 ? value.substr(0, 30) + "..." : value;
}

function wrapFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    ellipsis = value.length > 57 ? value.substr(0, 57) + "..." : value;
    var num = 20
    var result = ""
    for (var i = 0; i < Math.ceil(ellipsis.length / num); i++) {
        result = result + ellipsis.substr(i * num, num) + "<br />";
    }
    return result;
}

function dateFormatter(value) {
    if (typeof (value) == "undefined" || value == "" || value == "00000000") {
        return ""
    } else {
        return value.replace(/(\d{4})(\d{2})(\d{2})/mg, '$1/$2/$3');
    }
}

function dateFormattermmdd(value) {
    if (typeof (value) == "undefined" || value == "" || value == "00000000") {
        return ""
    } else {
        return value.replace(/(\d{4})(\d{2})(\d{2})/mg, '$2$3');
    }
}

function timeFormatter(value) {
    if (typeof (value) == "undefined" || value == "" || value == "000000") {
        return ""
    } else {
        return value.replace(/(\d{2})(\d{2})(\d{2})/mg, '$1:$2');
    }

}

function decimalWanFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    var color = value > 0 ? "#ff0000" : (value == 0 ? "#000000" : "#0000ff");
    return '<div style="color: ' + color + '">' +
        Math.floor(value / 10000).toString() + "万"
    '</div>'
}

function decimalYiFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    var color = value > 0 ? "#ff0000" : (value == 0 ? "#000000" : "#0000ff");
    return '<div style="color: ' + color + '">' +
        (value / 100000000).toFixed(2) + "亿"
    '</div>'
}

function decimalPlainWanFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    return Math.floor(value / 10000).toString() + "万"
}

function decimalPlainYiFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    return (value / 100000000).toFixed(2) + "亿"
}


function decimalPlainYiFormatter0(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    return (value / 100000000).toFixed(0) + "亿"
}

function decimalAutoFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    if (Math.abs(value) >= 100000000) {
        return decimalYiFormatter(value);
    } else if (Math.abs(value) >= 10000) {
        return decimalWanFormatter(value);
    } else {
        return value.toString();
    }
}

function decimalPlainAutoFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    if (Math.abs(value) >= 100000000) {
        return decimalPlainYiFormatter(value);
    } else if (Math.abs(value) >= 10000) {
        return decimalPlainWanFormatter(value);
    } else {
        return value.toString();
    }
}

function percentPlainFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    var percent = Math.round(value * 100) / 100.0;
    return percent.toFixed(2) + "%"
}

function percentFormatter(value) {
    if (typeof (value) == "undefined" || value == "") {
        return ""
    }
    var color = value > 0 ? "#ff0000" : (value == 0 ? "#000000" : "#0000ff");
    var percent = Math.round(value * 100) / 100.0;
    return '<div style="color: ' + color + '">' +
        percent.toFixed(2) + "%"
    '</div>'
}