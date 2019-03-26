var usuarios = {
    init: function () {
        var atual = 1;
        var forpages = 6;
        var $element = $('.list-colaboradores .item');
        var l = $element.length;
        $element.hide();

        var change = function () {
            $element.slice(0, (atual * forpages)).fadeIn();

            if ((atual * forpages) >= l) {
                $(".btn-carregarmais").hide();
            }

            return false;
        };

        $("#divUsuarios").html("<img src='/images/carregando.gif'/>");
        $("#divUsuarios").load("/Colaboradores/Lista", function () {
            init();
        });

        var init = function () {
            edit.init();    
            atual = 1;
            $element = $('.list-colaboradores .item');
            l = $element.length;
            $element.hide();
            change();

            $(".btn-carregarmais").unbind('click').click(function () {
                atual++;
                change();
            });

            $("#frmBuscaUsuarios").unbind('submit').submit(function () {
                $("#divUsuarios").html("<img src='/images/carregando.gif'/>");
                $("#divUsuarios").load("/Colaboradores/Lista", { busca: $("#palavrachave").val(), ordenacao: $("#Ordenacao").val()}, function () {
                    init();
                });
                return false;
            });
        };

        init();
    }
};