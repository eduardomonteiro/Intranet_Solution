var albuns = {
    init: function () {
        var atual = 1;
        var forpages = 8;
        var $element = $('.list-noticias article');
        var l = $element.length;
        $element.hide();

        var change = function () {
            $element.slice(0, (atual * forpages)).fadeIn();

            if ((atual * forpages) >= l) {
                $(".btn-carregarmais").hide();
            }

            return false;
        };

        $("#divAlbuns").html("<img src='/images/carregando.gif'/>");
        $("#divAlbuns").load("/Albuns/Lista", function () {
            init();
        });

        var init = function () {
            edit.init();
            atual = 1;
            $element = $('.list-noticias article');
            l = $element.length;
            $element.hide();
            change();

            $(".btn-carregarmais").unbind('click').click(function () {
                atual++;
                change();
            });

            $("#frmBuscaAlbuns").unbind('submit').submit(function () {
                $("#divAlbuns").html("<img src='/images/carregando.gif'/>");
                $("#divAlbuns").load("/Albuns/Lista", { busca: $("#Busca").val(), idEmpreendimento: $("#Empreendimento").val() }, function () {
                    init();
                });
                return false;
            });
        };

        init();
    }
};