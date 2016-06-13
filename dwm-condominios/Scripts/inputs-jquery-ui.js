
function ShowMessageAlert(mensagem, tipo) {
    $('#MsgAlert').html(mensagem);
    if (tipo == "danger") {
        $('#alerta-heading').hide();
        $('#alerta').attr('class', 'alert alert-danger fade in');
    }
    else if (tipo == "warning") {
        $('#alerta-heading').show();
        $('#alerta').attr('class', 'alert alert-dismissable alert-warning');
    }
    else {
        $('#alerta-heading').hide();
        $('#alerta').attr('class', 'alert alert-dismissable alert-success');
    }

    $("#alerta").show();
}

function ShowMessageAjaxAlert(mensagem, tipo) {
    $('#MsgAjaxAlert').html(mensagem);
    if (tipo == "danger") {
        $('#div-alert').attr('class', 'alert alert-danger fade in');
    }
    else if (tipo == "warning") {
        $('#div-alert').attr('class', 'alert alert-dismissable alert-warning');
    }
    else {
        $('#div-alert').attr('class', 'alert alert-dismissable alert-success');
    }

    $("#AjaxAlert").show();
}

function CarregandoIn() {
    $('#carregando').css("visibility", "visible");
    $('#carregando').css("width", "100%");
    $('#carregando').css("height", "500%");
    $('#carregando').css("position", "absolute");
    $('#carregando').css("background-color", "black");
    $('#carregando').css("filter", "alpha(opacity=60)");
    $('#carregando').css("opacity", "0.6");
    $('#carregando').css("left", "0%");
    $('#carregando').css("top", "0%");
}

function FileUpload(_fileUpload, _fileHidden, _progress, _file_name, _myModal, _arq, _trash) {

    // _fileupload = fileUpload

    if (_arq == '' || _arq == '')
        _arq = 'arq1';

    if (_trash == '' || _trash == '')
        _trash = 'trash1';

    $('#' + _progress).hide();

    $('#' + _fileUpload).fileupload({
        dataType: 'json',
        url: "UploadFiles", // MasterController
        autoUpload: true,
        done: function (e, data) {

            if (data.result.mensagem != "Sucesso") {
                $('#' + _progress + ' #' + _progress + '-bar').css('width', '0%');
                ShowMessageAlert(data.result.mensagem, "danger");
                $('#' + _arq).hide();
                $('#' + _trash).hide();
                return;
            }

            $('#' + _fileUpload).hide(); // esconde o componente para forçar a exclusão do arquivo para poder enviar outro arquivo

            if (data.result.type == 'image/jpeg' || data.result.type == 'image/png' || data.result.type == 'image/bmp') {
                $('#' + _myModal + '-body').html('<img src="../Temp/' + data.result.name + '" class="img-responsive" alt="Responsive image" style="height: 600px">');
                $('#' + _file_name).html('<small data-toggle="modal" data-target="#' + _myModal + '" style="cursor: pointer" id="' + _arq + '"><img src="../Temp/' + data.result.name + '" class="img-responsive" alt="Responsive image" style="height: 75px">' + data.result.nome_original + '</small>&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-trash" aria-hidden="true" style="cursor: pointer" id="' + _trash + '" onclick="DelFile(\'' + data.result.name + '\', \'' + _fileUpload + '\', \'' + _arq + '\', \'' + _trash + '\')"></span>');
            }
            else if (data.result.type == 'application/pdf') {
                $('#' + _myModal + '-body').html('<iframe style="height: 600px; width: 100%" src="../Temp/' + data.result.name + '"></iframe>');
                $('#' + _file_name).html('<small data-toggle="modal" data-target="#' + _myModal + '" style="cursor: pointer" id="' + _arq + '"><img src="http://www.equidadeparaainfancia.org/img/pdf.jpg" class="img-responsive" alt="Responsive image">' + data.result.nome_original + '</small>&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-trash" aria-hidden="true" style="cursor: pointer" id="' + _trash + '" onclick="DelFile(\'' + data.result.name + '\', \'' + _fileUpload + '\', \'' + _arq + '\', \'' + _trash + '\')"></span>');
            }
            $('.modal-title').html(data.result.nome_original);
            //_fileHidden = fileBoleto
            $('#' + _fileHidden).val(data.result.name);
            $('#' + _progress + ' #' + _progress + '-bar').css('width', '0%');
            $('#' + _progress).hide();

            // word: http://www.portugalsenior.org/wp-content/uploads/2014/07/MW-Logo1-50x50.jpg

            //$('.file_type').html(data.result.type);
            //$('.file_size').html(data.result.size);
        }
    }).on('fileuploadprogressall', function (e, data) {
        $('#' + _progress).show();
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#' + _progress + ' #' + _progress + '-bar').css('width', progress + '%');
    });
}

