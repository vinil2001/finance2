function SetFileName(input) {
    input.parentElement.nextSibling.nextSibling.children[0].value = input.value.slice((input.value.lastIndexOf("\\") + 1), input.value.length);
    //document.getElementsByName('fileName')[document.getElementsByName('fileName').length - 1].value = input.value.slice(OnChangeInput(input), input.value.length);
    //input.parentElement.nextSibling.nextSibling.nextSibling.nextSibling.style.visibility = 'visible';
    var elements = input.parentElement.children[1].innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="17" viewBox="0 0 20 17">
                        <path d="M1 1 C 17 17, 17 17, 17 17" stroke-width="2" stroke-linecap="round" stroke="red" fill="transparent" line-height="10px" />
                        <path d="M17 1 C 1 17, 1 17, 1 17" stroke-width="2" stroke-linecap="round" stroke="red" fill="transparent" />
                    </svg>
                    <span onclick="RemoveFile(event)">Удалить файл</span>`;
    OnChangeInput(input);
}

function RemoveFile(event) {
    var removeButtonElement = event.target.parentElement.parentElement;
    var fileNameElement = removeButtonElement.nextSibling.nextSibling;
    //var emptyCellElement = removeButtonElement.nextSibling.nextSibling.nextSibling.nextSibling;
    if (fileNameElement)
        fileNameElement.remove();
    if (removeButtonElement)
        removeButtonElement.remove();
    //if (emptyCellElement)
    //    emptyCellElement.remove();
    alert("Файл удален");
}

function OnChangeInput(input) {

    var UniqUploadDocumentUrl = 'id' + (new Date()).getTime();

    var newTag = $(".item16").append(`
                <div class="uploadFile" name="uploadFile">
                <input onchange="SetFileName(this)" type="file" name="UploadDocumentUrl" id=`+ UniqUploadDocumentUrl + ` class="btn btn-default inputfile" />
                <label id="uploadDocumentLabelId" name="uploadDocumentLabel" for=`+ UniqUploadDocumentUrl + `>
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="17" viewBox="0 0 20 17">
                    <path d="M10 0l-5.2 4.9h3.3v5.1h3.8v-5.1h3.3l-5.2-4.9zm9.3 11.5l-3.2-2.1h-2l3.4 2.6h-3.5c-.1 0-.2.1-.2.1l-.8 2.3h-6l-.8-2.2c-.1-.1-.1-.2-.2-.2h-3.6l3.4-2.6h-2l-3.2 2.1c-.4.3-.7 1-.6 1.5l.6 3.1c.1.5.7.9 1.2.9h16.3c.6 0 1.1-.4 1.3-.9l.6-3.1c.1-.5-.2-1.2-.7-1.5z"></path>
                </svg>
                    <span>Добавить файл</span>
                </label>
                </div>
                <div class="fileName">
                    <input id="fileName" name="fileName" disabled type="text" />
                </div>
            `);

    return input.value.lastIndexOf("\\") + 1; // return uploaded file name 
}

function SumOnClick(e) {
    if (!$(e.target.closest('div')).next().next().find("input[name='PartialPaymentChecked']").is(':checked')) {
        e.target.readOnly = false;
    }
}


var partialPaymentChecked = false;
var partialPaymentApproved = false;
var paymentPaymentDone = false;


function SetPaymentChecked(e) {
    partialPaymentApproved = $(e.target.closest('div')).next().find("input[name='PartialPaymentApproved']").is(':checked');
    if (userIsInRoleSign1 === 'false') {
        e.preventDefault();
        alert("Действие запрещено, у вас нет прав для его осуществления");
    }

    if (partialPaymentApproved === true) {
        e.preventDefault();
        alert("Действие запрещено, так как счет подтвержден в оплату.");
    }

    if (!$(e.target).is(":checked")) {
        var target = $(e.target.closest('[removeImg]'));
        $(e.target.closest('div')).next().next().next().find("div[id='removeImg']").append('<img onclick="DeletePay(event); /*PartialPaymentsSumm(event);*/ SumChange(this.value); " style="cursor: pointer; width:44px; height:34px;" src="/pictures/remove-icon-png-7133-100x100.png" alt="Удалить" title="Удалить">');
        var summaInput = $(e.target.closest('div')).prev().prev().children()[0].readOnly === false;
        console.log(summaInput);
    }

}

function SetPaymentApproved(e) {
    partialPaymentChecked = $(e.target.closest('div')).prev().find("input[name='PartialPaymentChecked']").is(':checked');
    paymentPaymentDone = $(e.target.closest('div')).next().find("input[name='PaymentPaymentDone']").is(':checked');
    //paymentPaymentDone = $("input[name='PaymentPaymentDone']").is(':checked');

    if (userIsInRoleSign1 === 'true') {
        if (paymentPaymentDone === true) {
            e.preventDefault();
            alert("Действие запрещено, так как платеж в статусе ОПЛАЧЕН.");
        }
        else if (partialPaymentChecked === false) {
            e.preventDefault();
            alert("Действие запрещено. Счет не прошел проверку на предыдущем этапе.");
        }
    }
    else {
        e.preventDefault();
        alert("Действие запрещено, у вас нет прав для его осуществления");
    }
}

function SetPaymentDone(e) {
    partialPaymentApproved = $(e.target.closest('div')).prev().find("input[name='PartialPaymentApproved']").is(':checked');
    //partialPaymentApproved = $("input[name='PartialPaymentApproved']").is(':checked');
    //paymentPaymentDone = $("input[name='PaymentPaymentDone']").is(':checked');
    if (userIsInRoleSign1OrAccountant === 'true') {
        if (partialPaymentApproved === false) {
            e.preventDefault();
            alert("Действие запрещено, так как платеж не подтвержден в оплату.");
        }
    }
    else {
        e.preventDefault();
        alert("Действие запрещено, у вас нет прав для его осуществления");
    }
}

$('#DocumentUrl').on("change", function () {
    var $this = $(this), $clone = $this.clone();
    $clone.attr('name', 'UploadDocumentUrl');
    $clone.attr('id', '');
    $('#files-container').append('<div></div>');
    var divContainer = $('#files-container').children().last();

    //$this.after($clone).appendTo(divContainer); // does not work
    $this.after($clone);
    $clone.appendTo(divContainer);


    //var hiddenInput = "<input type='file' style='display: none' name='UploadDocumentUrl' value='" + $(this).val() +"'>";

    if ($(this).val() !== "") {
        var fileName = "<p style='float: left'>" + $(this).val() + "</p>";
        var removeLink = '<img onclick="$(this).parent().remove();" style="opacity: 0.5; cursor: pointer; width: 30px; height: 30px;" src="/pictures/symbol-delete-icon.png" alt="Удалить файл" title="Удалить файл">';
        divContainer.append(fileName + removeLink);
    }
    $('#DocumentUrl').val('');
});

function AddRmoveButton() {
    if ($("input[name='Summa']").length === 2) {
        var teg = '<input class="ppItem" disabled id="summPercentages" name="summPercentages"/>';
        $('#removeImg').append($(teg));
    }

}

function AddPay() {
    //   alert(parseFloat($("input[name='Summa']:last").val().replace(",", ".")).toFixed(2));
    if (parseFloat($("input[name='Summa']:last").val().replace(",", ".")).toFixed(2) === parseFloat(0).toFixed(2)) {
        alert('Невозможно добавить новый платёж, пока предыдущий равен 0');
        return;
    }
    if (RestSumm() < 0) {
        alert('Невозможно добавить новый платёж. Сумма всех платежей превышает сумму счета!');
        return;
    }

    $("#MainLoader").show();
    $.ajax({
        url: '/PaymentStatements/_PartialPartOfPayment/',
        type: "GET", //Типа HTML-запроса
        success: function (data) {
            $("#MainLoader").hide();
            //$('#PaymentsContainer').append(data);
            $("div[name='removeImg']:last").after(data);            
            AddRmoveButton();
            //$("div[id^='removeImg']").last().append(`<img onclick="DeletePay(event);  SumChange(); " style="cursor: pointer; width:44px; height:34px;" src="/pictures/remove-icon-png-7133-100x100.png" alt="Удалить" title="Удалить">`);
            //$("input[name='Summa']:last").on("change keydown paste input", SumChange());
            //$("input[name='Summa']:last").on("change", PartialPaymentsSumm);
            $("input[name='Summa']:last").val(RestSumm().toFixed(2).replace(".", ","));
            SumChange();
        },
        error: function (data) {
            alert('Нет связи с сервером. Data:' + data);
            $("#MainLoader").hide();
        }
    });
}

function RestSumm() {
    var partialPaymentSumm = 0.0;
    $("input[name='Summa']").each(function () {
        partialPaymentSumm += parseFloat($(this).val().replace(',', '.'));
        var val = parseFloat($(this).val().replace(',', '.'));
    });
    //var invoiceS = parseFloat($("#InvoiceSumma").val().replace(',', '.'), 2);
    var restSumm = parseFloat($("#InvoiceSumma").val().replace(',', '.')) - partialPaymentSumm;

    return restSumm;
}

function DeletePay(event) {
    var elements = $(event.target).parent().prevAll();
    for (let i = 0; i < 6; i++) { 
        elements[i].remove();
    }
    $(event.target).parent().remove();

    if ($("input[name='Summa']").length <= 1) {
        $(".item11").children().remove();
    }
    //$(event.target).parent().parent().remove();
}

function sumaRoundUp(e) {
    var sum = $(e.target).val().replace('.',',');
    if (sum.indexOf(',') > 0) {
        var sumStringSlice = sum.slice(0, sum.indexOf(',') + 3);
        $(e.target).val(sumStringSlice);
    }

}

function SumChange() {  
    //$("#InvoiceSumma").val($("#InvoiceSumma").val().replace(".", ","));
    var sumaValues = [];
    $("input[name='Summa']").each(function () {
        $(this).val($(this).val().replace(".", ","));
        sumaValues.push($(this).val());
    });
    var invSum = $("#InvoiceSumma").val($("#InvoiceSumma").val().replace(".", ","));
    var percentsSumm = parseFloat(0.00);
    var sp = $("#summPercentages").val();
    $("input[name='percentage']").each(function (index) {
        var invoiceSumma = parseFloat($("#InvoiceSumma").val().replace(",", "."), 4);
        var percent = (parseFloat(sumaValues[index].replace(",", ".")) * 100 / invoiceSumma).toFixed(4);
        if (percent.length > 0 && percent !== "NaN") {
            $(this).val(parseFloat(percent).toFixed(2).toString() + "%");
            
        } else {
            $(this).val("0%");
        }      
        percentsSumm += parseFloat(percent);
    });
    if (percentsSumm) {
        $("#summPercentages").val("∑ = " + percentsSumm.toFixed(2).toString() + "%");
    }
    else {
        $("#summPercentages").val("∑ = 0%");
    }
   
    var paymentsSum = 0;
    $.each(sumaValues, function (key, value) {
        paymentsSum += parseFloat(value.replace(",", "."), 4);
    });
    if (paymentsSum > 0) {
        $("#paymentsSum").val("∑ = " + paymentsSum.toFixed(2));
    }
    else {
        $("#paymentsSum").val("∑ = 0");
    }
    
    //alert("SumChange");
}


function PartialPaymentsSumm(e) {   //Считает общую сумму частичных оплат...
    //alert("fired");
    var partialPaymentSumm = 0;
    $("input[name='Summa']").each(function () {
        partialPaymentSumm += parseFloat($(this).val().replace(",", "."));
    });
    var InvoiceSumma = parseFloat($("#InvoiceSumma").val().replace(",", ".")).toFixed(2);
    if (partialPaymentSumm > InvoiceSumma) {
        alert("Превышение суммы счета на " + (partialPaymentSumm - InvoiceSumma).toFixed(2));
        e.preventDefault();
        //$("#SaveButton").prop("disabled", true);
    }
    else {
        $("#SaveButton").prop("disabled", false);
    }
}
//$("#SaveButton").click(function PartialPaymentsSumm(e) {//Считает общую сумму частичных оплат...
//    var partialPaymentSumm = 0;
//    $("input[name='Summa']").each(function () {
//        partialPaymentSumm += parseFloat($(this).val().replace(",", "."));
//    });
//    var InvoiceSumma = parseFloat($("#InvoiceSumma").val().replace(",", ".")).toFixed(2);
//    if (partialPaymentSumm > InvoiceSumma) {
//        alert("Превышение суммы счета на " + (partialPaymentSumm - InvoiceSumma).toFixed(2));
//        e.preventDefault();
//        //$("#SaveButton").prop("disabled", true);
//    }
//    //else {
//    //    $("#SaveButton").prop("disabled", false);
//    //}
//});

//function getFileName() {
//    var isThis = this;
//}

$("#CreatePaymentStatementForm").on("submit", function (event) {
    var element = $("#Counterparty_name")[0];

    if ($("#Counterparty_name").val() === '') {
        element.setCustomValidity("Компания не выбрана");
        element.reportValidity();
        event.preventDefault();
    }
    else
        if ($("#KltId").val() === 0) {
            element.setCustomValidity("Данной компании нет в базе данных. Выберите название из выпадающего списка в поле 'Компания'. Либо создайте новую нажав на кнопку 'Добавить новую'");
            element.reportValidity();
            event.preventDefault();
        }
})

$(document).ready(function () {
    var element = $("#Counterparty_name")[0];
    var CompanyNameAutocomplete = $("#Counterparty_name").autocomplete({
        source: function (request, response) {
            $('#KltId').val('');
            element.setCustomValidity("");
            $.ajax({
                url: '/Counterparties/SearchAutocomplete/',
                type: "POST", //Типа HTML-запроса
                dataType: "json",
                data: {
                    request: request.term,
                },
                success: function (data) {

                    response($.map(data, function (item) {
                        return { label: item.Name, id: item.Id/*, TypeDb: item.TypeDb*/ };
                    }));
                },
                error: function () {
                    //alert("Данной компании нет в базе данных. Выберите название из выпадающего списка в поле 'Компания'. Либо создайте новую нажав на кнопку 'Добавить новую'")
                    alert('Нет связи с базой данных');
                }
            });
        },
        select: function (event, ui) {
            $('#KltId').val(ui.item.id);
            element.setCustomValidity("");

            //$('#CounterpartyTypeDb').val(ui.item.TypeDb);
        },
        //change: function () {

        //}
    });
});

$(document).ready(function () {
    $("#InvoiceDate").datepicker({
        dateFormat: 'dd.mm.yy'
    });

    $("div[name='removeImg']").each(function (i) {
        if (i == 0) {
            this.hidden = true;
        }
    });
});