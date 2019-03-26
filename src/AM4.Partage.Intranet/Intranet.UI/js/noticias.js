var noticias = {
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

        $("#divNoticias").html("<img src='/images/carregando.gif'/>");
        $("#divNoticias").load("/Noticias/Lista", function () {
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
                $(this).css("background-image", "url('/Content/Noticias/" + $(this).data("imagem") + "')");
            });

            $("#frmBuscaNoticias").unbind('submit').submit(function () {
                $("#divNoticias").html("<img src='/images/carregando.gif'/>");
                $("#divNoticias").load("/Noticias/Lista", { busca: $("#palavrachave").val(), idCategoria: $("#Categoria").val()}, function () {
                    init();
                });
                return false;
            });
        };

        init();
    }
};