function DelFile(_file, _fileupload, _arq, _trash) {
    var link = encodeURI('DeleteFile?fileName=' + _file);
    CarregandoIn();

    link = encodeURI(link + '&noCahce=' + new Date());

    $.ajax({
        type: "get",
        url: link,
        contentType: "application/json; charset=utf-8",
        global: false,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.mensagem != "Sucesso") {
                ShowMessageAlert(data[0].result.mensagem, "danger");
            }
            $('#' + _arq).hide();
            $('#' + _trash).hide();
            $('#' + _fileupload).show();
            $('#carregando').css("visibility", "hidden");
            $('#carregando').css("height", "0px");
            $('#carregando').css("margin-top", "0%");
            $('#carregando').css("margin-left", "0%");
        }
    });
}


function Mascara(formato, keypress, objeto) {
    campo = eval(objeto);

    if (formato == 'DATA') {
        separador = '/';
        conjunto1 = 2;
        conjunto2 = 5;
        if (campo.value.length == conjunto1) {
            campo.value = campo.value + separador;
        }
        if (campo.value.length == conjunto2) {
            campo.value = campo.value + separador;
        }
    }
}

function GetSelectListOnCascade(thisId, nextId, tag) {

    var link = "GetNames";
    var _Id = "";
    var _tag = "";

    // Recupera o VALUE do dropdownlist selecionado
    _Id = $("#" + thisId + " option:selected").val();

    if (_Id == "") {
        $('#' + nextId).html('<select class="form-control input-sm" id="' + nextId + '" name="' + nextId + '"><option value="">Selecione...</option></select>');
        return
    }
    
    link = encodeURI(link + '?term=' + _Id);

    // Recupera o campo adicional TAG
    if (tag != '') {
        _tag = $("#" + tag).val();
        link = encodeURI(link + '&tag=' + _tag);
    }

    CarregandoIn();

    link = encodeURI(link + '&noCahce=' + new Date());

    $.ajax({
        type: "POST",
        url: link,
        contentType: "application/json; charset=utf-8",
        global: false,
        async: false,
        dataType: "json",
        success: function (jsonObj) {
            var listItems = "";
            for (i in jsonObj) {
                listItems += "<option value='" + jsonObj[i].Value + "'>" + jsonObj[i].Text + "</option>";
            }
            $("#" + nextId).html(listItems);
            $('#carregando').css("visibility", "hidden");
            $('#carregando').css("height", "0px");
            $('#carregando').css("margin-top", "0%");
            $('#carregando').css("margin-left", "0%");
        }
    });
}

function BuscaCep(_cep, _logradouro, _bairro, _cidade, _uf) {
    CarregandoIn();
    var cep = $("#" + _cep).val();
    cep = cep.replace(/\D/g, "");
    var link = encodeURI("http://cep.correiocontrol.com.br/" + cep + ".json");

    $.getJSON(link, function (data) {
        $("#" + _logradouro).val(data.logradouro);
        $("#" + _bairro).val(data.bairro);

        $('[name=' + _cidade + '] option').filter(function () {
            return ($(this).text() == data.localidade); 
        }).prop('selected', true);

        $("#" + _uf).val(data.uf);
        $('#carregando').css("visibility", "hidden");
        $('#carregando').css("height", "0px");
        $('#carregando').css("margin-top", "0%");
        $('#carregando').css("margin-left", "0%");

    });
}

