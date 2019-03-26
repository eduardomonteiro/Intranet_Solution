var busca = {
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

        var init = function () {
            atual = 1;
            $element = $('.list-links .item');
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