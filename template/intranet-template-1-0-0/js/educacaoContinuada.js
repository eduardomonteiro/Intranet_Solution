var educacaoContinuada = {
    init: function () {
        var atual = 1;
        var forpages = 4;
        var $element = $('li[data-edu="todos"] article');
        var l = $element.length;
        $element.hide();

        var change = function () {
            $element.slice(0, (atual * forpages)).fadeIn();

            if ((atual * forpages) >= l) {
                $(".btn-carregarmais").hide();
            }

            return false;
        };

        $("#divCursos").html("<img src='/images/carregando.gif'/>");
        $("#divCursos").load("/EducacaoContinuada/Lista", function () {
            init();
        });

        var init = function () {
            edit.init();
            atual = 1;
            $element = $('li[data-edu="todos"] article');
            l = $element.length;
            $element.hide();
            change();

            $(".btn-carregarmais").unbind('click').click(function () {
                atual++;
                change();
            });

            $("#frmBuscaCursos").unbind('submit').submit(function () {
                $("#divCursos").html("<img src='/images/carregando.gif'/>");
                $("#divCursos").load("/EducacaoContinuada/Lista", { busca: $("#palavrachave").val(), categoria: $("#Categoria").val(), realizado: $("#Realizado").val() }, function () {
                    init();
                });
                return false;
            });

            $("#btnRealizados").unbind('click').click(function () {
                $('.filter-area button').each(function () {
                    $('.active').removeClass("active");
                })
                $(this).addClass("active");
                $("#divCursos").html("<img src='/images/carregando.gif'/>");
                $("#divCursos").load("/EducacaoContinuada/Lista", { busca: $("#palavrachave").val(), categoria: $("#Categoria").val(), realizado: true }, function () {
                    init();
                });
            });

            $("#btnNRealizados").unbind('click').click(function () {
                $('.filter-area button').each(function () {
                    $('.active').removeClass("active");
                })
                $(this).addClass("active");
                $("#divCursos").html("<img src='/images/carregando.gif'/>");
                $("#divCursos").load("/EducacaoContinuada/Lista", { busca: $("#palavrachave").val(), categoria: $("#Categoria").val(), realizado: false }, function () {
                    init();
                });
            });

            $("#btnTodos").unbind('click').click(function () {
                $('.filter-area button').each(function () {
                    $('.active').removeClass("active");
                })
                $(this).addClass("active");
                $("#divCursos").html("<img src='/images/carregando.gif'/>");
                $("#divCursos").load("/EducacaoContinuada/Lista", { busca: $("#palavrachave").val(), categoria: $("#Categoria").val() }, function () {
                    init();
                });
            });
        };

        init();
    }
};