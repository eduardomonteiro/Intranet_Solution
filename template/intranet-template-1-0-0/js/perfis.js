var perfis = {
    init: function () {
        var atual = 1;
        var forpages = 4;
        var $element = $('.list-links .item');
        var l = $element.length;
        $element.hide();

        var change = function () {
            $element.slice(0, (atual * forpages)).fadeIn();

            if ((atual * forpages) >= l) {
                $(".btn-carregarmais").hide();
            }

            return false;
        };

        $("#divPerfis").html("<img src='/images/carregando.gif'/>");
        $("#divPerfis").load("/Perfis/Lista", function () {
            init();
        });

        var init = function () {
            edit.init();    
            atual = 1;
            $element = $('.list-links .item');
            l = $element.length;
            $element.hide();
            change();

            $(".btn-carregarmais").unbind('click').click(function () {
                atual++;
                change();
            });

            $("#frmBuscaPerfis").unbind('submit').submit(function () {
                $("#divPerfis").html("<img src='/images/carregando.gif'/>");
                $("#divPerfis").load("/Perfis/Lista", { busca: $("#palavrachave").val()}, function () {
                    init();
                });
                return false;
            });
        };

        init();
    }
};