var documentos = {
    init: function (area) {
        var atual = 1;
        var forpages = 4;
        var $element = $('.list-arquivos .col');
        var l = $element.length;
        $element.hide();

        var change = function () {
            $element.slice(0, (atual * forpages)).fadeIn();

            if ((atual * forpages) >= l) {
                $(".btn-carregarmais").hide();
            }

            return false;
        };

        $("#divDoc").html("<img src='/images/carregando.gif'/>");
        $("#divDoc").load("/Documentos/Lista/" + area, function () {
            init();
        });

        var init = function () {
            edit.init();
            atual = 1;
            $element = $('.list-arquivos .col');
            l = $element.length;
            $element.hide();
            change();

            $(".btn-carregarmais").unbind('click').click(function () {
                atual++;
                change();
            });

            $("#frmBuscaDoc").unbind('submit').submit(function () {
                $("#divDoc").html("<img src='/images/carregando.gif'/>");
                $("#divDoc").load("/Documentos/Lista/" + area, { busca: $("#palavrachave").val(), idEmpreendimento: $("#Empreendimento").val(), idCategoria: $("#Categoria").val() }, function () {
                    init();
                });
                return false;
            });
        };

        init();
    }
};