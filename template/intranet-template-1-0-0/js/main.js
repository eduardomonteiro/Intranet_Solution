var datepickerBR = {
    init: function () {
        $.datepicker.regional['pt-br'] = {
            closeText: 'Fechar',
            prevText: 'Anterior',
            nextText: 'Seguinte',
            currentText: 'Hoje',
            monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
            monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
            dayNames: ['Domingo', 'Segunda-feira', 'Ter&ccedil;a-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'S&aacute;bado'],
            dayNamesShort: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
            dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
            weekHeader: 'Sem',
            dateFormat: 'dd/mm/yy',
            firstDay: 0,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['pt-br']);
    }
};

var menu = {
    init: function () {
        $('.list-menu li .item').click(function () {
            if ($(this).hasClass('ative')) {
                $(this).removeClass('ative');
                $('.list-menu li .sub-itens').slideUp(400);
                $(this).parent().find('.sub-itens').slideUp(400);
            }
            else {
                $('.list-menu li .item').removeClass('ative');
                $('.list-menu li .sub-itens').slideUp(400);
                $(this).addClass('ative');
                $(this).parent().find('.sub-itens').slideDown(400);
            }
        });
    }
};

var submenu = {
    init: function () {
        $(".btn-submenu").click(function () {
            $('.wrap-side').toggleClass('active');
        });
    }
}

var boxperfil = {
    init: function () {
        $(".perfil").click(function (event) {
            event.stopPropagation();
            $('.box-userperfil').slideToggle("50");
            $(this).toggleClass('ativo');
        });
        $(window).click(function() {

             $('.box-userperfil').slideUp("50");
             $('.perfil').removeClass('ativo');
        });
    }
}

var featured = {
    init: function () {
        $('.ban-featured').slick({
            dots: true,
            arrows: false,
            infinite: true,
            speed: 500,
            slidesToShow: 1,
            adaptiveHeight: true
        });
    }
}

var communicated = {
    init: function () {
        $('.list-communicated').slick({
            dots: true,
            arrows: false,
            infinite: true,
            speed: 500,
            slidesToShow: 3,
            slidesToScroll: 3,
            responsive: [
                {
                    breakpoint: 1300,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        infinite: true,
                        dots: true
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }

            ]
        });
    }
}

var homesystem = {
    init: function () {
        $('.list-system').slick({
            dots: true,
            arrows: false,
            infinite: true,
            speed: 500,
            slidesToShow: 3,
            slidesToScroll: 3,
            responsive: [
                {
                    breakpoint: 1300,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        infinite: true,
                        dots: true
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
    }
}

var homenews = {
    init: function () {
        $('.list-news').slick({
            dots: false,
            arrows: false,
            infinite: true,
            speed: 500,
            slidesToShow: 4,
            slidesToScroll: 4,
            responsive: [
                {
                    breakpoint: 1300,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        infinite: true,
                        dots: true
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }

            ]
        });
    }
}

var edit = {
    init: function () {
        $(".edits .icon-edits").unbind("click").click(function () {
            $(this).parent().find('.subedit').slideToggle("50");
            $(this).toggleClass('active');
        });

        $(".btnEditar").unbind("click").click(function () {
            document.location = $(this).data("link");
        });

        $(".btnExcluir").unbind("click").click(function () {
            var link = $(this).data("link");

            swal({
                title: 'Deseja realmente excluir esse item?',
                html: "<h5>Essa opera&ccedil;&atilde;o n&atilde;o poder&aacute; ser desfeita.",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#ed1b36',
                cancelButtonColor: '#cfcfcf',
                confirmButtonText: 'Sim',
                cancelButtonText: 'N&atilde;o'
            }).then(function () {
                $.post(link, function (data) {
                    swal(
                        'Ok!',
                        'O item foi deletado com sucesso',
                        'success'
                    ).then(function () {
                        location.reload();
                    });
                }).fail(function () {
                    swal(
                        'Opss!',
                        'Ocorreu um erro ao deletar o item',
                        'error'
                    );
                });
            });
        });
    }
}

var falecomrh = {
    init: function () {
        $('.filter-area button').on('click', function () {
            $('.filter-area button').each(function () {
                $('.active').removeClass("active");
            })
            $(this).addClass("active");
            $('.list-fale ul li').fadeOut(0);
            var fale = $(this).data('fale');
            $('.list-fale ul li[data-fale="' + fale + '"]').fadeIn(400);

            $('.list-edu li').fadeOut(0);
            var edu = $(this).data('edu');
            $('.list-edu li[data-edu="' + edu + '"]').fadeIn(400);
        });
    }
}

var isotope = {
    init: function () {
        $('.list-fotos').isotope({
            itemSelector: 'figure',
            percentPosition: true,
            masonry: {
                columnWidth: 'figure'
            }
        });
    }
}

var listcheck = {
    init: function () {
        $(".list-check .item").click(function () {
            if ($(this).find('input').is(':checked')) {
                $(this).addClass("ativo");
            } else {
                $(this).removeClass("ativo");
            }
        });

        $(".list-check.radio .item").click(function () {
            $(this).parent(".list-check.radio .item").removeClass("ativo");
            if ($(this).find('input').is(':checked')) {
                $(this).addClass("ativo");
            } else {
                $('.list-check.radio .item').removeClass("ativo");
            }
        });
    }
}

var filtermobile = {
    init: function () {
        $(".mobile-filter").click(function () {
            $('.bloco-filter').slideToggle("50");
            $(this).toggleClass('ativo');
        });
    }
}
jQuery(function () {
    menu.init();
    submenu.init();
    featured.init();
    communicated.init();
    homesystem.init();
    homenews.init();
    edit.init();
    falecomrh.init();
    boxperfil.init();
    filtermobile.init();

    listcheck.init();

    $('.list-botoes .btn-form .fa-times').parent().click(function (e) { e.preventDefault(); history.back(); })
});