pageSetUp();

var onSuccess = function () {
    $.validator.unobtrusive.parse(document);
};

$(function () {
    $(document).ajaxError(function (e, xhr, settings) {
        if (xhr.status == 401) {
            location = '/Account/Login';
        }
    });

    $(document).ajaxStart(function () {
        showLoading();
    });

    $(document).ajaxStop(function () {
        hideLoading();
    });
});

function showLoading() {
    $("#generalLoading").show();
}

function hideLoading() {
    $("#generalLoading").hide();
}

$(document).ready(function () {
    $('input, select').not('input[type=button], input[type=hidden], .lastField').on("keypress", function (e) {
        var listFields = $('input, select, textarea, button, a').not('input[type=hidden]');
        var n = listFields.length;

        if (e.which == 13) {
            e.preventDefault();
            var nextIndex = listFields.index(this) + 1;
            if (nextIndex < n)
                listFields[nextIndex].focus();
            else {
                listFields[nextIndex - 1].blur();
                listFields.click();
            }
        }
    });
});

$(document).ready(function () {
    jQuery.validator.addMethod(
        'date',
        function (value, element, params) {
            if (this.optional(element)) {
                return true;
            };
            var result = false;
            try {
                $.datepicker.parseDate('dd/mm/yy', value);
                result = true;
            } catch (err) {
                result = false;
            }
            return result;
        }
    );
});

(function (factory) {
    if (typeof define === "function" && define.amd) {
        // AMD. Register as an anonymous module.
        define(["../jquery.ui.datepicker"], factory);
    } else {
        // Browser globals
        factory(jQuery.datepicker);
    }
}
(function (datepicker) {
    datepicker.regional['pt-BR'] = {
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior'
    };
    datepicker.setDefaults(datepicker.regional['pt-BR']);

    return datepicker.regional['pt-BR'];
}));

function showErrorMessage(tag, msg) {
    if (msg.length > 0)
        tag.removeClass("field-validation-valid").addClass("field-validation-error");
    else
        tag.removeClass("field-validation-error").addClass("field-validation-valid");

    tag.html(msg);
}

jQuery.extend(jQuery.validator.methods, {
    number: function (value, element) {
        return this.optional(element) ||
            /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    }
});

$(document).ready(function () {
    initializeDecimalBehaviour();
});

function initializeDecimalBehaviour() {
    $("input.decimal-2-casas").on("focusout", function () {
        $(this).val(roundDecimal($(this).val(), 2));
    });

    $("input.decimal-4-casas").on("focusout", function () {
        $(this).val(roundDecimal($(this).val(), 4));
    });

    $("input.decimal-5-casas").on("focusout", function () {
        $(this).val(roundDecimal($(this).val(), 5));
    });

    $("input.decimal-7-casas").on("focusout", function () {
        $(this).val(roundDecimal($(this).val(), 7));
    });
}

function roundDecimal(value, precision) {
    var originalValue = 0 + value;
    var roundedValue = parseFloat(stringToFloat(originalValue.toString())).toFixed(precision);
    return roundedValue.replace(".", ",");
}

Date.prototype.toFormatDDMMYYYY = function () {
    if (!isNaN(this)) {
        var yyyy = this.getUTCFullYear().toString();
        var mm = (this.getUTCMonth() + 1).toString(); // getMonth() is zero-based
        var dd = this.getUTCDate().toString();
        return (dd[1] ? dd : "0" + dd[0]) + '/' + (mm[1] ? mm : "0" + mm[0]) + '/' + yyyy;
    }
    return "";
};

function highlight(data, search) {
    return data.replace(new RegExp("(" + stringToRegExp(search) + ")", 'gi'), "<strong>$1</strong>");
}

function stringToRegExp(str) {
    return (str + '').replace(/([\\\.\+\*\?\[\^\]\$\(\)\{\}\=\!\<\>\|\:])/g, "\\$1");
}

function getErrorMessageContainer(fieldName) {
    return $(document).find('span[data-valmsg-for="' + fieldName + '"]');
}

jQuery.validator.addMethod(
    "decimalGreaterThanZero",
    function (value, element) {
        return stringToFloat(value) > 0;
    },
    "Informe um valor maior que zero."
);

