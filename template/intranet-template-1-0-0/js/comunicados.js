var comunicados = {
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

        $("#divComunicados").html("<img src='/images/carregando.gif'/>");
        $("#divComunicados").load("/Comunicados/Lista", function () {
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

            $(".image").each(function() {
                $(this).css("background-image", "url('/Content/Comunicados/" + $(this).data("imagem") + "')");
            });

            $("#frmBuscaComunicados").unbind('submit').submit(function () {
                $("#divComunicados").html("<img src='/images/carregando.gif'/>");
                $("#divComunicados").load("/Comunicados/Lista", { busca: $("#palavrachave").val(), tipo: $("#Tipo").val()}, function () {
                    init();
                });
                return false;
            });
        };

        init();
    }
};