function BuscaCep2(_cep, _logradouro, _bairro, _cidade, _uf) {
    CarregandoIn();
    var cep = $("#" + _cep).val();
    cep = cep.replace(/\D/g, "");
    var link = encodeURI("http://cep.correiocontrol.com.br/" + cep + ".json");

    $.getJSON(link, function (data) {
        $("#" + _logradouro).val(data.logradouro);
        $("#" + _bairro).val(data.bairro);
        $("#" + _cidade).val(data.localidade);
        $("#" + _uf).val(data.uf);
    });
    $('#carregando').css("visibility", "hidden");
    $('#carregando').css("height", "0px");
    $('#carregando').css("margin-top", "0%");
    $('#carregando').css("margin-left", "0%");

}

function ReadAlert(id) {
    var link = "../Home/ReadAlert?alertaId=" + id;
    $('#read-alert').load(encodeURI(link));
}

function AddMiniCrud(id, text, controller, action, DivId) {
    if ($("#" + id).val() != "" && $("#" + id).val() != "0") {
        var link = action + '?value=' + $("#" + id).val() + '&text=' + $("#" + text).val() + "&DivId=" + DivId
        $('#' + id).val("");
        $('#' + text).val("");
        $('#' + text + "1").val("");
        $('#' + DivId).load(encodeURI(link));
    }
}

function DelMiniCrud(value, text, controller, action, DivId) {
    var link = action + '?value=' + value + "&text=" + text + "&DivId=" + DivId;
    $('#' + DivId).load(encodeURI(link));
}

function foward(pageindex, pagesize, LastPage, action, DivId) {
    var max;
    max = LastPage;
    if (eval(pageindex) > max)
        pageindex = max;
    else if (pageindex < 0)
        pageindex = 0;
    
    Refresh(pageindex, pagesize, action, DivId);
}

function Refresh(index, pagesize, action, DivId) {
    var link = action;
    link = encodeURI(link + '?index=' + index + '&pageSize=' + pagesize);

    var $inputs = $('#form0 :input');

    // not sure if you wanted this, but I thought I'd add it.
    // get an associative array of just the values.
    $inputs.each(function () {
        if (this.id != '' && this.id != null)
            link += '&' + this.id + '=' + $(this).val()
    });

    $('#carregando').css("visibility", "visible");
    $('#carregando').css("width", "100%");
    $('#carregando').css("height", "100%");
    $('#carregando').css("position", "absolute");
    $('#carregando').css("background-color", "black");
    $('#carregando').css("filter", "alpha(opacity=60)");
    $('#carregando').css("opacity", "0.6");
    $('#carregando').css("left", "0%");
    $('#carregando').css("top", "0%");

    link = encodeURI(link + '&noCahce=' + new Date());

    $('#' + DivId).load(link);
    $( document ).ajaxSuccess(function (event, xhr, settings) {
        $('#carregando').css("visibility", "hidden");
        $('#carregando').css("height", "0px");
        $('#carregando').css("margin-top", "0%");
        $('#carregando').css("margin-left", "0%");
    }).error(function () {
        $('#carregando').css("visibility", "hidden");
        $('#carregando').css("height", "0px");
        $('#carregando').css("margin-top", "0%");
        $('#carregando').css("margin-left", "0%");
    })
}

function selecionaPdf(obj, controller, action, javascriptFunction) {
    if (javascriptFunction != null)
    {
        var result = eval(javascriptFunction);
        if (result == false)
            return;
    }

    var link = '../' + controller + '/' + action;
    link = encodeURI(link + '?noCahce=' + new Date() + '&export=' + obj);

    var $inputs = $('#form0 :input');

    // not sure if you wanted this, but I thought I'd add it.
    // get an associative array of just the values.
    $inputs.each(function () {
        if (this.id != '' && this.id != null)
            link += '&' + this.id + '=' + $(this).val()
    });

    link = encodeURI(link)
    window.location = link;

    //$("#export").val(obj);
    //return true;
}

function Buscar(action, DivId) {
    RedisplayParam();
    Refresh(0, getPageSize(), action, DivId);
};

function LoadParam() {
    window.location = 'Browse?index=0&pageSize=' + getPageSize() + '&descricao=' + $("#reportName").val();
};

function SaveParam(DivId) {

    var link = 'SaveParam';
    link = encodeURI(link + '?index=0&pageSize=' + getPageSize());

    var $inputs = $('#form0 :input');

    // not sure if you wanted this, but I thought I'd add it.
    // get an associative array of just the values.
    $inputs.each(function () {
        if (this.id != '' && this.id != null)
            link += '&' + this.id + '=' + $(this).val()
    });

    link = encodeURI(link + '&noCahce=' + new Date())
    RedisplayParam();
    $('#' + DivId).load(link);
};

