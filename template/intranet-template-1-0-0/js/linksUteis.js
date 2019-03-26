var linksUteis = {
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

        $("#divLinks").html("<img src='/images/carregando.gif'/>");
        $("#divLinks").load("/LinksUteis/Lista", function () {
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

            $("#frmBuscaLinks").unbind('submit').submit(function () {
                $("#divLinks").html("<img src='/images/carregando.gif'/>");
                $("#divLinks").load("/LinksUteis/Lista", { busca: $("#palavrachave").val(), idEmpreendimento: $("#Empreendimento").val()}, function () {
                    init();
                });
                return false;
            });
        };

        init();
    }
};