var contato = {
    init: function (area) {
        var atual = 1;
        var forpages = 4;
        var $element = $('.list-fale .item');
        var l = $element.length;
        $element.hide();

        var change = function () {
            $element.slice(0, (atual * forpages)).fadeIn();

            if ((atual * forpages) >= l) {
                $(".btn-carregarmais").hide();
            }

            return false;
        };

        $("#divContato").html("<img src='/images/carregando.gif'/>");
        $("#divContato").load("/Contato/Lista/"+area, function () {
            init();
        });

        var init = function () {
            edit.init();
            atual = 1;
            $element = $('.list-fale .item');
            l = $element.length;
            $element.hide();
            change();

            $(".btn-carregarmais").unbind('click').click(function () {
                atual++;
                change();
            });

            $("#frmBuscaContato").unbind('submit').submit(function () {
                $("#divContato").html("<img src='/images/carregando.gif'/>");
                $("#divContato").load("/Contato/Lista/" + area, { busca: $("#palavrachave").val(), empreendimento: $("#Empreendimento").val(), assunto: $("#Assunto").val() }, function () {
                    init();
                });
                return false;
            });

            $("#btnAbertos").unbind('click').click(function () {
                $('.filter-area button').each(function () {
                    $('.active').removeClass("active");
                })
                $(this).addClass("active");
                $("#divContato").html("<img src='/images/carregando.gif'/>");
                $("#divContato").load("/Contato/Lista/" + area, { busca: $("#Busca").val(), empreendimento: $("#Empreendimento").val(), assunto: $("#Assunto").val(), finalizado: false }, function () {
                    init();
                });
            });

            $("#btnFinalizados").unbind('click').click(function () {
                $('.filter-area button').each(function () {
                    $('.active').removeClass("active");
                })
                $(this).addClass("active");
                $("#divContato").html("<img src='/images/carregando.gif'/>");
                $("#divContato").load("/Contato/Lista/" + area, { busca: $("#Busca").val(), empreendimento: $("#Empreendimento").val(), assunto: $("#Assunto").val(), finalizado: true }, function () {
                    init();
                });
            });

            $("#btnTodos").unbind('click').click(function () {
                $('.filter-area button').each(function () {
                    $('.active').removeClass("active");
                })
                $(this).addClass("active");
                $("#divContato").html("<img src='/images/carregando.gif'/>");
                $("#divContato").load("/Contato/Lista/" + area, { busca: $("#Busca").val(), empreendimento: $("#Empreendimento").val(), assunto: $("#Assunto").val() }, function () {
                    init();
                });
            });
        };

        init();
    }
};