function DoColapse(colapse, glyph) {
    if ($("#" + colapse).val() == "in") {
        $("#" + glyph).removeClass("icon-plus-sign");
        $("#" + glyph).addClass("icon-minus-sign");
        $("#" + colapse).val("out");
    }
    else {
        $("#" + glyph).removeClass("icon-minus-sign");
        $("#" + glyph).addClass("icon-plus-sign");
        $("#" + colapse).val("in");
    }
}

function getPageSize() {
    return 50;
}

function MoveId(_descricao, id, data) {
    $('#' + id).val("");
    for (i in data.countries) {
        if (data.countries[i].descricao == _descricao.value)
            $('#' + id).val(data.countries[i].objId);
    };
}


function validateTypeahead(obj, temp, id) {
    if ($("#" + temp).val().trim() != $("#" + obj).val().trim()) {
        $("#" + obj).val("");
        $("#" + id).val("0");
        $("#" + temp).val("");
    }
}

function GetTypeahead(_id, _text, _lovModal, _controller) {
    var url = '../' + _controller + '/GetTypeahead?id=' + _id + '&text=' + _text + '&lovModal=' + _lovModal;
    url = encodeURI(url);
    $('#typeahead-div-' + _id).load(url);
};

function datepickerShow(id) {
    $(id).datepicker({
        language: 'pt-BR',
        format: 'dd/mm/yyyy',
        autoclose: true,
        keyboardNavigation: false
    });
    $(id).datepicker('show');
}

function showLookup(lovModal, DivId) {
    var divLov = "div-lov";
    if (DivId != "" && DivId != "undefined" && DivId != undefined && DivId != null)
        divLov = DivId;

    $('#carregando').css("visibility", "visible");
    $('#carregando').css("width", "100%");
    $('#carregando').css("height", "100%");
    $('#carregando').css("position", "absolute");
    $('#carregando').css("background-color", "black");
    $('#carregando').css("filter", "alpha(opacity=60)");
    $('#carregando').css("opacity", "0.6");
    $('#carregando').css("left", "0%");
    $('#carregando').css("top", "0%");

    $.ajax({
        type: "GET",
        url: "../Home/" + lovModal,
        data: {
            index: 0,
            pageSize: 50,
            hoje: new Date()
        },
        success: function (data) {
            $('#' + divLov).html(data);
            $('#msgs').html("");
            $('#carregando').css("visibility", "hidden");
        }
    });
}

function showFiltroPesquisa(lovModal, DivId, controller, action) {
    var divLov = "div-lov";
    if (DivId != "" && DivId != "undefined" && DivId != undefined && DivId != null)
        divLov = DivId;
    $.ajax({
        type: "GET",
        url: "../Home/" + lovModal,
        data: {
            index: 0,
            pageSize: 50,
            _controller: controller,
            _action: action
        },
        success: function (data) {
            $('#' + divLov).html(data);
            $('#msgs').html("");
        }
    });
}

function showMyLovModal(lovModal, DivId, report, controller, action) {
    var divLov = "div-lov";
    if (DivId != "" && DivId != "undefined" && DivId != undefined && DivId != null)
        divLov = DivId;
    $.ajax({
        type: "GET",
        url: "../Home/" + lovModal,
        data: {
            index: 0,
            pageSize: 50,
            _report: report,
            _controller: controller,
            _action: action
        },
        success: function (data) {
            $('#' + divLov).html(data);
            $('#msgs').html("");
        }
    });
}


function InsertModal(crudModal, fieldName, id, temp, text, nextField) {
    // crudModal => Nome do controller/action que irá fazer a inclusão
    // fieldName => Nome do campo do repository que retorna o conteúdo da descrição
    // id => nome do hidden field que armazenará o ID do objeto incluído
    // temp => nome do hidden field que armazenará a descrição
    // text => nome do objeto que receberá o texto incluído
    // nextField => nome do próximo objeto a ser movido o foco
    $.ajax({
        type: "get",
        url: crudModal,
        data: {
            descricao: $('#' + text).val()
        },
        success: function (data) {
            var map = {};
            $.each(data, function (i, repository) {
                map[i] = repository;
            });
            $('#' + id).val(map[id]);
            $('#' + temp).val(map[fieldName]);
            $('#' + text).val(map[fieldName]);
            $('#' + nextField).focus();

            if (map['mensagem'].MessageBase != '') {
                $("#catarina").text(map['mensagem'].MessageBase);
                $("#linkModal").click();
            }
        }
    });
}


