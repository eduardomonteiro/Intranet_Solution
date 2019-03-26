var albumFotos = {
    init: function (id) {
        var atual = 1;
        var forpages = 12;
        var $element = $('.list-fotos figure');
        var l = $element.length;
        $element.hide();

        var change = function () {
            $element.slice(0, (atual * forpages)).fadeIn();

            setTimeout("isotope.init()", 50);

            if ((atual * forpages) >= l) {
                $(".btn-carregarmais").hide();
            }

            return false;
        };

        $(".content-album").html("<img src='/images/carregando.gif'/>");
        $(".content-album").load("/Albuns/ListaFotos/" + id, function () {
            init();
        });

        var init = function () {
            edit.init();
            atual = 1;
            $element = $('.list-fotos figure');
            l = $element.length;
            $element.hide();
            change();

            $(".btn-carregarmais").unbind('click').click(function () {
                atual++;
                change();
            });
        };

        init();
    }
};