function floatToString(value) {
    if (value !== null) value = value.toString().replace(".", ",");
    return value;
}

function stringToFloat(value) {
    return parseFloat(value.toString().replace(".", "").replace(",", "."));
}

function smartAlert(title, message, type) {
    var color = "";
    var icon = "";
    switch (type.toLowerCase()) {
        case 'error':
            color = "#c26565";
            icon = "fa fa-times";
            break;
        case 'success':
            color = "#cde0c4";
            icon = "fa fa-check";
            break;
        case 'warning':
            color = "#efe1b3";
            icon = "fa fa-warning";
            break;
        case 'info':
        default:
            color = "#d6dde7";
            icon = "fa fa-info-circle";
            break;
    }

    $.smallBox({
        title: title,
        content: message.replace("\n", "<br />"),
        color: color,
        timeout: 4000,
        icon: icon
    });
    $('.SmallBox:has(i.fa-times)').addClass('text-color-error');
    $('.SmallBox:has(i.fa-check)').addClass('text-color-success');
    $('.SmallBox:has(i.fa-warning)').addClass('text-color-warning');
    $('.SmallBox:has(i.fa-info-circle)').addClass('text-color-info');
}

function compararDatas(data1, data2) {

    var data_1 = data1.substring(0, 10);
    var data_2 = data2.substring(0, 10);

    var diaDt1 = data_1.split("/")[0];
    var mesDt1 = data_1.split("/")[1];
    var anoDt1 = data_1.split("/")[2];

    var dt1 = new Date(anoDt1, mesDt1, diaDt1);

    var diaDt2 = data_2.split("/")[0];
    var mesDt2 = data_2.split("/")[1];
    var anoDt2 = data_2.split("/")[2];

    var dt2 = new Date(anoDt2, mesDt2, diaDt2);

    dt1.setHours(0);
    dt1.setMinutes(0);
    dt1.setSeconds(0);
    dt2.setHours(0);
    dt2.setMinutes(0);
    dt2.setSeconds(0);

    retorno = 0
    if (dt1 > dt2) {
        retorno = 1
    }
    else {
        if (dt1 < dt2) {
            retorno = -1
        }
    }

    return retorno;
}

function diasDecorridos(dt1, dt2){
    // variáveis auxiliares
    var minuto = 60000; 
    var dia = minuto * 60 * 24;
    var horarioVerao = 0;
    
    dt1 = dt1.substring(0, 10);
    dt2 = dt2.substring(0, 10);

    var diaDt1 = dt1.split("/")[0];
    var mesDt1 = dt1.split("/")[1];
    var anoDt1 = dt1.split("/")[2];

    var dt1 = new Date(anoDt1, mesDt1, diaDt1);

    var diaDt2 = dt2.split("/")[0];
    var mesDt2 = dt2.split("/")[1];
    var anoDt2 = dt2.split("/")[2];

    var dt2 = new Date(anoDt2, mesDt2, diaDt2);

    // ajusta o horario de cada objeto Date
    dt1.setHours(0);
    dt1.setMinutes(0);
    dt1.setSeconds(0);
    dt2.setHours(0);
    dt2.setMinutes(0);
    dt2.setSeconds(0);
    
    // determina o fuso horário de cada objeto Date
    var fh1 = dt1.getTimezoneOffset();
    var fh2 = dt2.getTimezoneOffset(); 
    
    // retira a diferença do horário de verão
    if(dt2 > dt1){
        horarioVerao = (fh2 - fh1) * minuto;
    } 
    else{
        horarioVerao = (fh1 - fh2) * minuto;    
    }
    
    var dif = Math.abs(dt2.getTime() - dt1.getTime()) - horarioVerao;
    return Math.ceil(dif / dia);
}

$('.numeric').on('input', function (event) {
    this.value = this.value.replace(/[^0-9]/g, '');
});

function dateDiffInDays(start, end) {
    var days = (end - start) / 1000 / 60 / 60 / 24;
    return days;
}

function isValidDate(format, value)
{
    var isValid = true;

    try {
        jQuery.datepicker.parseDate(format, value);
    }
    catch (error) {
        isValid = false;
    }

    return isValid;
}
function goToTop() {
    $('html, body').animate({ scrollTop: 0 }, 'slow');
}