function clean(id, text) {
    $("#" + text).val("");
    $("#" + id).val("");
    $("#" + text + "1").val("");
}

function CleanMiniCrud(id, text, controller, action, DivId) {
    clean(id, text);
    var link = action + "?DivId=" + DivId;
    $('#' + DivId).load(encodeURI(link));
}


function numeros() {
    var tecla = event.keyCode;
    if (tecla > 47 && tecla < 58) // numeros de 0 a 9  ou . (ponto)
        return true;
    else {
        if (tecla != 8 && tecla != 9) // backspace e tab
            return false;
        else
            return true;
    }
}

function numerosPonto() {

    var tecla = event.keyCode;   

    if ((tecla > 47 && tecla < 58) || tecla == 46) // numeros de 0 a 9  ou . (ponto)
        return true;
    else {
        if (tecla != 8) // backspace  
            //event.keyCode = 0;  
            return false;
        else
            return true;
    }
}


function float2moeda(num) {

    x = 0;

    if (num < 0) {
        num = Math.abs(num);
        x = 1;
    } if (isNaN(num)) num = "0";
    cents = Math.floor((num * 100 + 0.5) % 100);

    num = Math.floor((num * 100 + 0.5) / 100).toString();

    if (cents < 10) cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '.'
               + num.substring(num.length - (4 * i + 3)); ret = num + ',' + cents; if (x == 1) ret = ' - ' + ret; return ret;

}

function FormataNumero(campo) {
    vr = campo.value;
    sinal = vr.substr(0, 1);
    vr = vr.replace("-", "");
    vr = vr.replace("/", "");
    vr = vr.replace(",", "");
    vr = vr.replace(".", "");
    tam = vr.length;

    if (tam == 0)
        campo.value = vr + "0,00";

    if (tam <= 2 && tam > 0) {
        campo.value = vr + ",00";
    }
    if ((tam > 2) && (tam <= 5)) {
        campo.value = vr.substr(0, tam - 2) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 6) && (tam <= 8)) {
        campo.value = vr.substr(0, tam - 5) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 9) && (tam <= 11)) {
        campo.value = vr.substr(0, tam - 8) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 12) && (tam <= 14)) {
        campo.value = vr.substr(0, tam - 11) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 15) && (tam <= 17)) {
        campo.value = vr.substr(0, tam - 14) + '.' + vr.substr(tam - 14, 3) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }

    if (sinal == "-") {
        campo.value = "-" + campo.value
    }
}



function isNumeric(source) {

    var count;
    var e;
    var i;
    var chars;
    var flag;
    flag = true;
    count = source.value.length;
    i = 0;
    chars = "0123456789-,.";

    while (i <= count - 1) {
        e = source.value.substr(i, 1);
        if (chars.indexOf(e) == -1) {
            flag = false;
            i = count + 2;
        }
        i = i + 1;

    }

    return flag;

}

//Esta função deve ser chamada no evento onblur do textbox
// Exemplo: @onblur = "return fnValidaValor(this);"
function fnValidaValor(source) {
    var obj;
    obj = source; // document.forms[0].item(source);

    if (!isNumeric(obj)) {
        alert("Valor deve ser preenchida com números");
        obj.value = "0,00";
        obj.focus();
        return false;
    }
    FormataNumero(obj);

    return true;
}

function Converte(obj, objFor) {
    var flag = fnValidaValor(obj);
    if (flag) {
        $("#" + objFor).val(parseFloat($("#" + objFor + "_1").val().replace(".", "").replace(",", "")) / 100);
        $("#" + objFor).val($("#" + objFor).val().replace(".", ","));
    }
    return flag;
}

function FormatDate(obj) {
    var from = obj.split("/");
    var f = from[2] + '-' + from[1] + '-' + from[0];
    return f;
}

